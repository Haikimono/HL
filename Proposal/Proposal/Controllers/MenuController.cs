using Microsoft.AspNetCore.Mvc;

namespace Proposal.Controllers
{
    public class MenuController : Controller
    {
        [HttpGet]
        [Route("proposal/logout")]
        public IActionResult Logout()
        {
            // 清除session
            HttpContext.Session.Clear();
            return View("~/Views/Login/Login.cshtml");
        }
    }
}
