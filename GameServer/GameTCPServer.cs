using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

        // Danh sách Client đang kết nối
        private readonly ConcurrentDictionary<Guid, ClientSession>
            clients = new();

        // Các tác vụ đang phục vụ Client
        private readonly ConcurrentDictionary<Guid, Task>
            handlers = new();

        private readonly UserDAO dao = new UserDAO();

        public event Action<string>? OnLog;
        public event Action<List<ClientInfo>>? OnClientsChanged;

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

                Address = client.Client.RemoteEndPoint?
                    .ToString() ?? "Không xác định";

                ConnectedAt = DateTime.Now;
            }
        }


        private void Log(string message)
        {
            OnLog?.Invoke(
                $"[{DateTime.Now:HH:mm:ss}] {message}");
        }


        public void Start()
        {
            if (IsRunning)
                return;

            cts = new CancellationTokenSource();

            listener = new TcpListener(
                IPAddress.Any, PORT);

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

                        Username = session.Username
                            ?? "Chưa đăng nhập",

                        Address = session.Address,

                        ConnectedAt = session.ConnectedAt
                    })
                    .OrderBy(x => x.ConnectedAt)
                    .ToList();

            OnClientsChanged?.Invoke(danhSach);
        }



        private async Task AcceptLoopAsync(
            CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    TcpClient client =
                        await listener!.AcceptTcpClientAsync(
                            token);

                    ClientSession session =
                        new ClientSession(client);

                    // Lưu Client vào danh sách
                    clients[session.Id] = session;
                    ThongBaoDanhSachClient();

                    Log(
                        $"Client kết nối: " +
                        $"{client.Client.RemoteEndPoint}");

                    Log(
                        $"Tổng Client đang kết nối: " +
                        $"{ClientCount}");

                    // Mỗi Client được phục vụ riêng
                    // Không await tại vòng lặp Accept
                    Task task = HandleClientAsync(
                        session, token);

                    handlers[session.Id] = task;

                    // Xóa task đã hoàn thành
                    _ = task.ContinueWith(
                        completedTask =>
                        {
                            handlers.TryRemove(
                                session.Id,
                                out Task? removedTask);
                        },
                        TaskScheduler.Default);
                }
            }
            catch (OperationCanceledException)
            {
                // Server dừng bình thường
            }
            catch (ObjectDisposedException)
            {
                // Listener đã được đóng
            }
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
                using NetworkStream stream =
                    client.GetStream();

                using StreamReader reader =
                    new StreamReader(
                        stream, Encoding.UTF8);

                using StreamWriter writer =
                    new StreamWriter(
                        stream, new UTF8Encoding(false))
                    {
                        AutoFlush = true
                    };

                // Giữ kết nối để nhận nhiều lệnh
                // Giữ kết nối để nhận nhiều lệnh
                while (!token.IsCancellationRequested)
                {
                    // Đọc một lần duy nhất cho mỗi vòng lặp
                    string? json = await reader.ReadLineAsync(token);

                    // Client đã ngắt kết nối
                    if (json == null)
                    {
                        break;
                    }

                    // Kiểm tra kích thước thông điệp theo byte UTF-8
                    if (Encoding.UTF8.GetByteCount(json) > MAX_MESSAGE_LENGTH)
                    {
                        NetworkResponse sizeErrorResponse = new NetworkResponse
                        {
                            StatusCode = StatusCodes.BAD_REQUEST,
                            Success = false,
                            Message = "Thông điệp vượt quá giới hạn 16KB."
                        };

                        // Sửa lỗi: Serialize đúng biến vừa được tạo ở trên
                        string sizeErrorResult = JsonSerializer.Serialize(sizeErrorResponse);
                        await writer.WriteLineAsync(sizeErrorResult);

                        continue;
                    }

                    NetworkRequest? request = null;
                    NetworkResponse response;


                    try
                    {
                        // 1. Deserialize JSON thành NetworkRequest
                        request = JsonSerializer.Deserialize<NetworkRequest>(
                            json,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        // 2. Kiểm tra đối tượng sau khi Deserialize
                        if (request == null)
                        {
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
                        // 4. Kiểm tra yêu cầu hợp lệ
                        else
                        {
                            response = await ProcessRequestAsync(
                                session,
                                request);
                        }
                    }
                    catch (JsonException)
                    {
                        // JSON sai định dạng hoặc không thể chuyển đổi
                        response = Fail(
                            StatusCodes.BAD_REQUEST,
                            "Dữ liệu JSON không hợp lệ.");
                    }

                    // Trả kết quả về Client
                    string result =
                        JsonSerializer.Serialize(response);

                    await writer.WriteLineAsync(result);

                    // Sau khi đăng xuất thì đóng kết nối
                    if (request?.Action?.Trim().Equals(
                        Commands.DANGXUAT,
                        StringComparison.OrdinalIgnoreCase)
                        == true)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Server đang dừng
            }
            catch (IOException ex)
            {
                Log(
                    $"Client {session.Id} mất kết nối: " +
                    ex.Message);
            }
            catch (SocketException ex)
            {
                Log(
                    $"Lỗi Socket Client {session.Id}: " +
                    ex.Message);
            }
            catch (ObjectDisposedException)
            {
                // Socket đã đóng
            }
            catch (Exception ex)
            {
                Log(
                    $"Lỗi xử lý Client {session.Id}: " +
                    ex.Message);
            }
            finally
            {
                // Xóa trạng thái đăng nhập
                session.Username = null;

                // Xóa Client khỏi danh sách
                clients.TryRemove(session.Id, out _);
                ThongBaoDanhSachClient();

                client.Close();

                Log(
                    $"Client đã ngắt kết nối. " +
                    $"Còn lại: {ClientCount}");
            }
        }


        private async Task<NetworkResponse>
            ProcessRequestAsync(
                ClientSession session,
                NetworkRequest request)
        {
            try
            {
                string action =
                    (request.Action ?? "")
                    .Trim()
                    .ToUpperInvariant();

                switch (action)
                {
                    case Commands.DANGKY:
                        return await XuLyDangKyAsync(
                            request);

                    case Commands.DANGNHAP:
                        return await XuLyDangNhapAsync(
                            session, request);

                    case Commands.LAYTHONGTIN:
                        return await XuLyLayThongTinAsync(
                            session);

                    case Commands.DANGXUAT:
                        return XuLyDangXuat(session);

                    default:
                        return Fail(
                            StatusCodes.BAD_REQUEST,
                            "Lệnh không được hỗ trợ.");
                }
            }
            catch (SqlException ex)
                when (ex.Number == 2601 ||
                      ex.Number == 2627)
            {
                // Trùng khóa UNIQUE
                return Fail(
                    StatusCodes.CONFLICT,
                    "Tên đăng nhập đã tồn tại.");
            }
            catch (SqlException ex)
            {
                // Chỉ ghi chi tiết lỗi vào Server
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
            // Kiểm tra dữ liệu phía Server
            string? error =
                InputValidator.ValidateRegister(request);

            if (error != null)
            {
                return Fail(
                    StatusCodes.VALIDATION_ERROR,
                    error);
            }

            // Kiểm tra tên đăng nhập
            bool exists =
                await dao.KiemTraTenDangNhapTonTaiAsync(
                    request.Username);

            if (exists)
            {
                return Fail(
                    StatusCodes.CONFLICT,
                    "Tên đăng nhập đã tồn tại.");
            }

            // Băm mật khẩu và thêm vào SQL
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

            return Ok(
                "Đăng ký tài khoản thành công!",
                StatusCodes.CREATED);
        }




        private async Task<NetworkResponse> XuLyDangNhapAsync(
                ClientSession session,
                NetworkRequest request)
        {
            string username = request.Username ?? "";
            string password = request.Password ?? "";

            // Kiểm tra dữ liệu đăng nhập
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password) ||
                username.Length > 20 ||
                password.Length > 128)
            {
                return Fail(
                    StatusCodes.BAD_REQUEST,
                    "Dữ liệu đăng nhập không hợp lệ.");
            }

            // Gọi UserDAO kiểm tra mật khẩu
            bool valid =
                await dao.KiemTraDangNhapAsync(
                    username,
                    password);

            if (!valid)
            {
                Log("Có yêu cầu đăng nhập thất bại.");

                return Fail(
                    StatusCodes.UNAUTHORIZED,
                    "Sai tên đăng nhập hoặc mật khẩu.");
            }

            // Cập nhật thời gian đăng nhập
            await dao.CapNhatLanDangNhapCuoiAsync(username);

            // Lấy thông tin người dùng
            UserProfile? profile =
                await dao.LayThongTinUserAsync(
                    username);

            if (profile == null)
            {
                return Fail(
                    StatusCodes.INTERNAL_SERVER_ERROR,
                    "Không lấy được thông tin tài khoản.");
            }

            // Lưu trạng thái đăng nhập
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



        private async Task<NetworkResponse>
            XuLyLayThongTinAsync(
                ClientSession session)
        {
            // Kiểm tra Client đã đăng nhập chưa
            if (string.IsNullOrEmpty(session.Username))
            {
                return Fail( StatusCodes.UNAUTHORIZED, "Bạn chưa đăng nhập.");
            }

            // Truy vấn SQL Server
            UserProfile? profile = await dao.LayThongTinUserAsync(session.Username);

            if (profile == null)
            {
                return Fail(
                    StatusCodes.NOT_FOUND,
                    "Không tìm thấy thông tin tài khoản.");
            }

            profile.LoginTime = session.LoginTime?.ToString("dd/MM/yyyy HH:mm") ?? "";

            return new NetworkResponse
            {
                StatusCode = StatusCodes.SUCCESS,
                Success = true,
                Message = "Lấy thông tin thành công.",
                Profile = profile
            };
        }


        private NetworkResponse XuLyDangXuat(
            ClientSession session)
        {
            Log(
                $"Đăng xuất: " +
                $"{session.Username ?? "Chưa đăng nhập"}");

            // Xóa trạng thái xác thực
            session.Username = null;
            session.LoginTime = null;
            ThongBaoDanhSachClient();

            return Ok(
                "Đăng xuất thành công!",
                StatusCodes.NO_CONTENT);
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



        private static NetworkResponse Fail(
            int statusCode,
            string message)
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

            // Yêu cầu hủy các tác vụ
            cts?.Cancel();

            // Ngừng nhận kết nối
            listener?.Stop();

            // Chờ vòng lặp Accept kết thúc
            if (acceptTask != null)
            {
                try
                {
                    await acceptTask;
                }
                catch (OperationCanceledException)
                {
                }
            }

            // Chờ các tác vụ Client kết thúc
            Task[] runningHandlers =
                handlers.Values.ToArray();

            await Task.WhenAll(runningHandlers);

            cts?.Dispose();
            cts = null;

            Log("Server đã dừng hoàn toàn.");
        }
    }
}

