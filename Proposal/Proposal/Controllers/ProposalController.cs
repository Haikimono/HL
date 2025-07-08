using Microsoft.AspNetCore.Mvc;
using Proposal.BL;
using Proposal.Models;
using System.Linq;

namespace Proposal.Controllers
{
    [Route("proposal")]
    public class ProposalController : Controller
    {
        private readonly string _connectionString;

        public ProposalController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProposalDB");
        }

        [HttpGet("list")]
        public IActionResult List(int? year, int? status)
        {
            var bl = new ProposalBL(_connectionString);
            var proposals = bl.GetProposalList();

            if (year.HasValue)
            {
                proposals = proposals.Where(p => p.ProposalYear == year.Value).ToList();
            }
            if (status.HasValue)
            {
                proposals = proposals.Where(p => p.Status == status.Value).ToList();
            }

            // 传递当前筛选条件到View
            ViewBag.SelectedYear = year;
            ViewBag.SelectedStatus = status;

            // 传递所有可选年度和状态到View
            ViewBag.Years = proposals.Select(p => p.ProposalYear).Distinct().OrderByDescending(y => y).ToList();
            ViewBag.Statuses = System.Enum.GetValues(typeof(ProposalStatus)).Cast<ProposalStatus>().ToList();

            return View("~/Views/List/List.cshtml", proposals);
        }
    }
}
