using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proposal.BL;
using Proposal.Models;

namespace Proposal.Controllers
{
    public class ForgetPassController : Controller
    {
        private readonly ILogger<ForgetPassController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ForgetPassBL _forgetPassBL;

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
    }
}
