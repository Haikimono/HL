using Proposal.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                var cmd = new SqlCommand("SELECT user_id, user_name, password, user_kbn, email FROM M_user WHERE email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = reader["user_id"].ToString(),
                            UserName = reader["user_name"].ToString(),
                            Password = reader["password"].ToString(),
                            UserKbn = reader["user_kbn"].ToString(),
                            UserEmail = reader["email"].ToString()
                        };
                    }
                }
            }

            if (user == null)
            {
                return "該当するユーザーが見つかりませんでした。";
            }

            // 生成随机密码
            var newPassword = GenerateRandomPassword(10);

            // 更新密码
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var updateCmd = new SqlCommand("UPDATE M_user SET password = @Password WHERE user_id = @UserId", conn);
                updateCmd.Parameters.AddWithValue("@Password", newPassword);
                updateCmd.Parameters.AddWithValue("@UserId", user.UserId);
                updateCmd.ExecuteNonQuery();
            }

            // 发邮件
            try
            {
                SendEmail(user.UserEmail, user.UserName, newPassword);
                return $"新しいパスワードを {user.UserEmail} に送信しました。";
            }
            catch (Exception ex)
            {
                return "メールの送信に失敗しました：" + ex.Message;
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            var rand = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
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
