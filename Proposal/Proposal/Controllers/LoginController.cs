using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Proposal.BL;
using Proposal.Models;
using System.Diagnostics;

namespace Proposal.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private readonly LoginBL _loginBL;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var connStr = _configuration.GetConnectionString("ProposalDB");
            _loginBL = new LoginBL(connStr);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Sessionで登録したことを確認
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Menu");
            }
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Login(LoginModel model, string action)
        {
            if (action == "login")
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "入力内容に誤りがあります。";
                    return View(model);
                }

                var user = _loginBL.ValidateUser(model);
                if (user == null)
                {
                    ViewBag.Error = "ユーザーIDまたはパスワードが間違っています。";
                    return View(model);
                }

                // 登录成功，设置 session
                HttpContext.Session.SetString("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserKbn", user.UserKbn);

                return RedirectToAction("Menu");
            }
            ViewBag.Error = "不明なアクションです。";
            return View(model);
        }

        [HttpGet]
        [Route("proposal/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("proposal/register")]
        public IActionResult Register(string user_id, string user_name, string password, string user_kbn)
        {
            if (string.IsNullOrEmpty(user_id) || user_id.Length < 4 || user_id.Length > 20 || string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 32 || string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(user_kbn))
            {
                ViewBag.Error = "入力内容を確認してください。";
                return View();
            }
            string connStr = _configuration.GetConnectionString("ProposalDB");
            using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
            {
                conn.Open();
                string checkSql = "SELECT COUNT(*) FROM M_User WHERE user_id=@id";
                using (var checkCmd = new System.Data.SqlClient.SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", user_id);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        ViewBag.Error = "このユーザーIDは既に存在します。";
                        return View();
                    }
                }
                string sql = "INSERT INTO M_User(user_id, user_name, password, user_kbn, login_time) VALUES(@id, @name, @pw, @kbn, NULL)";
                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", user_id);
                    cmd.Parameters.AddWithValue("@name", user_name);
                    cmd.Parameters.AddWithValue("@pw", password);
                    cmd.Parameters.AddWithValue("@kbn", user_kbn);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        ViewBag.Success = "ユーザー登録が完了しました。ログインしてください。";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "登録に失敗しました。";
                        return View();
                    }
                }
            }
        }

        [Route("proposal/menu")]
        public IActionResult Menu()
        {
            // 检查是否已登录
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                // 未登录，跳转到登录页面
                return RedirectToAction("Login");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserKbn = HttpContext.Session.GetString("UserKbn");
            return View("~/Views/Menu/Menu.cshtml");
        }
    }
}
