using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Common;
using NPC.Application.ManageModels.Proposals;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class ProposalAction : BaseAction
    {
        private ProposalRepository _proposalRepository;
        public ProposalAction()
        {
            _proposalRepository = new ProposalRepository();
        }

        public EditProposalModel InitializeEditProposalModel(Guid? id)
        {
            var model = new EditProposalModel();
            model.ProposalTypeOptions = Helper.GetProposalTypeOptions();
            model.CurrentUser = NpcContext.CurrentUser;
            return model;
        }
    }
}
