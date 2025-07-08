using System.ComponentModel.DataAnnotations;

namespace Proposal.Models
{
    public class CreateModel
    {
        //提案題名
        [Required(ErrorMessage = "提案題名を入力してください。")]
        [MaxLength(20, ErrorMessage = "20文字以内で入力してください。")]
        public string? TeianDaimei { get; set; }

        //提案の種類
        [Required(ErrorMessage = "提案の種類を選択してください。")]
        public string? TeianShurui { get; set; }

        //提案の区分
        [Required(ErrorMessage = "提案の区分を選択してください。")]
        public string? TeianKbn { get; set; }

        //所属
        [Required(ErrorMessage = "所属を選択してください。")]
        public string? Shozoku { get; set; }

        //部・署
        public string? BuSho { get; set; }

        //課・部門
        public string? KaBumon { get; set; }

        //係・担当
        public string? KakariTantou { get; set; }

        //氏名又は代表名
        [Required(ErrorMessage = "氏名又は代表名を入力してください。")]
        [MaxLength(6, ErrorMessage = "6文字以内で入力してください。")]
        public string? ShimeiOrDaihyoumei { get; set; }

        //グループ名
        [MaxLength(10, ErrorMessage = "10文字以内で入力してください。")]
        public string? GroupMei { get; set; }

        //グループの全員①
        public string? GroupZenin1 { get; set; }
        //グループの全員部・署①
        public string? GroupZenin1BuSho { get; set; }
        //グループの全員課・部門①
        public string? GroupZenin1KaBumon { get; set; }
        //グループの全員係・担当①
        public string? GroupZenin1KakariTantou { get; set; }

        //グループの全員②
        public string? GroupZenin2 { get; set; }
        //グループの全員部・署②
        public string? GroupZenin2BuSho { get; set; }
        //グループの全員課・部門②
        public string? GroupZenin2KaBumon { get; set; }
        //グループの全員係・担当②
        public string? GroupZenin2KakariTantou { get; set; }

        //グループの全員③
        public string? GroupZenin3 { get; set; }
        //グループの全員部・署③
        public string? GroupZenin3BuSho { get; set; }
        //グループの全員課・部門③
        public string? GroupZenin3KaBumon { get; set; }
        //グループの全員係・担当③
        public string? GroupZenin3KakariTantou { get; set; }

        //第一次審査者を経ずに提出する
        public bool DaiijishinsashaHezuIsChecked { get; set; }

        //第一次審査者所属
        [Required(ErrorMessage = "所属を選択してください。")]
        public string? DaiijishinsashaShozoku { get; set; }

        //第一次審査者部・署
        public string? DaiijishinsashaBuSho { get; set; }

        //第一次審査者課・部門
        public string? DaiijishinsashaKaBumon { get; set; }

        //第一次審査者氏名
        public string? DaiijishinsashaShimei { get; set; }

        //第一次審査者官職
        public string? DaiijishinsashaKanshokun { get; set; }

        //主務課
        public string? ShumuKa { get; set; }

        //関係課
        public string? KankeiKa { get; set; }

        //現状・問題点
        [Required(ErrorMessage = "現状・問題点を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string? GenjyoMondaiten { get; set; }

        //改善案
        [Required(ErrorMessage = "改善案を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string? Kaizenan { get; set; }

        //効果（実施）
        public string? KoukaJishi { get; set; }

        //効果
        [Required(ErrorMessage = "効果を入力してください。")]
        [MaxLength(2000, ErrorMessage = "2,000文字以内で入力してください。")]
        public string? Kouka { get; set; }

        //添付書類
        public IFormFile? TenpuFile1 { get; set; }
        public IFormFile? TenpuFile2 { get; set; }
        public IFormFile? TenpuFile3 { get; set; }
        public IFormFile? TenpuFile4 { get; set; }
        public IFormFile? TenpuFile5 { get; set; }
    }
}