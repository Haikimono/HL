using System.Runtime.InteropServices;

namespace Proposal.Models
{
    /// <summary>
    /// 作成状態
    /// </summary>
    public enum ProposalStatus
    {
        作成中 = 1,
        差戻中 = 9,
        一次審査中 = 11,
        二次審査中 = 12,
        三次審査中 = 13,
        局へ提出済 = 20,
        庁へ提出済 = 30,
        庁入賞 = 21,
        局入賞 = 31,
        選外 = 99
    }

    /// <summary>  
    /// 所属  
    /// </summary>  
    public enum ProposalFrom
    {
        国税庁 = 1,
        国税局 = 2,
        税務署_センター = 3, // Replace "・" with "_" to avoid invalid character error  
        税大 = 4,
        審判所 = 5
    }

    /// <summary>  
    /// 判定  
    /// </summary>  
    public enum ProposalDecision
    {
        Accept = 1, // Replace "○" with a valid identifier
        Reject = 0  // Replace "×" with a valid identifier
    }

    /// <summary>  
    /// 追加進達  
    /// </summary>  
    public enum Proposalresubmission
    {
        Accept = 1, // Replace "○" with a valid identifier
        Reject = 0  // Replace "×" with a valid identifier
    }
    public class ProposalList
    {
        public string ProposalId { get; set; }
        public string UserId { get; set; }
        public string? ProposalYear { get; set; }
        public int Status { get; set; }
        public string? ProposalName { get; set; }
        public string? UserName { get; set; }
        //public int? From { get; set; } 
        //public DateTime? SubmittedAt { get; set; }
        //public int? Point { get; set; }
        //public int? Decision { get; set; }
        //public bool Resubmission { get; set; }
        //public int? ReviewSection { get; set; }
        //public int? RelatedSections { get; set; }
        //public int? AwardDivision { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public bool Delete { get; set; }

        public string StatusDisplay =>
            Status switch
            {
                1 => "作成中",
                9 => "差戻中",
                11 => "一次審査中",
                12 => "二次審査中",
                13 => "三次審査中",
                20 => "局へ提出済",
                30 => "庁へ提出済",
                21 => "庁入賞",
                31 => "局入賞",
                99 => "選外",
                _ => "不明"
            };

        //public string FromDisplay =>
        //    From switch
        //    {
        //       1 => "国税庁",
        //       2 => "国税局 ",
        //       3 => "税務署・センター ",
        //       4 => "税大",
        //       5 => "審判所",
        //       _ => "不明"
        //    };
         
        //public string DecisionDisplay =>
        //    Decision switch
        //    {
        //        1 => "○",
        //        0 => "×",
        //        _ => "不明"
        //    };

    }
}
