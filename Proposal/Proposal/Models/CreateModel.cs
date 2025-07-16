using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Proposal.Models;

namespace Proposal.Models
{
    // 提案の区分のenum
    public enum TeianKbn
    {
        [Description("選択してください")]
        Select = 0,
        [Description("個人提案")]
        Kojin = 1,
        [Description("グループ提案")]
        Group = 2
    }

    // 所属のenum
    public enum Shozoku
    {
        [Description("選択してください")]
        Select = 0,
        [Description("国税庁")]
        KokuzaiCho = 1,
        [Description("国税局")]
        KokuzaiKyoku = 2,
        [Description("税務署・センター")]
        ZeimuShoCenter = 3,
        [Description("税大")]
        ZeiDai = 4,
        [Description("審判所")]
        ShinpanSho = 5
    }

    // 主務課のenum
    public enum ShumuKa
    {
        [Description("選択してください")]
        Select = 0,
        [Description("主務課")]
        ShumuKa = 1
    }

    // 関係課のenum
    public enum KankeiKa
    {
        [Description("選択してください")]
        Select = 0,
        [Description("関係課")]
        KankeiKa = 1
    }

    // 効果（実施）のenum
    public enum KoukaJishi
    {
        [Description("選択してください")]
        Select = 0,
        [Description("実施済")]
        JisshiZumi = 1,
        [Description("未実施（予想効果）")]
        MiJisshiYosoKouka = 2
    }

