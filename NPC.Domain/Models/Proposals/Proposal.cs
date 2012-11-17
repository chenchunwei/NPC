using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Proposals
{
    public class Proposal
    {
        public Proposal()
        {
            ProposalOriginators=new List<User>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual ProposalType ProposalType { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual IList<User> ProposalOriginators { get; set; }
        public virtual IList<Task> Tasks { get; set; }
    }
}
