using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;
using Fluent.Infrastructure.Domain;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public class FlowNodeInstanceTask : IAggregateRoot
    {
        public FlowNodeInstanceTask()
        {
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual FlowNodeInstance FlowNodeInstance { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }
        public virtual FlowNodeAction FlowNodeAction { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual bool IsOpened { get; set; }
    }
}
