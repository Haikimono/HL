using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Proposal.Models;

namespace Proposal.Models
{

    // 所属
    public class Affiliation
    {
        public string Id { get; set; }
        public string Shozoku { get; set; }
    }
    // 部・署
    public class Department
    {
        public string Department_id { get; set; }
        public string Department_name { get; set; }
    }
    // 課・部門
    public class Section
    {
        public string Section_id { get; set; }
        public string Section_name { get; set; }
    }
    // 係・担当
    public class Subsection
    {
        public string Subsection_id { get; set; }
        public string Subsection_name { get; set; }
    }

    // 提案种类
    public class ProposalType
    {
        public string Kbn { get; set; }
        public string KbnName { get; set; }
    }

    public enum KoukaJishi
    {
        [Description("選択してください")] Select = 0,
        [Description("実施済")] JisshiZumi = 1,
        [Description("未実施（予想効果）")] MiJisshiYosoKouka = 2
    }

    public class CreateModel : IValidatableObject
    {
        // 主提案者 所属/部・署/課・部門/係・担当 id
        public string AffiliationId { get; set; } // 所属id
        public string DepartmentId { get; set; }  // 部・署id
        public string SectionId { get; set; }     // 課・部門id
        public string SubsectionId { get; set; }  // 係・担当id

        // 主提案者 所属/部・署/課・部門/係・担当 名称（只读显示用）
        public string AffiliationName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string SubsectionName { get; set; }

        // 提案种类
        [Required(ErrorMessage = "提案の種類を選択してください。")]
        public string ProposalTypeId { get; set; }
        // 提案区分
        [Required(ErrorMessage = "提案の区分を選択してください。")]
        public string ProposalKbnId { get; set; }

        // 用户ID
        public string UserId { get; set; }
        // 提案书编号
        public string Id { get; set; }
        // 提案状态
        public int? Status { get; set; }
        // 提案年度
        public string TeianYear { get; set; }
        // 提案题名
        [Required(ErrorMessage = "提案題名を入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string TeianDaimei { get; set; }
        // 氏名又は代表名
        [Required(ErrorMessage = "氏名又は代表名を入力してください。")]
        [MaxLength(20, ErrorMessage = "20文字以内で入力してください。")]
        public string ShimeiOrDaihyoumei { get; set; }
        // グループ名
        [MaxLength(20, ErrorMessage = "20文字以内で入力してください。")]
        public string GroupMei { get; set; }
        // Group成员1
        public string GroupZenin1AffiliationId { get; set; }
        public string GroupZenin1DepartmentId { get; set; }
        public string GroupZenin1SectionId { get; set; }
        public string GroupZenin1SubsectionId { get; set; }
        public string GroupZenin1Name { get; set; }
        // Group成员2
        public string GroupZenin2AffiliationId { get; set; }
        public string GroupZenin2DepartmentId { get; set; }
        public string GroupZenin2SectionId { get; set; }
        public string GroupZenin2SubsectionId { get; set; }
        public string GroupZenin2Name { get; set; }
        // Group成员3
        public string GroupZenin3AffiliationId { get; set; }
        public string GroupZenin3DepartmentId { get; set; }
        public string GroupZenin3SectionId { get; set; }
        public string GroupZenin3SubsectionId { get; set; }
        public string GroupZenin3Name { get; set; }
        // 第一次審査者
        public string FirstReviewerAffiliationId { get; set; }
        public string FirstReviewerDepartmentId { get; set; }
        public string FirstReviewerSectionId { get; set; }
        public string FirstReviewerSubsectionId { get; set; }
        public string FirstReviewerUserId { get; set; }
        public string FirstReviewerName { get; set; }
        public string FirstReviewerTitle { get; set; }
        // 主務課
        [Required(ErrorMessage = "主務課を選択してください。")]
        public string ShumuKaId { get; set; }
        // 関係課
        [Required(ErrorMessage = "関係課を選択してください。")]
        public string KankeiKaId { get; set; }
        // 現状・問題点
        [Required(ErrorMessage = "現状・問題点を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string GenjyoMondaiten { get; set; }
        // 改善案
        [Required(ErrorMessage = "改善案を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string Kaizenan { get; set; }
        // 効果（実施）
        [Required(ErrorMessage = "効果の種類を選択してください。")]
        public KoukaJishi? KoukaJishi { get; set; }
        // 効果
        [Required(ErrorMessage = "効果を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string Kouka { get; set; }
        // 添付書類
        public IFormFile TenpuFile1 { get; set; }
        public IFormFile TenpuFile2 { get; set; }
        public IFormFile TenpuFile3 { get; set; }
        public IFormFile TenpuFile4 { get; set; }
        public IFormFile TenpuFile5 { get; set; }
        // 添付ファイル名
        public string TenpuFileName1 { get; set; }
        public string TenpuFileName2 { get; set; }
        public string TenpuFileName3 { get; set; }
        public string TenpuFileName4 { get; set; }
        public string TenpuFileName5 { get; set; }
        // 添付ファイルパス
        public string TenpuFilePath1 { get; set; }
        public string TenpuFilePath2 { get; set; }
        public string TenpuFilePath3 { get; set; }
        public string TenpuFilePath4 { get; set; }
        public string TenpuFilePath5 { get; set; }
        //作成日
        public string Createddate { get; set; }
        //提出日
        public string Submissiondate { get; set; }
        // 校验逻辑
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 例：必填校验
            if (ProposalKbnId == "2") // 2=グループ
            {
                if (string.IsNullOrEmpty(GroupMei))
                {
                    yield return new ValidationResult("提案区分はグループの場合、グループ名を入力してください。", new[] { nameof(GroupMei) });
                }
                if (string.IsNullOrEmpty(GroupZenin1AffiliationId) || string.IsNullOrEmpty(GroupZenin1DepartmentId) || string.IsNullOrEmpty(GroupZenin1SectionId) || string.IsNullOrEmpty(GroupZenin1SubsectionId) || string.IsNullOrEmpty(GroupZenin1Name))
                {
                    yield return new ValidationResult("グループメンバー①の情報を入力してください。", new[] { nameof(GroupZenin1Name) });
                }
            }
        }
    }
}