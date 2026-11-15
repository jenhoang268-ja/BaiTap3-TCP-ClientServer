using System.IO;
using System.Text.Json;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using GameShared;

namespace GameServer.DataAccess
{
    public class UserDAO
    {
        private readonly string chuoiKetNoi;


        public UserDAO()
        {
            // Đường dẫn đến appsettings.json trong thư mục chạy
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "appsettings.json"
            );

            // Kiểm tra file cấu hình
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy file appsettings.json.",
                    filePath
                );
            }

            // Đọc nội dung JSON
            string json = File.ReadAllText(filePath);

            using JsonDocument document =
                JsonDocument.Parse(json);

            // Đọc ConnectionStrings
            if (!document.RootElement.TryGetProperty(
                "ConnectionStrings", out JsonElement connections))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy mục ConnectionStrings."
                );
            }

            // Đọc GameDatabase
            if (!connections.TryGetProperty(
                "GameDatabase", out JsonElement database))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy chuỗi kết nối GameDatabase."
                );
            }

            chuoiKetNoi = database.GetString() ?? "";

            if (string.IsNullOrWhiteSpace(chuoiKetNoi))
            {
                throw new InvalidOperationException(
                    "Chuỗi kết nối Database đang bị trống."
                );
            }
        }


        public async Task<bool> KiemTraKetNoiDatabaseAsync()
        {
            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);

                await conn.OpenAsync();

                return conn.State == ConnectionState.Open;
            }
            catch (SqlException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }


        // 1. Kiểm tra username tồn tại
        public async Task<bool> KiemTraTenDangNhapTonTaiAsync(
            string tenDangNhap)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM Users
                WHERE TenDangNhap = @ten";

            using var conn = new SqlConnection(chuoiKetNoi);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(
                "@ten", SqlDbType.NVarChar, 20)
                .Value = tenDangNhap;

            await conn.OpenAsync();

            int count = Convert.ToInt32(
                await cmd.ExecuteScalarAsync());

            return count > 0;
        }

        // 2. Đăng ký tài khoản mới
        public async Task<bool> DangKyUserAsync(
            string tenDangNhap,
            string matKhau,
            string hoTen,
            string email)
        {
            // Mật khẩu chỉ được băm ở Server
            var (salt, hash) = MatKhau.Tao(matKhau);

            const string sql = @"
                INSERT INTO Users
                (
                    TenDangNhap,
                    MatKhauBam,
                    Salt,
                    HoTen,
                    Email
                )
                VALUES
                (
                    @ten,
                    @hash,
                    @salt,
                    @hoTen,
                    @email
                )";

            using var conn = new SqlConnection(chuoiKetNoi);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(
                "@ten", SqlDbType.NVarChar, 20)
                .Value = tenDangNhap;

            cmd.Parameters.Add(
                "@hash", SqlDbType.NVarChar, 64)
                .Value = hash;

            cmd.Parameters.Add(
                "@salt", SqlDbType.NVarChar, 32)
                .Value = salt;

            cmd.Parameters.Add(
                "@hoTen", SqlDbType.NVarChar, 50)
                .Value = hoTen;

            cmd.Parameters.Add(
                "@email", SqlDbType.NVarChar, 100)
                .Value = email;

            await conn.OpenAsync();

            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }

        // 3. Kiểm tra đăng nhập
        public async Task<bool> KiemTraDangNhapAsync(
            string tenDangNhap,
            string matKhau)
        {
            const string sql = @"
                SELECT MatKhauBam, Salt
                FROM Users
                WHERE TenDangNhap = @ten";

            using var conn = new SqlConnection(chuoiKetNoi);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(
                "@ten", SqlDbType.NVarChar, 20)
                .Value = tenDangNhap;

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return false;
            }

            string storedHash =
                reader["MatKhauBam"].ToString() ?? "";

            string salt =
                reader["Salt"].ToString() ?? "";

            return MatKhau.KiemTra(
                matKhau,
                salt,
                storedHash);
        }

        // 4. Lấy thông tin người dùng
        public async Task<UserProfile?> LayThongTinUserAsync(
            string tenDangNhap)
        {
            const string sql = @"
                SELECT
                    TenDangNhap,
                    HoTen,
                    Email,
                    LanDangNhapCuoi
                FROM Users
                WHERE TenDangNhap = @ten";

            using var conn = new SqlConnection(chuoiKetNoi);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(
                "@ten", SqlDbType.NVarChar, 20)
                .Value = tenDangNhap;

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            string loginTime = "";

            if (reader["LanDangNhapCuoi"] != DBNull.Value)
            {
                loginTime = Convert.ToDateTime(
                    reader["LanDangNhapCuoi"])
                    .ToString("dd/MM/yyyy HH:mm");
            }

            return new UserProfile
            {
                Username =
                    reader["TenDangNhap"].ToString() ?? "",
                FullName =
                    reader["HoTen"].ToString() ?? "",
                Email =
                    reader["Email"].ToString() ?? "",
                LoginTime = loginTime
            };
        }

        // 5. Cập nhật lần đăng nhập cuối
        public async Task CapNhatLanDangNhapCuoiAsync(
            string tenDangNhap)
        {
            const string sql = @"
                UPDATE Users
                SET LanDangNhapCuoi = SYSDATETIME()
                WHERE TenDangNhap = @ten";

            using var conn = new SqlConnection(chuoiKetNoi);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(
                "@ten", SqlDbType.NVarChar, 20)
                .Value = tenDangNhap;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
