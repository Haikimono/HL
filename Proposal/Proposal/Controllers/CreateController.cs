using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
                _createBL.GetProposalDetailById(model);
            }

            //新規又は一時保存の場合
            if(model.Status == null || model.Status == 1)
            {
                //年度取得
                model.TeianYear = DateTime.Now.ToString("ggy年", new CultureInfo("ja-JP") { DateTimeFormat = { Calendar = new JapaneseCalendar() } });
            }

            return View(model);
        }

        //基本情報のチェックと提案内容のチェック
        private bool ValidateAndSetTab(CreateModel model)
        {
            //基本情報のチェック
            if (!ValidateBasicInfo(model))
            {
                ViewBag.ShowProposalContent = false;
                return false;
            }
            //提案内容のチェック
            if (!ValidateProposalContent(model))
            {
                ViewBag.ShowProposalContent = true;
                return false;
            }
            return true;
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
                if (!ValidateAndSetTab(model))
                {
                    return View(model);
                }
                //提案状態を作成中に設定
                model.Status = 1;
                this.InsertOrUpdate(model);
                return RedirectToAction("List", "Proposal");
            }

            //出力
            if (action == "Deryoku")
            {
                if (!ValidateAndSetTab(model))
                {
                    return View(model);
                }
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
                if (!ValidateAndSetTab(model))
                {
                    return View(model);
                }
                //提案状態を審査中に設定
                model.Status = 11;
                this.InsertOrUpdate(model);
                return RedirectToAction("List", "Proposal");
            }

            return View(model);
        }
       
        // 基本情報のバリデーション実行
        private bool ValidateBasicInfo(CreateModel model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            
            // 基本情報のプロパティのみを検証
            var basicInfoProperties = new[]
            {
                nameof(model.TeianDaimei),
                nameof(model.TeianShurui),
                nameof(model.TeianKbn),
                nameof(model.Shozoku),
                nameof(model.ShimeiOrDaihyoumei),
                nameof(model.GroupMei),
                nameof(model.GroupZenin1),
                nameof(model.GroupZenin1BuSho),
                nameof(model.GroupZenin1KaBumon),
                nameof(model.GroupZenin1KakariTantou),
                nameof(model.GroupZenin2),
                nameof(model.GroupZenin2BuSho),
                nameof(model.GroupZenin2KaBumon),
                nameof(model.GroupZenin2KakariTantou),
                nameof(model.GroupZenin3),
                nameof(model.GroupZenin3BuSho),
                nameof(model.GroupZenin3KaBumon),
                nameof(model.GroupZenin3KakariTantou),
                nameof(model.DaiijishinsashaHezuIsChecked),
                nameof(model.DaiijishinsashaShozoku),
                nameof(model.DaiijishinsashaBuSho),
                nameof(model.DaiijishinsashaKaBumon),
                nameof(model.DaiijishinsashaShimei),
                nameof(model.DaiijishinsashaKanshokun),
                nameof(model.ShumuKa),
                nameof(model.KankeiKa)
            };

            // 基本情報のプロパティのみを検証
            foreach (var propertyName in basicInfoProperties)
            {
                var property = typeof(CreateModel).GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.GetValue(model);
                    var propertyContext = new ValidationContext(model, null, null) { MemberName = propertyName };
                    
                    // プロパティの属性を検証
                    var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
                    foreach (ValidationAttribute attribute in attributes)
                    {
                        var result = attribute.GetValidationResult(value, propertyContext);
                        if (result != null)
                        {
                            validationResults.Add(result);
                        }
                    }
                }
            }

            // 基本情報のカスタムバリデーション
            var basicInfoValidationResults = ValidateBasicInfoCustom(model);
            validationResults.AddRange(basicInfoValidationResults);

            // 基本情報のバリデーション結果をModelStateに追加
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }

            return validationResults.Count == 0;
        }

        // 提案内容のバリデーション実行
        private bool ValidateProposalContent(CreateModel model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            
            // 提案内容のプロパティのみを検証
            var proposalContentProperties = new[]
            {
                nameof(model.GenjyoMondaiten),
                nameof(model.Kaizenan),
                nameof(model.KoukaJishi),
                nameof(model.Kouka)
            };

            // 提案内容のプロパティのみを検証
            foreach (var propertyName in proposalContentProperties)
            {
                var property = typeof(CreateModel).GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.GetValue(model);
                    var propertyContext = new ValidationContext(model, null, null) { MemberName = propertyName };
                    
                    // プロパティの属性を検証
                    var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
                    foreach (ValidationAttribute attribute in attributes)
                    {
                        var result = attribute.GetValidationResult(value, propertyContext);
                        if (result != null)
                        {
                            validationResults.Add(result);
                        }
                    }
                }
            }

            // 提案内容のバリデーション結果をModelStateに追加
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }

            return validationResults.Count == 0;
        }

        // 基本情報のカスタムバリデーション
        private IEnumerable<ValidationResult> ValidateBasicInfoCustom(CreateModel model)
        {
            var results = new List<ValidationResult>();

            //提案区分はグループの場合、
            if (model.TeianKbn.HasValue && model.TeianKbn.Value == Models.TeianKbn.Group)
            {
                //グループ名は必須項目
                if (String.IsNullOrEmpty(model.GroupMei))
                {                    
                    results.Add(new ValidationResult("提案区分はグループの場合、グループ名を入力してください。", new[] { nameof(model.GroupMei) }));
                }

                //グループの全員①は必須項目
                if (model.GroupZenin1 == Proposal.Models.Shozoku.Select || 
                    String.IsNullOrEmpty(model.GroupZenin1BuSho) || String.IsNullOrEmpty(model.GroupZenin1KaBumon) || String.IsNullOrEmpty(model.GroupZenin1KakariTantou))
                {
                    results.Add(new ValidationResult("グループメンバー①の情報を入力してください。", new[] { nameof(model.GroupZenin1) }));
                }

                //グループの全員②の所属はNULLではないの場合
                if ((model.GroupZenin2.HasValue && model.GroupZenin2.Value != Proposal.Models.Shozoku.Select) &&
                    (String.IsNullOrEmpty(model.GroupZenin2BuSho) || String.IsNullOrEmpty(model.GroupZenin2KaBumon) || String.IsNullOrEmpty(model.GroupZenin2KakariTantou)))
                {
                    results.Add(new ValidationResult("グループメンバー②の情報を入力してください。", new[] { nameof(model.GroupZenin2) }));
                }

                //グループの全員③の所属はNULLではないの場合
                if ((model.GroupZenin3.HasValue && model.GroupZenin3.Value != Proposal.Models.Shozoku.Select) &&
                    (String.IsNullOrEmpty(model.GroupZenin3BuSho) || String.IsNullOrEmpty(model.GroupZenin3KaBumon) || String.IsNullOrEmpty(model.GroupZenin3KakariTantou)))
                {
                    results.Add(new ValidationResult("グループメンバー③の情報を入力してください。", new[] { nameof(model.GroupZenin3) }));
                }
            }

            //第一次審査者を経ずに提出する
            if (!model.DaiijishinsashaHezuIsChecked)
            {
                //第一次審査者所属は必須項目
                if (!model.DaiijishinsashaShozoku.HasValue || model.DaiijishinsashaShozoku.Value == Models.Shozoku.Select)
                {
                    results.Add(new ValidationResult("第一次審査者所属を選択してください。", new[] { nameof(model.DaiijishinsashaShozoku) }));
                }
                //第一次審査者部・署は必須項目
                if (String.IsNullOrEmpty(model.DaiijishinsashaBuSho)) 
                {
                    results.Add(new ValidationResult("第一次審査者部・署を入力してください。", new[] { nameof(model.DaiijishinsashaBuSho) }));
                }
                //第一次審査者課・部門は必須項目
                if (String.IsNullOrEmpty(model.DaiijishinsashaKaBumon))
                {
                    results.Add(new ValidationResult("第一次審査者課・部門を入力してください。", new[] { nameof(model.DaiijishinsashaKaBumon) }));
                }
                //第一次審査者氏名は必須項目
                if (String.IsNullOrEmpty(model.DaiijishinsashaShimei))
                {
                    results.Add(new ValidationResult("第一次審査者氏名を入力してください。", new[] { nameof(model.DaiijishinsashaShimei) }));
                }
                //第一次審査者官職は必須項目
                if (String.IsNullOrEmpty(model.DaiijishinsashaKanshokun))
                {
                    results.Add(new ValidationResult("第一次審査者官職を入力してください。", new[] { nameof(model.DaiijishinsashaKanshokun) }));
                }
            }

            return results;
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

                //提案書一覧登録
                _createBL.Insertproposals(model);
            }
            else
            {
                //更新処理
                //提案書詳細更新
                _createBL.Updateproposals_detail(model);

                //提案書一覧更新
                _createBL.Updateproposals(model);
            }
        }
    }
}
