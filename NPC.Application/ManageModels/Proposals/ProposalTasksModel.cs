using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Tasks;

namespace NPC.Application.ManageModels.Proposals
{
    public class ProposalTasksModel
    {
        public ProposalTasksModel()
        {
            FlowNodeInstanceTasks = new List<FlowNodeInstanceTask>();
            Proposals = new List<Proposal>();
        }
        public IList<FlowNodeInstanceTask> FlowNodeInstanceTasks { get; set; }
        public IList<Proposal> Proposals { get; set; }
        public Proposal GetProposal(FlowNodeInstanceTask task)
        {
            return Proposals.First(o => o.Id == task.FlowNodeInstance.BelongsFlow.Id);
        }
    }
}
