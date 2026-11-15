using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameShared;

namespace UserManagement
{
    public static class NetworkService 
    {
        private static string serverIP = "127.0.0.1";
        private static int serverPort = 8006;

        private static TcpClient? client;
        private static StreamReader? reader;
        private static StreamWriter? writer;

        // Tránh hai yêu cầu gửi đồng thời
        // trên cùng một kết nối
        private static readonly SemaphoreSlim sendLock =
            new SemaphoreSlim(1, 1);

        private static readonly JsonSerializerOptions jsonOptions =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        public static string ServerIP => serverIP;

        public static int ServerPort => serverPort;

        public static bool IsConnected =>
            client != null &&
            reader != null &&
            writer != null;

        public static void Configure(string ip, int port)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new ArgumentException(
                    "Địa chỉ IP không được để trống.");
            }

            if (port < 1 || port > 65535)
            {
                throw new ArgumentException(
                    "Port phải từ 1 đến 65535.");
            }

            ip = ip.Trim();

            // Không thay đổi đích khi đang kết nối
            if (client != null &&
                (ip != serverIP || port != serverPort))
            {
                throw new InvalidOperationException(
                    "Hãy ngắt kết nối trước khi đổi Server.");
            }

            serverIP = ip;
            serverPort = port;
        }


        private static async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            TcpClient newClient = new TcpClient();

            try
            {
                // Giới hạn thời gian kết nối 10 giây
                using var timeout =
                    new CancellationTokenSource(
                        TimeSpan.FromSeconds(10));

                await newClient.ConnectAsync(
                    serverIP,
                    serverPort,
                    timeout.Token);

                NetworkStream stream =
                    newClient.GetStream();

                StreamReader newReader =
                    new StreamReader(
                        stream,
                        new UTF8Encoding(false));

                StreamWriter newWriter =
                    new StreamWriter(
                        stream,
                        new UTF8Encoding(false))
                    {
                        AutoFlush = true
                    };

                client = newClient;
                reader = newReader;
                writer = newWriter;
            }
            catch
            {
                newClient.Dispose();
                throw;
            }
        }


        public static async Task<NetworkResponse> SendAsync(
            NetworkRequest request)
        {
            await sendLock.WaitAsync();

            try
            {
                // Tự kết nối nếu chưa có TCP
                await ConnectAsync();

                // Chuyển object thành JSON
                string json =
                    JsonSerializer.Serialize(
                        request, jsonOptions);

                // Kiểm tra kích thước thông điệp trước khi gửi
                if (Encoding.UTF8.GetByteCount(json) > 16384)
                {
                    return Error(400, "Thông điệp gửi đi vượt quá giới hạn 16KB.");
                }


                // Gửi một dòng JSON
                await writer!
                    .WriteLineAsync(json)
                    .WaitAsync(TimeSpan.FromSeconds(15));

                // Đọc phản hồi với timeout
                string? responseJson =
                    await reader!
                        .ReadLineAsync()
                        .WaitAsync(TimeSpan.FromSeconds(15));

                if (string.IsNullOrWhiteSpace(responseJson))
                {
                    throw new IOException(
                        "Server đã đóng kết nối hoặc không phản hồi.");
                }

                // Chuyển JSON về NetworkResponse
                NetworkResponse? response =
                    JsonSerializer.Deserialize<NetworkResponse>(
                        responseJson, jsonOptions);

                if (response == null)
                {
                    throw new JsonException(
                        "Phản hồi Server không hợp lệ.");
                }

                // Sau khi đăng xuất, đóng TCP
                if (request.Action == Commands.DANGXUAT)
                {
                    CloseConnection();
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                CloseConnection();

                return Error(
                    503,
                    "Không thể kết nối Server trong thời gian cho phép.");
            }
            catch (TimeoutException)
            {
                CloseConnection();

                return Error(
                    503,
                    "Server phản hồi quá thời gian cho phép.");
            }
            catch (SocketException)
            {
                CloseConnection();

                return Error(
                    503,
                    "Không thể kết nối tới GameServer. " +
                    "Hãy kiểm tra IP, Port và trạng thái Server.");
            }
            catch (IOException)
            {
                CloseConnection();

                return Error(
                    503,
                    "Mất kết nối với GameServer.");
            }
            catch (JsonException)
            {
                CloseConnection();

                return Error(
                    500,
                    "Dữ liệu phản hồi không đúng định dạng.");
            }
            catch (Exception)
            {
                CloseConnection();

                return Error(
                    500,
                    "Đã xảy ra lỗi khi trao đổi dữ liệu.");
            }
            finally
            {
                sendLock.Release();
            }
        }


        private static NetworkResponse Error(
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

        private static void CloseConnection()
        {
            try { writer?.Dispose(); } catch { }
            try { reader?.Dispose(); } catch { }
            try { client?.Dispose(); } catch { }

            writer = null;
            reader = null;
            client = null;
        }

        public static void Disconnect()
        {
            CloseConnection();
        }
    }
}
