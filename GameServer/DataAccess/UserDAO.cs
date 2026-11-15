
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using GameShared;

namespace GameServer.DataAccess
{
    public class UserDAO
    {
        private readonly string chuoiKetNoi;

        // Giới hạn dữ liệu theo cấu trúc bảng Users
        private const int MAX_USERNAME_LENGTH = 20;
        private const int MAX_PASSWORD_HASH_LENGTH = 64;
        private const int MAX_SALT_LENGTH = 32;
        private const int MAX_FULLNAME_LENGTH = 50;
        private const int MAX_EMAIL_LENGTH = 100;

        public UserDAO()
        {
            // 1. Xác định đường dẫn appsettings.json
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "appsettings.json"
            );

            // 2. Kiểm tra file cấu hình
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy file appsettings.json.",
                    filePath
                );
            }

            // 3. Đọc nội dung JSON
            string json = File.ReadAllText(filePath);

            using JsonDocument document = JsonDocument.Parse(json);

            JsonElement root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException(
                    "Nội dung appsettings.json không hợp lệ."
                );
            }

            // 4. Đọc ConnectionStrings
            if (!root.TryGetProperty(
                "ConnectionStrings",
                out JsonElement connections)
                || connections.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy mục ConnectionStrings hợp lệ."
                );
            }

            // 5. Đọc GameDatabase
            if (!connections.TryGetProperty(
                "GameDatabase",
                out JsonElement database)
                || database.ValueKind != JsonValueKind.String)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy chuỗi kết nối GameDatabase hợp lệ."
                );
            }

            chuoiKetNoi = database.GetString() ?? "";

            if (string.IsNullOrWhiteSpace(chuoiKetNoi))
            {
                throw new InvalidOperationException(
                    "Chuỗi kết nối Database đang bị trống."
                );
            }

            // 6. Kiểm tra định dạng chuỗi kết nối
            try
            {
                _ = new SqlConnectionStringBuilder(chuoiKetNoi);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(
                    "Chuỗi kết nối Database không đúng định dạng.",
                    ex
                );
            }
        }

        // Ghi nhận lỗi SQL trong cửa sổ Output -> Debug
        private void GhiLogSql(
            SqlException ex,
            string tenChucNang)
        {
            Debug.WriteLine(
                $"[UserDAO - {tenChucNang}] " +
                $"SQL Error Number: {ex.Number} | " +
                $"Message: {ex.Message}"
            );
        }

        // Kiểm tra dữ liệu đầu vào
        private static void KiemTraChuoi(
            string? giaTri,
            string tenTruong,
            int doDaiToiDa)
        {
            if (string.IsNullOrWhiteSpace(giaTri))
            {
                throw new ArgumentException(
                    $"{tenTruong} không được để trống."
                );
            }

            if (giaTri.Length > doDaiToiDa)
            {
                throw new ArgumentException(
                    $"{tenTruong} không được vượt quá " +
                    $"{doDaiToiDa} ký tự."
                );
            }
        }

        // =====================================================
        // 1. KIỂM TRA KẾT NỐI DATABASE
        // =====================================================

        public async Task<bool> KiemTraKetNoiDatabaseAsync()
        {
            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);

                await conn.OpenAsync();

                return conn.State == ConnectionState.Open;
            }
            catch (SqlException ex)
            {
                GhiLogSql(ex, "KiemTraKetNoiDatabaseAsync");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(
                    $"[UserDAO - Database] {ex.Message}"
                );

                return false;
            }
        }

        // =====================================================
        // 2. KIỂM TRA TÊN ĐĂNG NHẬP ĐÃ TỒN TẠI
        // =====================================================

        public async Task<bool> KiemTraTenDangNhapTonTaiAsync(
            string tenDangNhap)
        {
            KiemTraChuoi(
                tenDangNhap,
                "Tên đăng nhập",
                MAX_USERNAME_LENGTH
            );

            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Users
                WHERE TenDangNhap = @ten";

            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(
                    "@ten",
                    SqlDbType.NVarChar,
                    MAX_USERNAME_LENGTH
                ).Value = tenDangNhap;

                await conn.OpenAsync();

                int count = Convert.ToInt32(
                    await cmd.ExecuteScalarAsync()
                );

                return count > 0;
            }
            catch (SqlException ex)
            {
                GhiLogSql(ex, "KiemTraTenDangNhapTonTaiAsync");
                throw;
            }
        }

        // =====================================================
        // 3. ĐĂNG KÝ TÀI KHOẢN
        // =====================================================

        public async Task<bool> DangKyUserAsync(
            string tenDangNhap,
            string matKhau,
            string hoTen,
            string email)
        {
            KiemTraChuoi(
                tenDangNhap,
                "Tên đăng nhập",
                MAX_USERNAME_LENGTH
            );

            KiemTraChuoi(
                matKhau,
                "Mật khẩu",
                int.MaxValue
            );

            KiemTraChuoi(
                hoTen,
                "Họ tên",
                MAX_FULLNAME_LENGTH
            );

            KiemTraChuoi(
                email,
                "Email",
                MAX_EMAIL_LENGTH
            );

            // Mật khẩu chỉ được băm tại Server
            var (salt, hash) = MatKhau.Tao(matKhau);

            // Kiểm tra kết quả băm trước khi lưu
            KiemTraChuoi(
                hash,
                "Password Hash",
                MAX_PASSWORD_HASH_LENGTH
            );

            KiemTraChuoi(
                salt,
                "Password Salt",
                MAX_SALT_LENGTH
            );

            const string sql = @"
                INSERT INTO dbo.Users
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

            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(
                    "@ten",
                    SqlDbType.NVarChar,
                    MAX_USERNAME_LENGTH
                ).Value = tenDangNhap;

                cmd.Parameters.Add(
                    "@hash",
                    SqlDbType.NVarChar,
                    MAX_PASSWORD_HASH_LENGTH
                ).Value = hash;

                cmd.Parameters.Add(
                    "@salt",
                    SqlDbType.NVarChar,
                    MAX_SALT_LENGTH
                ).Value = salt;

                cmd.Parameters.Add(
                    "@hoTen",
                    SqlDbType.NVarChar,
                    MAX_FULLNAME_LENGTH
                ).Value = hoTen;

                cmd.Parameters.Add(
                    "@email",
                    SqlDbType.NVarChar,
                    MAX_EMAIL_LENGTH
                ).Value = email;

                await conn.OpenAsync();

                int rows = await cmd.ExecuteNonQueryAsync();

                return rows > 0;
            }
            catch (SqlException ex)
            {
                GhiLogSql(ex, "DangKyUserAsync");
                throw;
            }
        }

        // =====================================================
        // 4. KIỂM TRA ĐĂNG NHẬP
        // =====================================================

        public async Task<bool> KiemTraDangNhapAsync(
            string tenDangNhap,
            string matKhau)
        {
            KiemTraChuoi(
                tenDangNhap,
                "Tên đăng nhập",
                MAX_USERNAME_LENGTH
            );

            KiemTraChuoi(
                matKhau,
                "Mật khẩu",
                int.MaxValue
            );

            const string sql = @"
                SELECT MatKhauBam, Salt
                FROM dbo.Users
                WHERE TenDangNhap = @ten";

            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(
                    "@ten",
                    SqlDbType.NVarChar,
                    MAX_USERNAME_LENGTH
                ).Value = tenDangNhap;

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    return false;
                }

                string storedHash =
                    reader["MatKhauBam"].ToString() ?? "";

                string salt =
                    reader["Salt"].ToString() ?? "";

                if (string.IsNullOrWhiteSpace(storedHash)
                    || string.IsNullOrWhiteSpace(salt))
                {
                    Debug.WriteLine(
                        "[UserDAO] Hash hoặc Salt trong Database không hợp lệ."
                    );

                    return false;
                }

                return MatKhau.KiemTra(
                    matKhau,
                    salt,
                    storedHash
                );
            }
            catch (SqlException ex)
            {
                GhiLogSql(ex, "KiemTraDangNhapAsync");
                throw;
            }
        }

        // =====================================================
        // 5. LẤY THÔNG TIN NGƯỜI DÙNG
        // =====================================================

        public async Task<UserProfile?> LayThongTinUserAsync(
            string tenDangNhap)
        {
            KiemTraChuoi(
                tenDangNhap,
                "Tên đăng nhập",
                MAX_USERNAME_LENGTH
            );

            const string sql = @"
                SELECT
                    TenDangNhap,
                    HoTen,
                    Email,
                    LanDangNhapCuoi
                FROM dbo.Users
                WHERE TenDangNhap = @ten";

            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(
                    "@ten",
                    SqlDbType.NVarChar,
                    MAX_USERNAME_LENGTH
                ).Value = tenDangNhap;

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    return null;
                }

                string loginTime = "";

                if (reader["LanDangNhapCuoi"] != DBNull.Value)
                {
                    loginTime = Convert.ToDateTime(
                        reader["LanDangNhapCuoi"]
                    ).ToString("dd/MM/yyyy HH:mm");
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
            catch (SqlException ex)
            {
                GhiLogSql(ex, "LayThongTinUserAsync");
                throw;
            }
        }

        // =====================================================
        // 6. CẬP NHẬT THỜI GIAN ĐĂNG NHẬP CUỐI
        // =====================================================

        public async Task CapNhatLanDangNhapCuoiAsync(
            string tenDangNhap)
        {
            KiemTraChuoi(
                tenDangNhap,
                "Tên đăng nhập",
                MAX_USERNAME_LENGTH
            );

            const string sql = @"
                UPDATE dbo.Users
                SET LanDangNhapCuoi = SYSDATETIME()
                WHERE TenDangNhap = @ten";

            try
            {
                using var conn = new SqlConnection(chuoiKetNoi);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(
                    "@ten",
                    SqlDbType.NVarChar,
                    MAX_USERNAME_LENGTH
                ).Value = tenDangNhap;

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                GhiLogSql(ex, "CapNhatLanDangNhapCuoiAsync");
                throw;
            }
        }
    }
}
