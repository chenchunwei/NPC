using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcSmsSends;

namespace NPC.Domain.Model.Mappings.NpcSmsSends
{
    public class NpcSmsSendReceiverMap : ClassMap<NpcSmsSendReceiver>
    {
        public NpcSmsSendReceiverMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            References(o => o.NpcSmsSend).Column("NpcSmsSendId");
            Map(o => o.TelNumber);
            Table("NpcSmsSendReceivers");
        }
    }
}
