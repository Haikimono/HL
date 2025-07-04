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


        [Route("proposal/list")]
        public IActionResult List()
        {
            // 返回列表视图
            return View("~/Views/Menu/List.cshtml");
        }


        [Route("proposal/create")]
        public IActionResult Create ()
        {
            // 返回列表视图
            return View("~/Views/Menu/create.cshtml");
        }
    }
}
