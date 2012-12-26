using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.NpcMmses
{
    public class NpcMms : IAggregateRoot
    {
        public NpcMms()
        {
            RecordDescription = new RecordDescription();
            NpcMmsReceivers = new List<NpcMmsReceiver>();
        }

        public virtual Guid Id { get; set; }
        public virtual IList<NpcMmsReceiver> NpcMmsReceivers { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime TimeOfExceptSend { get; set; }
        public virtual int ByteSize { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }

    }
}
