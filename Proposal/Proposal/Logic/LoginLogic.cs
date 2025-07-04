using Proposal.BL;
using Proposal.Models;

namespace Proposal.Logic
{
    public class LoginLogic
    {
        private readonly LoginBL _loginBL;

        public LoginLogic()
        {
            _loginBL = new LoginBL();
        }

        /// <summary>
        /// 处理登录流程：格式校验 + 调用BL层业务校验
        /// </summary>
        /// <returns>登录结果和消息</returns>
        public (bool Success, string Message) Login(string userId, string password,LoginModel pModel)
        {
            // 调用业务逻辑校验账号密码
            var User = _loginBL.ValidateUser(pModel);
            if (User != null)
            {
                return (true, "登录成功");
            }
            else
            {
                return (false, "账号或密码错误");
            }
        }
    }
}
