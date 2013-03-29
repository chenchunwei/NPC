using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcSmsSends;

namespace NPC.Domain.Models.NpcSmsSends
{
    public class NpcSmsSendReceiver
    {
        public virtual Guid Id { get; set; }
        public virtual NpcSmsSend NpcSmsSend { get; set; }
        public virtual string TelNumber { get; set; }
    }
}
