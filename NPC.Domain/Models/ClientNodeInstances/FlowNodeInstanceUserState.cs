using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public class FlowNodeInstanceUserState
    {
        public FlowNodeInstanceUserState()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual User User { get; set; }
        public virtual ExecuteStatus ExecuteStatus { get; set; }
        public virtual FlowNodeAction FlowNodeAction { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
