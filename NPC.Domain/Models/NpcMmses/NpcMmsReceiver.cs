using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.NpcMmses
{
    public class NpcMmsReceiver
    {
        public virtual Guid Id { get; set; }
        public virtual string TelNum { get; set; }
        public virtual NpcMms NpcMms { get; set; }
        public virtual string MessageId { get; set; }
        public virtual string SendStatus { get; set; }
        public virtual string DealStatus { get; set; }
    }
}
