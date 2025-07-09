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
        private readonly CreateBL _createBL;



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
                //登録と更新処理判断

                //登録処理
                 _createBL.TeianshoTeishutsu(model);

            }

            //出力
            if (action == "Deryoku")
            {
                // 使用enum的Description属性来获取CSV值
                string teianShurui = CreateModel.GetEnumDescriptionForCsv(model.TeianShurui);
                string teianKbn = CreateModel.GetEnumDescriptionForCsv(model.TeianKbn);
                string shozoku = CreateModel.GetEnumDescriptionForCsv(model.Shozoku);
                string groupZenin1 = CreateModel.GetEnumDescriptionForCsv(model.GroupZenin1);
                string groupZenin2 = CreateModel.GetEnumDescriptionForCsv(model.GroupZenin2);
                string groupZenin3 = CreateModel.GetEnumDescriptionForCsv(model.GroupZenin3);
                string daiijishinsashaHezuIsChecked = model.DaiijishinsashaHezuIsChecked ? "はい" : "いいえ";
                string daiijishinsashaShozoku = CreateModel.GetEnumDescriptionForCsv(model.DaiijishinsashaShozoku);
                string shumuKaMap = CreateModel.GetEnumDescriptionForCsv(model.ShumuKa);
                string kankeiKaMap = CreateModel.GetEnumDescriptionForCsv(model.KankeiKa);
                string koukaJishi = CreateModel.GetEnumDescriptionForCsv(model.KoukaJishi);

                var csv = new StringBuilder();
                csv.AppendLine("提案年度,提案題名,提案の種類," +
                               "提案の区分,所属," +
                               "提案者部・署,提案者課・部門,提案者係・担当," +
                               "氏名又は代表名,グループ名," +
                               "グループの全員①,グループの全員①部・署,グループの全員①課・部門,グループの全員①係・担当," +
                               "グループの全員②,グループの全員②部・署,グループの全員②課・部門,グループの全員②係・担当," +
                               "グループの全員③,グループの全員③部・署,グループの全員③課・部門,グループの全員③係・担当," +
                               "第一次審査者を経ずに提出する,第一次審査者所属," +
                               "第一次審査者部・署,第一次審査者課・部門,第一次審査者氏名,第一次審査者官職," + 
                               "主務課,関係課," +
                               "現状・問題点,改善案,効果（実施）,効果");

                csv.AppendLine($"\"{model.TeianYear}\",\"{model.TeianDaimei}\",\"{teianShurui}\"," +
                               $"\"{teianKbn}\",\"{shozoku}\"," +
                               $"\"{model.BuSho}\",\"{model.KaBumon}\",\"{model.KakariTantou}\"," +
                               $"\"{model.ShimeiOrDaihyoumei}\",\"{model.GroupMei}\"," +
                               $"\"{groupZenin1}\",\"{model.GroupZenin1BuSho}\",\"{model.GroupZenin1KaBumon}\",\"{model.GroupZenin1KakariTantou}\"," +
                               $"\"{groupZenin2}\",\"{model.GroupZenin2BuSho}\",\"{model.GroupZenin2KaBumon}\",\"{model.GroupZenin2KakariTantou}\"," +
                               $"\"{groupZenin3}\",\"{model.GroupZenin3BuSho}\",\"{model.GroupZenin3KaBumon}\",\"{model.GroupZenin3KakariTantou}\"," +
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
