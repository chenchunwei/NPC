using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Model.Mappings.NpcMmses
{
    public class NpcMmsReceiverMap : ClassMap<NpcMmsReceiver>
    {
        public NpcMmsReceiverMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.DealStatus);
            Map(o => o.MessageId);
            References(o => o.NpcMms).Column("NpcMmsId");
            Map(o => o.SendStatus);
            Map(o => o.TelNum);
            Table("NpcMmsReceivers");
        }
    }
}
