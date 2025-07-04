using Proposal.BL;

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
        public (bool Success, string Message) Login(string userId, string password)
        {
            // 调用业务逻辑校验账号密码
            bool valid = _loginBL.ValidateUser(userId, password);
            if (valid)
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
