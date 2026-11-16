using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameShared;
using GameServer.DataAccess;
using Microsoft.Data.SqlClient;

namespace GameServer
{
    public class GameTcpServer
    {
        private const int PORT = 8006;
        private const int MAX_MESSAGE_LENGTH = 16384;

        private TcpListener? listener;
        private CancellationTokenSource? cts;
        private Task? acceptTask;

        private readonly ConcurrentDictionary<Guid, ClientSession> clients = new();
        private readonly ConcurrentDictionary<Guid, Task> handlers = new();

        private readonly UserDAO dao = new UserDAO();

        // Giữ nguyên tiếng Việt trong JSON trả về (không đổi thành \uXXXX)
        private static readonly JsonSerializerOptions jsonOut = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static readonly JsonSerializerOptions jsonIn = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public event Action<string>? OnLog;
        public event Action<List<ClientInfo>>? OnClientsChanged;
        public event Action<JsonErrorInfo>? OnJsonError;

        public bool IsRunning { get; private set; }

        public int ClientCount => clients.Count;

        private class ClientSession
        {
            public Guid Id { get; } = Guid.NewGuid();
            public TcpClient Client { get; }
            public string? Username { get; set; }
            public DateTime? LoginTime { get; set; }
            public string Address { get; }
            public DateTime ConnectedAt { get; }

            public ClientSession(TcpClient client)
            {
                Client = client;
                Address = client.Client.RemoteEndPoint?.ToString()
                    ?? "Không xác định";
                ConnectedAt = DateTime.Now;
            }
        }

        private void Log(string message)
        {
            OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        private void BaoLoiJson(
            ClientSession session,
            string message,
            string rawData)
        {
            OnJsonError?.Invoke(new JsonErrorInfo
            {
                Time = DateTime.Now,
                ClientAddress = session.Address,
                ErrorCode = StatusCodes.BAD_REQUEST,
                Message = message,
                RawData = rawData
            });

            Log($"400 BAD_REQUEST - JSON lỗi từ {session.Address}");
        }

        public void Start()
        {
            if (IsRunning)
                return;

            cts = new CancellationTokenSource();

            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();

            IsRunning = true;

            Log($"Server đang lắng nghe cổng {PORT}");

            acceptTask = AcceptLoopAsync(cts.Token);
        }

        private void ThongBaoDanhSachClient()
        {
            List<ClientInfo> danhSach =
                clients.Values
                    .Select(session => new ClientInfo
                    {
                        ClientId = session.Id,
                        Username = session.Username ?? "Chưa đăng nhập",
                        Address = session.Address,
                        ConnectedAt = session.ConnectedAt
                    })
                    .OrderBy(x => x.ConnectedAt)
                    .ToList();

            OnClientsChanged?.Invoke(danhSach);
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    TcpClient client =
                        await listener!.AcceptTcpClientAsync(token);

                    ClientSession session = new ClientSession(client);

                    clients[session.Id] = session;
                    ThongBaoDanhSachClient();

                    Log($"Client kết nối: {client.Client.RemoteEndPoint}");
                    Log($"Tổng Client đang kết nối: {ClientCount}");

                    Task task = HandleClientAsync(session, token);

                    handlers[session.Id] = task;

                    _ = task.ContinueWith(
                        completedTask =>
                        {
                            handlers.TryRemove(session.Id, out Task? removedTask);
                        },
                        TaskScheduler.Default);
                }
            }
            catch (OperationCanceledException) { }
            catch (ObjectDisposedException) { }
            catch (SocketException ex)
            {
                if (!token.IsCancellationRequested)
                {
                    Log($"Lỗi Socket: {ex.Message}");
                }
            }
            finally
            {
                listener?.Stop();
                Log("Đã kết thúc vòng lặp nhận kết nối.");
            }
        }

