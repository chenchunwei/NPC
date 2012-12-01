using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.ClientNodeInstances
{
    public class ClientNodeInstanceUserState
    {
        public ClientNodeInstanceUserState()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual User User { get; set; }
        public virtual ExecuteStatus ExecuteStatus { get; set; }
        public virtual ClientNodeAction ClientNodeAction { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
