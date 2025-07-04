using System.ComponentModel.DataAnnotations;

namespace Proposal.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "ユーザーIDは必須です。")]
        [MinLength(4, ErrorMessage = "4文字以上入力してください。")]
        [MaxLength(20, ErrorMessage = "20文字以内で入力してください。")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "パスワードは必須です。")]
        [MinLength(8, ErrorMessage = "8文字以上入力してください。")]
        [MaxLength(32, ErrorMessage = "32文字以内で入力してください。")]
        public string Password { get; set; }
    }
}
