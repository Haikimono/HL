using Proposal.Models;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Proposal.BL
{
    public class ForgetPassBL
    {
        private readonly string _connectionString;

        public ForgetPassBL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ResetPasswordByEmail(string email)
        {
            User user = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT user_id, user_name, password, email_adress FROM [user] WHERE email_adress = @Email", conn);
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 100).Value = email;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = reader["user_id"].ToString(),
                            UserName = reader["user_name"].ToString(),
                            Password = reader["password"].ToString(),
                            UserEmail = reader["email_adress"].ToString()
                        };
                    }
                }
            }

            if (user == null)
            {
                return "該当するユーザーが見つかりませんでした。";
            }

            // 生成随机密码（原文）并加密保存
            var plainPassword = GenerateRandomPassword(8);
            var hashedPassword = HashPasswordSHA256(plainPassword);

            // 更新数据库中的加密密码
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var updateCmd = new SqlCommand("UPDATE [user] SET password = @Password, registration_status = 0 WHERE user_id = @UserId", conn);
                updateCmd.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 256).Value = hashedPassword;
                updateCmd.Parameters.Add("@UserId", System.Data.SqlDbType.VarChar, 50).Value = user.UserId;
                updateCmd.ExecuteNonQuery();
            }

            // 发送邮件（使用原文密码）
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

        /// <summary>
        /// 安全なランダムパスワードを生成します。
        /// </summary>
        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[data[i] % chars.Length];
            }
            return new string(result);
        }

        private string HashPasswordSHA256(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private void SendEmail(string toEmail, string userName, string newPassword)
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
    }
}
