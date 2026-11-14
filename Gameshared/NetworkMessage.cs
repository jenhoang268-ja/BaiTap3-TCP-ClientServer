using System;
namespace GameShared
{

    public static class Commands
    {
        public const string DANGKY = "DANGKY";
        public const string DANGNHAP = "DANGNHAP";
        public const string LAYTHONGTIN = "LAYTHONGTIN";
        public const string DANGXUAT = "DANGXUAT";
    }

    public static class StatusCodes
    {
        // A. Thành công (2xx)
        public const int SUCCESS = 200;
        public const int CREATED = 201;
        public const int NO_CONTENT = 204;

        // B. Lỗi phía Client (4xx)
        public const int BAD_REQUEST = 400;
        public const int UNAUTHORIZED = 401;
        public const int FORBIDDEN = 403;
        public const int NOT_FOUND = 404;
        public const int CONFLICT = 409;
        public const int VALIDATION_ERROR = 422;
        public const int TOO_MANY_REQUESTS = 429;

        // C. Lỗi phía Server (5xx)
        public const int INTERNAL_SERVER_ERROR = 500;
        public const int BAD_GATEWAY = 502;
        public const int SERVICE_UNAVAILABLE = 503;
        public const int GATEWAY_TIMEOUT = 504;
    }

    // Dữ liệu Client gửi tới Server
    public class NetworkRequest
    {
        public string Action { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
    }

    // Thông tin người dùng được phép trả về Client
    public class UserProfile
    {
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string LoginTime { get; set; } = "";
    }

    // Dữ liệu Server trả về
    public class NetworkResponse
    {
        public int StatusCode { get; set; }

        // Cho biết yêu cầu có thành công không
        public bool Success { get; set; }

        // Thông báo gửi về Client
        public string Message { get; set; } = "";

        // Thông tin tài khoản khi đăng nhập thành công
        public UserProfile? Profile { get; set; }
    }
}
