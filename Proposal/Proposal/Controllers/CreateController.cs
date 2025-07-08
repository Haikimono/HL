using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Proposal.BL;
using Proposal.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;


namespace Proposal.Controllers
{
    public class CreateController : Controller
    {
        private readonly CreateBL _CreateBL;

        private static readonly Dictionary<string, string> TeianShuruiMap = new()
        {
            { "0", "選択してください" },
            { "1", "適正な事務の管理に関する提案" },
            { "2", "事務の効率化に関する提案（内部事務の改善）" },
            { "3", "事務の効率化に関する提案（外部事務の改善）" },
            { "4", "その他提案" }
        };

        private static readonly Dictionary<string, string> TeianKbnMap = new()
        {
            { "0", "選択してください" },
            { "1", "個人提案" },
            { "2", "グループ提案" }
        };

        private static readonly Dictionary<string, string> ShozokuMap = new()
        {
            { "0", "選択してください" },
            { "1", "国税庁" },
            { "2", "国税局" },
            { "3", "税務署・センター" },
            { "4", "税大" },
            { "5", "審判所" }
        };

        private static readonly Dictionary<string, string> KoukaJishiMap = new()
        {
            { "0", "選択してください" },
            { "1", "実施済" },
            { "2", "未実施（予想効果）" }
        };

        private static readonly Dictionary<string, string> ShumuKaMap = new()
        {
            { "0", "選択してください" },
            { "1", "主務課" }
        };

        private static readonly Dictionary<string, string> KankeiKaMap = new()
        {
            { "0", "選択してください" },
            { "1", "関係課" }
        };


        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateModel());
        }

        [HttpPost]
        public IActionResult Create(CreateModel model, string action)
        {
            //戻る
            if (action == "Menu")
            {
                return View("~/Views/Menu/Menu.cshtml");
            }

            //一時保存
            if (action == "Ichijihozon")
            {
                return View("~/Views/Menu/Menu.cshtml");
            }

            //出力
            if (action == "Deryoku")
            {
                //提案
                string teianShurui = TeianShuruiMap.GetValueOrDefault(model.TeianShurui ?? "", model.TeianShurui ?? "");
                string teianKbn = TeianKbnMap.GetValueOrDefault(model.TeianKbn ?? "", model.TeianKbn ?? "");
                string shozoku = ShozokuMap.GetValueOrDefault(model.Shozoku ?? "", model.Shozoku ?? "");
                string koukaJishi = KoukaJishiMap.GetValueOrDefault(model.KoukaJishi ?? "", model.KoukaJishi ?? "");

                var csv = new StringBuilder();
                csv.AppendLine("提案題名,提案の種類,提案の区分,所属,氏名,グループ名,現状・問題点,改善案,効果（実施）,効果");

                csv.AppendLine($"\"{model.TeianDaimei}\",\"{teianShurui}\",\"{teianKbn}\",\"{shozoku}\",\"{model.ShimeiOrDaihyoumei}\",\"{model.GroupMei}\",\"{model.GenjyoMondaiten}\",\"{model.Kaizenan}\",\"{koukaJishi}\",\"{model.Kouka}\"");

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var filename = $"提案内容_{DateTime.Now:yyyyMMddHHmmss}.csv";
                return File(bytes, "text/csv", filename);
            }

            //提出
            if (action == "Submit")
            {
                return View("~/Views/Menu/Menu.cshtml");
            }

            return View(model);
        }


    }
}
