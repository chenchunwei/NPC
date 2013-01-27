using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Proposals
{
    public class Proposal : IAggregateRoot
    {
        public Proposal()
        {
            RecordDescription = new RecordDescription();
            ProposalOriginators = new List<User>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual ProposalType ProposalType { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        /// <summary>
        /// 附议人
        /// </summary>
        public virtual IList<User> ProposalOriginators { get; set; }
        public virtual IList<Task> Tasks { get; set; }
    }
}
