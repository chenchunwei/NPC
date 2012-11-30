using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Flows
{
    public class Flow : IAggregateRoot
    {
        public Flow()
        {
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual User UserOfFlowAdmin { get; set; }
        public virtual FlowType FlowType { get; set; }
        public virtual FlowStatus FlowStatus { get; set; }
        public virtual DateTime? DateTimeofFinished { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
