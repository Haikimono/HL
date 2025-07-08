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

        //提案の種類
        private static readonly Dictionary<string, string> TeianShuruiMap = new()
        {
            { "0", "選択してください" },
            { "1", "適正な事務の管理に関する提案" },
            { "2", "事務の効率化に関する提案（内部事務の改善）" },
            { "3", "事務の効率化に関する提案（外部事務の改善）" },
            { "4", "その他提案" }
        };

        //提案の区分
        private static readonly Dictionary<string, string> TeianKbnMap = new()
        {
            { "0", "選択してください" },
            { "1", "個人提案" },
            { "2", "グループ提案" }
        };

        //所属
        private static readonly Dictionary<string, string> ShozokuMap = new()
        {
            { "0", "選択してください" },
            { "1", "国税庁" },
            { "2", "国税局" },
            { "3", "税務署・センター" },
            { "4", "税大" },
            { "5", "審判所" }
        };

        // 第一次審査者を経ずに提出する
        private static readonly Dictionary<string, string> DaiijishinsashaHezuIsCheckedMap = new()
        {
            { "True", "第一次審査者を経ず" },
            { "False", "第一次審査者を経へ" },
        };

        //主務課
        private static readonly Dictionary<string, string> ShumuKaMap = new()
        {
            { "0", "選択してください" },
            { "1", "主務課" }
        };

        //関係課
        private static readonly Dictionary<string, string> KankeiKaMap = new()
        {
            { "0", "選択してください" },
            { "1", "関係課" }
        };

        //実施
        private static readonly Dictionary<string, string> KoukaJishiMap = new()
        {
            { "0", "選択してください" },
            { "1", "実施済" },
            { "2", "未実施（予想効果）" }
        };

        [HttpGet]
        [Route("proposal/Create")]
        public IActionResult Create()
        {
            var model = new CreateModel
            {
                TeianYear = DateTime.Now.Year.ToString()
            };
            return View(model);
        }

        [HttpPost]
        [Route("proposal/Create")]
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
                //提案の種類
                string teianShurui = TeianShuruiMap.GetValueOrDefault(model.TeianShurui ?? "", model.TeianShurui ?? "");
                //提案の区分
                string teianKbn = TeianKbnMap.GetValueOrDefault(model.TeianKbn ?? "", model.TeianKbn ?? "");
                //所属
                string shozoku = ShozokuMap.GetValueOrDefault(model.Shozoku ?? "", model.Shozoku ?? "");
                //グループの全員①
                string groupZenin1 = ShozokuMap.GetValueOrDefault(model.GroupZenin1 ?? "", model.GroupZenin1 ?? "");
                //グループの全員②
                string groupZenin2 = ShozokuMap.GetValueOrDefault(model.GroupZenin2 ?? "", model.GroupZenin2 ?? "");
                //グループの全員③
                string groupZenin3 = ShozokuMap.GetValueOrDefault(model.GroupZenin3 ?? "", model.GroupZenin3 ?? "");
                //第一次審査者を経ずに提出する
                string daiijishinsashaHezuIsChecked = DaiijishinsashaHezuIsCheckedMap.GetValueOrDefault(model.DaiijishinsashaHezuIsChecked.ToString() ?? "", model.DaiijishinsashaHezuIsChecked.ToString() ?? "");
                //第一次審査者所属
                string daiijishinsashaShozoku = ShozokuMap.GetValueOrDefault(model.DaiijishinsashaShozoku ?? "", model.DaiijishinsashaShozoku ?? "");
                //主務課
                string shumuKaMap = ShumuKaMap.GetValueOrDefault(model.GroupZenin2 ?? "", model.GroupZenin2 ?? "");
                //関係課
                string kankeiKaMap = KankeiKaMap.GetValueOrDefault(model.GroupZenin3 ?? "", model.GroupZenin3 ?? "");
                //実施
                string koukaJishi = KoukaJishiMap.GetValueOrDefault(model.KoukaJishi ?? "", model.KoukaJishi ?? "");

                var csv = new StringBuilder();
                csv.AppendLine("提案年度,提案題名,提案の種類," +
                                "提案の区分,所属," +
                                "提案者部・署,提案者課・部門,提案者係・担当," +
                                "氏名又は代表名,グループ名," +
                                "グループの全員①,グループの全員①部・署,グループの全員①課・部門,グループの全員①係・担当," +
                                "グループの全員②,グループの全員②部・署,グループの全員②課・部門,グループの全員②係・担当," +
                                "グループの全員③,グループの全員③部・署,グループの全員③課・部門,グループの全員③係・担当," +
                                "第一次審査者を経ずに提出する,第一次審査者所属," +
                                "第一次審査者部・署,第一次審査者課・部門,第一次審査者氏名,第一次審査者官職" +
                                "現状・問題点,改善案,効果（実施）,効果");

                csv.AppendLine($"\"{model.TeianYear}\",\"{model.TeianDaimei}\",\"{teianShurui}\"," +
                                $"\"{teianKbn}\",\"{shozoku}\"," +
                                $"\"{model.BuSho}\",\"{model.KaBumon}\",\"{model.KakariTantou}\"，" +
                                $"\"{model.ShimeiOrDaihyoumei}\",\"{model.GroupMei}\"," +
                                $"\"{model.GroupZenin1}\",\"{model.GroupZenin1BuSho}\",\"{model.GroupZenin1KaBumon}\",\"{model.GroupZenin1KakariTantou}\"," +
                                $"\"{model.GroupZenin2}\",\"{model.GroupZenin2BuSho}\",\"{model.GroupZenin2KaBumon}\",\"{model.GroupZenin2KakariTantou}\"," +
                                $"\"{model.GroupZenin3}\",\"{model.GroupZenin3BuSho}\",\"{model.GroupZenin3KaBumon}\",\"{model.GroupZenin3KakariTantou}\"," +
                                $"\"{daiijishinsashaHezuIsChecked}\",\"{daiijishinsashaShozoku}\"," +
                                $"\"{model.DaiijishinsashaBuSho}\",\"{model.DaiijishinsashaKaBumon}\",\"{model.DaiijishinsashaShimei}\",\"{model.DaiijishinsashaKanshokun}\"," +
                                $"\"{shumuKaMap}\",\"{kankeiKaMap}\"," +
                                $"\"{model.GenjyoMondaiten}\",\"{model.Kaizenan}\",\"{koukaJishi}\",\"{model.Kouka}\"");

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
