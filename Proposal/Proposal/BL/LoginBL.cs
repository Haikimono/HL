using Proposal.DAC;
using Proposal.Models;

namespace Proposal.BL
{
    public class LoginBL
    {
        private readonly LoginDAC _LoginDAC;

        public LoginBL()
        {
        }

        public LoginBL(string connectionString)
        {
            _LoginDAC = new LoginDAC(connectionString);
        }

        /// <summary>
        /// 验证用户账号密码
        /// </summary>
        /// <returns>登录成功返回用户对象，否则返回 null</returns>
        public bool ValidateUser(string userId, string password)
        {
            var user = _LoginDAC.GetUserById(userId);
            if (user != null && user.Password == password) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
