using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Extensions.Configuration;
using Proposal.BL;
using Proposal.DAC;
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

                //所属、部・署、課・部門、係・担当取得
                if (!String.IsNullOrEmpty(model.UserId))
                {
                    //ユーザー情報取得
                    _createBL.GetUserInfoByUserId(model);
                }

                // GET：新規作成時は GroupMembers を3人に補完し、編集時は group_info から実際のメンバー数を取得する
                if (model.GroupMembers == null || model.GroupMembers.Count < 3)
                {
                    int count = Math.Max(model.GroupMembers?.Count ?? 0, 3);
                    while (model.GroupMembers.Count < count) model.GroupMembers.Add(new GroupMemberModel());
                }
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

                return RedirectToAction("ProposalList", "ProposalList");
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

                var dropdowns = (Proposal.BL.DropdownsViewModel)ViewBag.Dropdowns;
                string proposalTypeName = dropdowns.ProposalTypes.FirstOrDefault(x => x.Value == model.ProposalTypeId)?.Text ?? "";
                string firstReviewerAffiliation = dropdowns.Affiliations.FirstOrDefault(x => x.Value == model.FirstReviewerAffiliationId)?.Text ?? "";
                string firstReviewerDepartment = dropdowns.Departments.FirstOrDefault(x => x.Value == model.FirstReviewerDepartmentId)?.Text ?? "";
                string firstReviewerSection = dropdowns.Sections.FirstOrDefault(x => x.Value == model.FirstReviewerSectionId)?.Text ?? "";

                var csv = new StringBuilder();
                csv.AppendLine("提案年度,提案題名,提案の種類,提案の区分,氏名又は代表名,グループ名,第一次審査者所属,第一次審査者部・署,第一次審査者課・部門,第一次審査者氏名,第一次審査者官職,主務課,関係課,現状・問題点,改善案,効果の種類,効果");
                string koukaJishiText = model.KoukaJishi.HasValue ? (model.KoukaJishi.Value.GetType().GetField(model.KoukaJishi.Value.ToString()).GetCustomAttribute<System.ComponentModel.DescriptionAttribute>()?.Description ?? model.KoukaJishi.Value.ToString()) : "";
                csv.AppendLine($"\"{model.TeianYear}\",\"{model.TeianDaimei}\",\"{proposalTypeName}\",\"{model.ProposalKbnId}\",\"{model.ShimeiOrDaihyoumei}\",\"{model.GroupMei}\",\"{firstReviewerAffiliation}\",\"{firstReviewerDepartment}\",\"{firstReviewerSection}\",\"{model.FirstReviewerName}\",\"{model.FirstReviewerTitle}\",\"{model.EvaluationSectionId}\",\"{model.ResponsibleSectionId1}\",\"{model.GenjyoMondaiten}\",\"{model.Kaizenan}\",\"{koukaJishiText}\",\"{model.Kouka}\"");

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

                return RedirectToAction("ProposalList", "ProposalList");
            }

            // POST：バリデーションエラー時に GroupMembers を3人に補完し、保存時に BL.SaveGroupInfo を呼び出す
            if (!ModelState.IsValid) {
                int count = Math.Max(model.GroupMembers?.Count ?? 0, 3);
                while (model.GroupMembers.Count < count) model.GroupMembers.Add(new GroupMemberModel());
                return View(model);
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

                //グループデータを登録（区分がグループの時のみ）
                if (model.ProposalKbnId == "2")
                {
                    _createBL.InsertGroupInfo(model);
                }
            }
            else
            {
                //更新処理
                //提案書詳細更新
                _createBL.Updateproposals_detail(model);

                //グループデータを登録（区分がグループの時のみ）
                if (model.ProposalKbnId == "2")
                {

                    //グループデータを削除
                    _createBL.DeleteGroupInfo(model.Id);

                    //グループデータを登録
                    _createBL.InsertGroupInfo(model);
                }
            }
        }
    }
}
