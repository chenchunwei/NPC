using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.NpcMmses;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.NpcMmsSends
{
    public class NpcMmsSend: IAggregateRoot
    {
        public NpcMmsSend()
        {
            RecordDescription = new RecordDescription();
            NpcMmsReceivers = new List<NpcMmsReceiver>();
        }
        public virtual Guid Id { get; set; }
        public virtual IList<NpcMmsReceiver> NpcMmsReceivers { get; set; }
        public virtual string Title { get; set; }
        public virtual NpcMms NpcMms { get; set; }
        public virtual DateTime? TimeOfExceptSend { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual string Extension { get; set; }
        public virtual string MessageId { get; set; }
        public virtual SendStatus SendStatus { get; set; }

    }
}