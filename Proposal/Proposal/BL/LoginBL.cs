using Proposal.DAC;
using Proposal.Models;

namespace Proposal.BL
{
    public class LoginBL
    {
        private readonly LoginDAC _LoginDAC;

        public LoginBL(string connectionString)
        {
            _LoginDAC = new LoginDAC(connectionString);
        }

        /// <summary>
        /// 验证用户账号密码
        /// </summary>
        /// <returns>登录成功返回用户对象，否则返回 null</returns>
        public User ValidateUser(LoginModel pModel)
        {
            var user = _LoginDAC.GetUserById(pModel);
            if (user != null && user.Password == pModel.Password) 
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
