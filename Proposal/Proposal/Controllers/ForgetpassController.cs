using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proposal.BL;
using Proposal.DAL;
using Proposal.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;


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
            _connectionString = _configuration.GetConnectionString("ProposalDB");
            _forgetPassBL = new ForgetPassBL(_connectionString);
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
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Login");
            }

            var success = _forgetPassBL.ChangeUserPassword(userId, newPassword, confirmPassword, out string error);

            if (!success)
            {
                ViewBag.Error = error;
                return View();
            }

            // 设置标志 Session
            HttpContext.Session.SetString("SetPass", "1");
            return RedirectToAction("Login", "Login");
        }
    }
}