    // Enum扩展方法
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? value.ToString();
        }
    }

    // 提案種類
    public class ProposalType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    // 提案区分

    //　提案所属

    public class CreateModel : IValidatableObject
    {
        //ユーザーID
        public string? UserId { get; set; }

        //提案書番号
        public string? Id { get; set; }

        //提案状態
        public int? Status { get; set; }
        
        // 提案年度
        public string? TeianYear { get; set; }

        // 提案題名
        [Required(ErrorMessage = "提案題名を入力してください。")]
        [MaxLength(20, ErrorMessage = "20文字以内で入力してください。")]
        public string? TeianDaimei { get; set; }

        // 提案の種類ID
        [Required(ErrorMessage = "提案の種類を選択してください。")]
        public int? ProposalTypeId { get; set; }

        // 提案の区分
        [Required(ErrorMessage = "提案の区分を選択してください。")]
        public TeianKbn? TeianKbn { get; set; }

        public static SelectList TeianKbnOptions => new SelectList(
            new[] { new { Value = "", Text = "選択してください" } }
            .Concat(Enum.GetValues(typeof(TeianKbn))
                .Cast<TeianKbn>()
                .Where(e => e != Models.TeianKbn.Select)
                .Select(e => new { Value = ((int)e).ToString(), Text = e.GetDescription() })),
                "Value", "Text");

        // 所属
        [Required(ErrorMessage = "所属を選択してください。")]
        public Shozoku? Shozoku { get; set; }

        public static SelectList ShozokuOptions => new SelectList(
            new[] { new { Value = "", Text = "選択してください" } }
            .Concat(Enum.GetValues(typeof(Shozoku))
                .Cast<Shozoku>()
                .Where(e => e != Models.Shozoku.Select)
                .Select(e => new { Value = ((int)e).ToString(), Text = e.GetDescription() })),
                "Value", "Text");

        // 部・署
        public string? BuSho { get; set; }

        // 課・部門
        public string? KaBumon { get; set; }

        // 係・担当
        public string? KakariTantou { get; set; }

        // 氏名又は代表名
        [Required(ErrorMessage = "氏名又は代表名を入力してください。")]
        [MaxLength(6, ErrorMessage = "6文字以内で入力してください。")]
        public string? ShimeiOrDaihyoumei { get; set; }

        // グループ名
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupMei { get; set; }

        // グループの全員①所属
        public Shozoku? GroupZenin1 { get; set; }

        // グループの全員①氏名
        [MaxLength(6, ErrorMessage = "6文字以内で入力してください。")]
        public string? GroupZenin1Name { get; set; }
        // グループの全員①部・署
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin1BuSho { get; set; }
        // グループの全員①課・部門
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin1KaBumon { get; set; }
        // グループの全員①係・担当
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin1KakariTantou { get; set; }

        // グループの全員②
        public Shozoku? GroupZenin2 { get; set; }
        // グループの全員②氏名
        [MaxLength(6, ErrorMessage = "6文字以内で入力してください。")]
        public string? GroupZenin2Name { get; set; }
        // グループの全員②部・署
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin2BuSho { get; set; }
        // グループの全員②課・部門
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin2KaBumon { get; set; }
        // グループの全員②係・担当
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin2KakariTantou { get; set; }

        // グループの全員③
        public Shozoku? GroupZenin3 { get; set; }
        // グループの全員③氏名
        [MaxLength(6, ErrorMessage = "6文字以内で入力してください。")]
        public string? GroupZenin3Name { get; set; }
        // グループの全員③部・署
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin3BuSho { get; set; }
        // グループの全員③課・部門
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin3KaBumon { get; set; }
        // グループの全員③係・担当
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupZenin3KakariTantou { get; set; }

        // 第一次審査者を経ずに提出する
        public bool DaiijishinsashaHezuIsChecked { get; set; }

        // 第一次審査者所属
        public Shozoku? DaiijishinsashaShozoku { get; set; }

        // 第一次審査者部・署
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? DaiijishinsashaBuSho { get; set; }

        // 第一次審査者課・部門
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? DaiijishinsashaKaBumon { get; set; }

        // 第一次審査者氏名
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? DaiijishinsashaShimei { get; set; }

        // 第一次審査者官職
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? DaiijishinsashaKanshokun { get; set; }

        // 主務課
        [Required(ErrorMessage = "主務課を選択してください。")]
        public ShumuKa? ShumuKa { get; set; }

        public static SelectList ShumuKaOptions => new SelectList(
            new[] { new { Value = "", Text = "選択してください" } }
            .Concat(Enum.GetValues(typeof(ShumuKa))
                .Cast<ShumuKa>()
                .Where(e => e != Models.ShumuKa.Select)
                .Select(e => new { Value = ((int)e).ToString(), Text = e.GetDescription() })),
                "Value", "Text");

        // 関係課
        [Required(ErrorMessage = "関係課を選択してください。")]
        public KankeiKa? KankeiKa { get; set; }

        public static SelectList KankeiKaOptions => new SelectList(
            new[] { new { Value = "", Text = "選択してください" } }
            .Concat(Enum.GetValues(typeof(KankeiKa))
                .Cast<KankeiKa>()
                .Where(e => e != Models.KankeiKa.Select)
                .Select(e => new { Value = ((int)e).ToString(), Text = e.GetDescription() })),
                "Value", "Text");

        // 現状・問題点
        [Required(ErrorMessage = "現状・問題点を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string? GenjyoMondaiten { get; set; }

        // 改善案
        [Required(ErrorMessage = "改善案を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string? Kaizenan { get; set; }

        // 効果（実施）
        [Required(ErrorMessage = "実施を選択してください。")]
        public KoukaJishi? KoukaJishi { get; set; }

        public static SelectList KoukaJishiOptions => new SelectList(
            new[] { new { Value = "", Text = "選択してください" } }
            .Concat(Enum.GetValues(typeof(KoukaJishi))
                .Cast<KoukaJishi>()
                .Where(e => e != Models.KoukaJishi.Select)
                .Select(e => new { Value = ((int)e).ToString(), Text = e.GetDescription() })),
                "Value", "Text");

        // 効果
        [Required(ErrorMessage = "効果を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string? Kouka { get; set; }

        // 添付書類
        public IFormFile? TenpuFile1 { get; set; }
        public IFormFile? TenpuFile2 { get; set; }
        public IFormFile? TenpuFile3 { get; set; }
        public IFormFile? TenpuFile4 { get; set; }
        public IFormFile? TenpuFile5 { get; set; }

        // 添付ファイル名
        public string? TenpuFileName1 { get; set; }
        public string? TenpuFileName2 { get; set; }
        public string? TenpuFileName3 { get; set; }
        public string? TenpuFileName4 { get; set; }
        public string? TenpuFileName5 { get; set; }

        // 添付ファイルパス
        public string? TenpuFilePath1 { get; set; }
        public string? TenpuFilePath2 { get; set; }
        public string? TenpuFilePath3 { get; set; }
        public string? TenpuFilePath4 { get; set; }
        public string? TenpuFilePath5 { get; set; }

        //作成日
        public string Createddate { get; set; }

        //提出日
        public string Submissiondate { get; set; }

        //  CSV出力用にenumの説明を取得する方法
        public static string GetEnumDescriptionForCsv(Enum? value)
        {
            return value?.GetDescription() ?? "";
        }

        // ProposalTypeIdから名称を取得
        public static string GetProposalTypeName(int? id, List<SelectListItem> proposalTypes)
        {
            if (id == null) return "";
            var item = proposalTypes?.FirstOrDefault(x => x.Value == id.ToString());
            return item?.Text ?? "";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //提案区分はグループの場合、
            if (TeianKbn.HasValue && TeianKbn.Value == Models.TeianKbn.Group)
            {
                //グループ名は必須項目
                if (String.IsNullOrEmpty(GroupMei))
                {                    
                    yield return new ValidationResult("提案区分はグループの場合、グループ名を入力してください。", new[] { nameof(GroupMei) });
                }

                //グループの全員①は必須項目
                if (GroupZenin1 == Proposal.Models.Shozoku.Select || 
                    String.IsNullOrEmpty(GroupZenin1BuSho) || String.IsNullOrEmpty(GroupZenin1KaBumon) || String.IsNullOrEmpty(GroupZenin1KakariTantou))
                {
                    yield return new ValidationResult("グループメンバー①の情報を入力してください。", new[] { nameof(GroupZenin1) });
                }

                //グループの全員②の所属はNULLではないの場合
                if ((GroupZenin2.HasValue && GroupZenin2.Value != Proposal.Models.Shozoku.Select) &&
                    (String.IsNullOrEmpty(GroupZenin2BuSho) || String.IsNullOrEmpty(GroupZenin2KaBumon) || String.IsNullOrEmpty(GroupZenin2KakariTantou)))
                {
                    yield return new ValidationResult("グループメンバー②の情報を入力してください。", new[] { nameof(GroupZenin2) });
                }

                //グループの全員③の所属はNULLではないの場合
                if ((GroupZenin3.HasValue && GroupZenin3.Value != Proposal.Models.Shozoku.Select) &&
                    (String.IsNullOrEmpty(GroupZenin3BuSho) || String.IsNullOrEmpty(GroupZenin3KaBumon) || String.IsNullOrEmpty(GroupZenin3KakariTantou)))
                {
                    yield return new ValidationResult("グループメンバー③の情報を入力してください。", new[] { nameof(GroupZenin3) });
                }
            }

            //第一次審査者を経ずに提出する
            if (!DaiijishinsashaHezuIsChecked)
            {
                //第一次審査者所属は必須項目
                if (!DaiijishinsashaShozoku.HasValue || DaiijishinsashaShozoku.Value == Models.Shozoku.Select)
                {
                    yield return new ValidationResult("第一次審査者所属を選択してください。", new[] { nameof(DaiijishinsashaShozoku) });
                }
                //第一次審査者部・署は必須項目
                if (String.IsNullOrEmpty(DaiijishinsashaBuSho)) 
                {
                    yield return new ValidationResult("第一次審査者部・署を入力してください。", new[] { nameof(DaiijishinsashaBuSho) });
                }
                //第一次審査者課・部門は必須項目
                if (String.IsNullOrEmpty(DaiijishinsashaKaBumon))
                {
                    yield return new ValidationResult("第一次審査者課・部門を入力してください。", new[] { nameof(DaiijishinsashaKaBumon) });
                }
                //第一次審査者氏名は必須項目
                if (String.IsNullOrEmpty(DaiijishinsashaShimei))
                {
                    yield return new ValidationResult("第一次審査者氏名を入力してください。", new[] { nameof(DaiijishinsashaShimei) });
                }
                //第一次審査者官職は必須項目
                if (String.IsNullOrEmpty(DaiijishinsashaKanshokun))
                {
                    yield return new ValidationResult("第一次審査者官職を入力してください。", new[] { nameof(DaiijishinsashaKanshokun) });
                }
            }
        }
    }
}