using System.ComponentModel.DataAnnotations;

namespace Proposal.Models
{
    public class ChangePassModel
{
    [Required(ErrorMessage = "新しいパスワードを入力してください。")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "パスワード（確認）を入力してください。")]
    [Compare("NewPassword", ErrorMessage = "パスワードが一致しません。")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}
}