        private async Task HandleClientAsync(
            ClientSession session,
            CancellationToken token)
        {
            using TcpClient client = session.Client;

            try
            {
                using NetworkStream stream = client.GetStream();

                using StreamReader reader =
                    new StreamReader(stream, Encoding.UTF8);

                using StreamWriter writer =
                    new StreamWriter(stream, new UTF8Encoding(false))
                    {
                        AutoFlush = true
                    };

                while (!token.IsCancellationRequested)
                {
                    string? json = await reader.ReadLineAsync(token);

                    // Client đã ngắt kết nối
                    if (json == null)
                        break;

                    // Kiểm tra kích thước thông điệp theo byte UTF-8
                    if (Encoding.UTF8.GetByteCount(json) > MAX_MESSAGE_LENGTH)
                    {
                        BaoLoiJson(
                            session,
                            "Thông điệp vượt quá giới hạn 16KB.",
                            json.Length > 200 ? json[..200] + "..." : json);

                        NetworkResponse sizeErrorResponse = Fail(
                            StatusCodes.BAD_REQUEST,
                            "Thông điệp vượt quá giới hạn 16KB.");

                        Log($"[{session.Address}] (quá lớn) -> " +
                            $"{sizeErrorResponse.StatusCode} - {sizeErrorResponse.Message}");

                        await writer.WriteLineAsync(
                            JsonSerializer.Serialize(sizeErrorResponse, jsonOut));

                        continue;
                    }

                    NetworkRequest? request = null;
                    NetworkResponse response;

                    try
                    {
                        // 1. Deserialize JSON thành NetworkRequest
                        request = JsonSerializer.Deserialize<NetworkRequest>(
                            json, jsonIn);

                        // 2. Kiểm tra đối tượng sau khi Deserialize
                        if (request == null)
                        {
                            BaoLoiJson(
                                session,
                                "Dữ liệu yêu cầu rỗng (null).",
                                json);

                            response = Fail(
                                StatusCodes.BAD_REQUEST,
                                "Dữ liệu yêu cầu không hợp lệ.");
                        }
                        // 3. Kiểm tra mã lệnh
                        else if (string.IsNullOrWhiteSpace(request.Action))
                        {
                            response = Fail(
                                StatusCodes.BAD_REQUEST,
                                "Thiếu mã lệnh Action.");
                        }
                        // 4. Xử lý yêu cầu hợp lệ
                        else
                        {
                            response = await ProcessRequestAsync(session, request);
                        }
                    }
                    catch (JsonException ex)
                    {
                        // JSON sai định dạng hoặc không thể chuyển đổi
                        BaoLoiJson(session, ex.Message, json);

                        response = Fail(
                            StatusCodes.BAD_REQUEST,
                            "Dữ liệu JSON không hợp lệ.");
                    }

                    // Ghi log mã trả về cho mỗi yêu cầu
                    Log($"[{session.Address}] " +
                        $"{request?.Action ?? "(không đọc được)"} -> " +
                        $"{response.StatusCode} - {response.Message}");

                    // Trả kết quả về Client
                    string result = JsonSerializer.Serialize(response, jsonOut);
                    await writer.WriteLineAsync(result);

                    // Sau khi đăng xuất thì đóng kết nối
                    if (request?.Action?.Trim().Equals(
                            Commands.DANGXUAT,
                            StringComparison.OrdinalIgnoreCase) == true)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (IOException ex)
            {
                Log($"Client {session.Id} mất kết nối: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Log($"Lỗi Socket Client {session.Id}: {ex.Message}");
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Log($"Lỗi xử lý Client {session.Id}: {ex.Message}");
            }
            finally
            {
                session.Username = null;

                clients.TryRemove(session.Id, out _);
                ThongBaoDanhSachClient();

                client.Close();

                Log($"Client đã ngắt kết nối. Còn lại: {ClientCount}");
            }
        }

        private async Task<NetworkResponse> ProcessRequestAsync(
            ClientSession session,
            NetworkRequest request)
        {
            try
            {
                string action =
                    (request.Action ?? "").Trim().ToUpperInvariant();

                switch (action)
                {
                    case Commands.DANGKY:
                        return await XuLyDangKyAsync(request);

                    case Commands.DANGNHAP:
                        return await XuLyDangNhapAsync(session, request);

                    case Commands.LAYTHONGTIN:
                        return await XuLyLayThongTinAsync(session);

                    case Commands.DANGXUAT:
                        return XuLyDangXuat(session);

                    default:
                        return Fail(
                            StatusCodes.BAD_REQUEST,
                            "Lệnh không được hỗ trợ.");
                }
            }
            catch (SqlException ex)
                when (ex.Number == 2601 || ex.Number == 2627)
            {
                return Fail(
                    StatusCodes.CONFLICT,
                    "Tên đăng nhập đã tồn tại.");
            }
            catch (SqlException ex)
            {
                Log($"Lỗi SQL: {ex.Message}");

                return Fail(
                    StatusCodes.SERVICE_UNAVAILABLE,
                    "Dịch vụ cơ sở dữ liệu không khả dụng.");
            }
            catch (Exception ex)
            {
                Log($"Lỗi Server: {ex.Message}");

                return Fail(
                    StatusCodes.INTERNAL_SERVER_ERROR,
                    "Server xảy ra lỗi nội bộ.");
            }
        }

        private async Task<NetworkResponse> XuLyDangKyAsync(
            NetworkRequest request)
        {
            string? error = InputValidator.ValidateRegister(request);

            if (error != null)
            {
                return Fail(StatusCodes.VALIDATION_ERROR, error);
            }

            bool exists =
                await dao.KiemTraTenDangNhapTonTaiAsync(request.Username);

            if (exists)
            {
                return Fail(
                    StatusCodes.CONFLICT,
                    "Tên đăng nhập đã tồn tại.");
            }

            bool success =
                await dao.DangKyUserAsync(
                    request.Username,
                    request.Password,
                    request.FullName,
                    request.Email);

            if (!success)
            {
                return Fail(
                    StatusCodes.INTERNAL_SERVER_ERROR,
                    "Đăng ký không thành công.");
            }

            Log($"Đăng ký thành công: {request.Username}");

            return Ok("Đăng ký tài khoản thành công!", StatusCodes.CREATED);
        }

        private async Task<NetworkResponse> XuLyDangNhapAsync(
            ClientSession session,
            NetworkRequest request)
        {
            string username = request.Username ?? "";
            string password = request.Password ?? "";

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password) ||
                username.Length > 20 ||
                password.Length > 128)
            {
                return Fail(
                    StatusCodes.BAD_REQUEST,
                    "Dữ liệu đăng nhập không hợp lệ.");
            }

            bool valid = await dao.KiemTraDangNhapAsync(username, password);

            if (!valid)
            {
                Log("Có yêu cầu đăng nhập thất bại.");

                return Fail(
                    StatusCodes.UNAUTHORIZED,
                    "Sai tên đăng nhập hoặc mật khẩu.");
            }

            await dao.CapNhatLanDangNhapCuoiAsync(username);

            UserProfile? profile = await dao.LayThongTinUserAsync(username);

            if (profile == null)
            {
                return Fail(
                    StatusCodes.INTERNAL_SERVER_ERROR,
                    "Không lấy được thông tin tài khoản.");
            }

            session.Username = username;
            session.LoginTime = DateTime.Now;

            profile.LoginTime =
                session.LoginTime.Value.ToString("dd/MM/yyyy HH:mm");

            ThongBaoDanhSachClient();

            Log($"Đăng nhập thành công: {username}");

            return new NetworkResponse
            {
                StatusCode = StatusCodes.SUCCESS,
                Success = true,
                Message = "Đăng nhập thành công!",
                Profile = profile
            };
        }

        private async Task<NetworkResponse> XuLyLayThongTinAsync(
            ClientSession session)
        {
            if (string.IsNullOrEmpty(session.Username))
            {
                return Fail(
                    StatusCodes.UNAUTHORIZED,
                    "Bạn chưa đăng nhập.");
            }

            UserProfile? profile =
                await dao.LayThongTinUserAsync(session.Username);

            if (profile == null)
            {
                return Fail(
                    StatusCodes.NOT_FOUND,
                    "Không tìm thấy thông tin tài khoản.");
            }

            profile.LoginTime =
                session.LoginTime?.ToString("dd/MM/yyyy HH:mm") ?? "";

            return new NetworkResponse
            {
                StatusCode = StatusCodes.SUCCESS,
                Success = true,
                Message = "Lấy thông tin thành công.",
                Profile = profile
            };
        }

        private NetworkResponse XuLyDangXuat(ClientSession session)
        {
            Log($"Đăng xuất: {session.Username ?? "Chưa đăng nhập"}");

            session.Username = null;
            session.LoginTime = null;
            ThongBaoDanhSachClient();

            return Ok("Đăng xuất thành công!", StatusCodes.NO_CONTENT);
        }

        private static NetworkResponse Ok(
            string message,
            int statusCode = StatusCodes.SUCCESS)
        {
            return new NetworkResponse
            {
                StatusCode = statusCode,
                Success = true,
                Message = message
            };
        }

        private static NetworkResponse Fail(int statusCode, string message)
        {
            return new NetworkResponse
            {
                StatusCode = statusCode,
                Success = false,
                Message = message
            };
        }

        public async Task StopAsync()
        {
            if (!IsRunning)
                return;

            IsRunning = false;

            cts?.Cancel();
            listener?.Stop();

            if (acceptTask != null)
            {
                try
                {
                    await acceptTask;
                }
                catch (OperationCanceledException) { }
            }

            Task[] runningHandlers = handlers.Values.ToArray();

            await Task.WhenAll(runningHandlers);

            cts?.Dispose();
            cts = null;

            Log("Server đã dừng hoàn toàn.");
        }
    }
}