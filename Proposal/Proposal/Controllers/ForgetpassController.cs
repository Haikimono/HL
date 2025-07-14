using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proposal.BL;
using Proposal.Models;
using System.Data.SqlClient;

namespace Proposal.Controllers
{
    public class ForgetPassController : Controller
    {
        private readonly ILogger<ForgetPassController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ForgetPassBL _forgetPassBL;
        private readonly string _connectionString;

        public ForgetPassController(ILogger<ForgetPassController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var connStr = _configuration.GetConnectionString("ProposalDB");
            _forgetPassBL = new ForgetPassBL(connStr);
        }

        [HttpGet]
        public IActionResult ForgetPass()
        {
            return View(new ForgetPassModel());
        }

        [HttpPost]
        public IActionResult SendResetLink(ForgetPassModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Message = "メールアドレスを入力してください。";
                return View("ForgetPass", model);
            }

            var resultMessage = _forgetPassBL.ResetPasswordByEmail(model.Email);
            ViewBag.Message = resultMessage;
            return View("ForgetPass", model);
        }


        [HttpGet]
        [Route("/Login/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        public IActionResult ChangePassword(string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            // 验证输入
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Error = "すべての項目を入力してください。";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "パスワードが一致しません。";
                return View();
            }

            // 更新密码并清除 reset_pass 标志
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE [user] SET password = @Password, registration_status = 0 WHERE user_id = @UserId", conn);
                cmd.Parameters.AddWithValue("@Password", newPassword);  // 可替换为加密处理
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
            }

            TempData["Message"] = "パスワードが変更されました。ログインしてください。";
            return RedirectToAction("Login");
        }
    }
}
