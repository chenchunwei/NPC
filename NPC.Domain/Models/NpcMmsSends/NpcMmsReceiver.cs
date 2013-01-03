using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Models.NpcMmsSends
{
    public class NpcMmsReceiver
    {
        public virtual Guid Id { get; set; }
        public virtual string TelNum { get; set; }
        public virtual NpcMmsSend NpcMmsSend { get; set; }
        public virtual string MessageId { get; set; }
        public virtual string SendStatus { get; set; }
        public virtual string DealStatus { get; set; }
    }
}
