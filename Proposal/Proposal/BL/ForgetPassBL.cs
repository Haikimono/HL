using Microsoft.Extensions.Configuration;
using Proposal.DAL;
using Proposal.Models;
using Proposal.Services;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Proposal.BL
{
    public class ForgetPassBL
    {
        private readonly UserDAC _userDAC;
        private readonly EmailSender _emailSender;

        // コンストラクタ：DB接続とメール送信の設定を初期化
        public ForgetPassBL(string connectionString, IConfiguration configuration)
        {
            _userDAC = new UserDAC(connectionString);

            // appsettings.json からメール設定を読み込む
            var emailConfig = configuration.GetSection("EmailSettings");
            var smtpHost = emailConfig["SmtpHost"];
            var smtpPort = int.Parse(emailConfig["SmtpPort"]);
            var fromEmail = emailConfig["FromEmail"];
            var fromPassword = emailConfig["FromPassword"];

            // メール送信クラスの初期化
            _emailSender = new EmailSender(smtpHost, smtpPort, fromEmail, fromPassword);
        }

        // メールアドレスを元にパスワードをリセットし、メールで通知する
        public string ResetPasswordByEmail(string email)
        {
            // ユーザー情報を取得
            var user = _userDAC.GetUserByEmail(email);
            if (user == null)
                return "該当するユーザーが見つかりませんでした。";

            // ランダムな仮パスワードを生成
            var plainPassword = GenerateRandomPassword(8);

            // パスワードを更新（※必要なら暗号化も可能）
            _userDAC.UpdateUserPassword(user.UserId, plainPassword);

            try
            {
                // メール送信内容の作成
                var subject = "【提案システム】パスワード再設定";
                var body = $"ユーザー {user.UserName} 様\n\n新しいパスワード: {plainPassword}\n\nログイン後、すぐにパスワードを変更してください。";

                // メール送信
                _emailSender.Send(user.UserEmail, subject, body);
                return $"新しいパスワードを {user.UserEmail} に送信しました。";
            }
            catch (Exception ex)
            {
                return "メールの送信に失敗しました：" + ex.Message;
            }
        }

        // パスワード変更処理（本人が設定）
        public bool ChangeUserPassword(string userId, string newPassword, string confirmPassword, out string error)
        {
            error = string.Empty;

            // 入力チェック
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

            // パスワードの更新（※暗号化処理は必要に応じて）
            // string hashedPassword = HashPasswordSHA256(newPassword);
            // _userDAC.UpdatePasswordAndActivate(userId, hashedPassword);
            _userDAC.UpdatePasswordAndActivate(userId, newPassword);

            return true;
        }

        // ランダムなパスワードを生成
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

        // SHA256によるパスワードハッシュ化処理
        public string HashPasswordSHA256(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
