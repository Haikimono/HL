using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Configuration;
using Proposal.BL;
using Proposal.Models;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

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
            var userId = HttpContext.Session.GetString("UserId");
            var reSetPass = HttpContext.Session.GetString("ReSetPass");

            if (!string.IsNullOrEmpty(userId))
            {
                if (reSetPass == "1")
                {
                    return RedirectToAction("ChangePassword");
                }
                return RedirectToAction("Menu");
            }
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Login(LoginModel model, string action)
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
            HttpContext.Session.SetString("ReSetPass", user.ReSetPass ? "1" : "0");

            if (user.ReSetPass)
            {
                return RedirectToAction("ChangePassword");
            }

            return RedirectToAction("Menu");
            
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

            ViewBag.UserKbn = HttpContext.Session.GetString("UserKbn");
            return View("~/Views/ForgetPass/ChangePassword.cshtml"); 
        }
    }
}
