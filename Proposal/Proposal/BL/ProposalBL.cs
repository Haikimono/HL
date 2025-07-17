using Proposal.DAC;
using Proposal.Models;

namespace Proposal.BL
{
    public class ProposalBL
    {
        private readonly ProposalDAC _dac;

        public ProposalBL(string connectionString)
        {
            _dac = new ProposalDAC(connectionString);
        }

        public List<ProposalList> GetProposalList()
        {
            return _dac.GetProposals();
        }
    }
}
