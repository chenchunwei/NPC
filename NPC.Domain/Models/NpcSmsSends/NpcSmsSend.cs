using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.NpcSmses;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.NpcSmsSends
{
    public class NpcSmsSend:IAggregateRoot
    {
        public NpcSmsSend()
        {
            NpcSmsSendReceivers=new List<NpcSmsSendReceiver>();
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual NpcSms NpcSms { get; set; }
        public virtual string MessageId { get; set; }
        public virtual IList<NpcSmsSendReceiver> NpcSmsSendReceivers { get; set; }
        public virtual DateTime? TimeOfExpectSend { get; set; }
        public virtual DateTime? TimeOfCallback { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
