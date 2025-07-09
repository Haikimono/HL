using Microsoft.AspNetCore.Http.HttpResults;
using Proposal.DAC;
using Proposal.Models;

namespace Proposal.BL
{
    public class CreateBL
    {
        private readonly CreateDAC _createDAC;

        public CreateBL(string connectionString)
        {
            _createDAC = new CreateDAC(connectionString);
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        public void TeianshoTeishutsu(CreateModel pModel)
        {
            _createDAC.TeianshoTeishutsu(pModel);
        }
    }
}
