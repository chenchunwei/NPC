using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Model.Mappings.NpcMmsSends
{
    public class NpcMmsReceiverMap : ClassMap<NpcMmsReceiver>
    {
        public NpcMmsReceiverMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.DealStatus);
            Map(o => o.MessageId);
            References(o => o.NpcMmsSend).Column("NpcMmsId");
            Map(o => o.SendStatus);
            Map(o => o.TelNum);
            Table("NpcMmsReceivers");
        }
    }
}
