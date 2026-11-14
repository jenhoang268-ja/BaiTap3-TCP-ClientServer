using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using GameShared;

namespace GameServer
{
    public static class InputValidator
    {
        public static string? ValidateRegister(
            NetworkRequest request)
        {
            string username = request.Username ?? "";
            string password = request.Password ?? "";
            string fullName = request.FullName ?? "";
            string email = request.Email ?? "";

            if (!Regex.IsMatch(
                username, @"^[a-zA-Z0-9_]{3,20}$"))
            {
                return "Tên đăng nhập phải từ 3-20 ký tự, " +
                       "chỉ chứa chữ, số và dấu gạch dưới.";
            }

            if (password.Length < 8 ||
                password.Length > 128 ||
                !Regex.IsMatch(password, @"[a-zA-Z]") ||
                !Regex.IsMatch(password, @"[0-9]"))
            {
                return "Mật khẩu phải từ 8-128 ký tự, " +
                       "bao gồm chữ và số.";
            }

            if (fullName.Trim().Length < 2 ||
                fullName.Length > 50)
            {
                return "Họ tên phải từ 2-50 ký tự.";
            }

            if (string.IsNullOrWhiteSpace(email) ||
                email.Length > 100)
            {
                return "Email không hợp lệ.";
            }

            try
            {
                var address = new MailAddress(email);

                if (address.Address != email)
                    return "Email không đúng định dạng.";
            }
            catch
            {
                return "Email không đúng định dạng.";
            }

            return null;
        }
    }
}
