using Proposal.DAL;
using Proposal.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Proposal.BL
{
    public class ForgetPassBL
    {
        private readonly UserDAC _userDAC;

        public ForgetPassBL(string connectionString)
        {
            _userDAC = new UserDAC(connectionString);
        }

        public string ResetPasswordByEmail(string email)
        {
            var user = _userDAC.GetUserByEmail(email);
            if (user == null)
                return "該当するユーザーが見つかりませんでした。";

            var plainPassword = GenerateRandomPassword(8);
            //暂时禁用
            //var hashedPassword = HashPasswordSHA256(plainPassword);
            //_userDAC.UpdateUserPassword(user.UserId, hashedPassword);
            _userDAC.UpdateUserPassword(user.UserId, plainPassword);

            try
            {
                SendEmail(user.UserEmail, user.UserName, plainPassword);
                return $"新しいパスワードを {user.UserEmail} に送信しました。";
            }
            catch (Exception ex)
            {
                return "メールの送信に失敗しました：" + ex.Message;
            }
        }

        public string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[data[i] % chars.Length];

            return new string(result);
        }

        public void SendEmail(string toEmail, string userName, string newPassword)
        {
            var fromEmail = "your-email@example.com";
            var fromPassword = "your-email-password";

            var smtp = new SmtpClient("smtp.example.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = "【提案システム】パスワード再設定",
                Body = $"ユーザー {userName} 様\n\n新しいパスワード: {newPassword}\n\nログイン後、すぐにパスワードを変更してください。",
                BodyEncoding = Encoding.UTF8
            };

            smtp.Send(mail);
        }


        public bool ChangeUserPassword(string userId, string newPassword, string confirmPassword, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                error = "すべての項目を入力してください。";
                return false;
            }

            if (newPassword != confirmPassword)
            {
                error = "パスワードが一致しません。";
                return false;
            }
            //暂时禁用
            //string hashedPassword = HashPasswordSHA256(newPassword);
            //_userDAC.UpdatePasswordAndActivate(userId, hashedPassword);
            _userDAC.UpdatePasswordAndActivate(userId, newPassword);
            return true;
        }


        public string HashPasswordSHA256(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
