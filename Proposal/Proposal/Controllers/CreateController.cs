using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Extensions.Configuration;
using Proposal.BL;
using Proposal.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;


namespace Proposal.Controllers
{
    public class CreateController : Controller
    {
        private readonly CreateBL _createBL = new CreateBL();
        private readonly IConfiguration _configuration;

        public CreateController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            var connStr = _configuration.GetConnectionString("ProposalDB");
            _createBL = new CreateBL(connStr);
        }

        [HttpGet]
        [Route("proposal/Create")]
        public IActionResult Create()
        {
            // すべてのドロップダウンリストをBL層の共通メソッドで取得
            ViewBag.Dropdowns = _createBL.GetDropdowns();

            var model = new CreateModel
            {
                //提案書ID
                Id = HttpContext.Session.GetString("Id"),

                //ユーザーID取得
                UserId = HttpContext.Session.GetString("UserId")
            };

            //修正・確認の場合
            if (!String.IsNullOrEmpty(model.Id))
            {
                //提案書情報取得
                _createBL.GetProposalDetailById(model);
            }

            //新規又は一時保存の場合
            if(model.Status == null || model.Status == 1)
            {
                //年度取得
                model.TeianYear = DateTime.Now.ToString("ggy年", new CultureInfo("ja-JP") { DateTimeFormat = { Calendar = new JapaneseCalendar() } });
            }

            //部・署、課・部門、係・担当取得
            if (!String.IsNullOrEmpty(model.UserId))
            {
                //ユーザー情報取得
                _createBL.GetUserInfoByUserId(model);
            }

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
                if (!ValidateModel(model))
                {
                    SetDropdowns();
                    SetShowProposalContentFlagForModelStateError();

                    return View(model);
                }

                // ファイル保存処理（最大5つ）
                SaveUploadedFiles(model);

                //提案状態を作成中に設定
                model.Status = 1;

                //登録又は更新処理
                this.InsertOrUpdate(model);

                return RedirectToAction("ProposalList", "Proposal");
            }

            //出力
            if (action == "Deryoku")
            {
                if (!ValidateModel(model))
                {
                    SetDropdowns();
                    SetShowProposalContentFlagForModelStateError();

                    return View(model);
                }

                // ドロップダウンリストから提案種類名を取得
                var dropdowns = (Proposal.BL.DropdownsViewModel)ViewBag.Dropdowns;
                string teianShurui = CreateModel.GetProposalTypeName(model.ProposalTypeId, dropdowns.ProposalTypes);
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
                if (!ValidateModel(model))
                {
                    SetDropdowns();
                    SetShowProposalContentFlagForModelStateError();

                    return View(model);
                }

                // ファイル保存処理（最大5つ）
                SaveUploadedFiles(model);

                //提案状態を審査中に設定
                model.Status = 11;

                //登録又は更新処理
                this.InsertOrUpdate(model);

                return RedirectToAction("ProposalList", "Proposal");
            }

            return View(model);
        }

        // ドロップダウンリストをViewBagにセット
        private void SetDropdowns()
        {
            ViewBag.Dropdowns = _createBL.GetDropdowns();
        }

        // ファイル保存処理（最大5つ）
        private void SaveUploadedFiles(CreateModel model)
        {
            for (int i = 1; i <= 5; i++)
            {
                var fileProp = model.GetType().GetProperty($"TenpuFile{i}");
                var file = fileProp?.GetValue(model) as IFormFile;
                if (file != null && file.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Proposal/wwwroot/uploads");
                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                    var ext = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    // ファイル名をモデルにセット
                    var nameProp = model.GetType().GetProperty($"TenpuFileName{i}");
                    nameProp?.SetValue(model, fileName);
                }
            }
        }

        // モデルバリデーション実行
        private bool ValidateModel(CreateModel model)
        {
            // カスタムバリデーションを明示的に実行
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            
            // カスタムバリデーション結果をModelStateに追加
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }
            
            // モデルバリデーション実行
            return ModelState.IsValid;
        }

        /// <summary>
        /// ModelStateに提案内容タブのエラーがある場合、ViewBag.ShowProposalContentをtrueにする
        /// </summary>
        private void SetShowProposalContentFlagForModelStateError()
        {
            // 提案内容tab相关字段
            var teianNaiyouFields = new[] { "GenjyoMondaiten", "Kaizenan", "KoukaJishi", "Kouka", "TenpuFile1", "TenpuFile2", "TenpuFile3", "TenpuFile4", "TenpuFile5", "TenpuFileName1", "TenpuFileName2", "TenpuFileName3", "TenpuFileName4", "TenpuFileName5", "TenpuFilePath1", "TenpuFilePath2", "TenpuFilePath3", "TenpuFilePath4", "TenpuFilePath5" };
            ViewBag.ShowProposalContent = ModelState.Keys.Any(k => teianNaiyouFields.Any(f => k.Contains(f)) && ModelState[k].Errors.Count > 0);
        }

        //登録と更新処理
        public void InsertOrUpdate(CreateModel model)
        {
            //ユーザーID
            model.UserId = HttpContext.Session.GetString("UserId");

            //登録と更新処理判断
            if (string.IsNullOrEmpty(model.Id))
            {
                //登録処理
                //提案書詳細登録
                model.Id = _createBL.Insertproposals_detail(model).ToString();

                ////提案書一覧登録
                //_createBL.Insertproposals(model);
            }
            else
            {
                //更新処理
                //提案書詳細更新
                _createBL.Updateproposals_detail(model);

                ////提案書一覧更新
                //_createBL.Updateproposals(model);
            }
        }
    }
}
