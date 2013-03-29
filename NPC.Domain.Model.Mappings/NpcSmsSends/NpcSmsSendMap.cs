using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcSmsSends;

namespace NPC.Domain.Model.Mappings.NpcSmsSends
{
    public class NpcSmsSendMap : ClassMap<NpcSmsSend>
    {
        public NpcSmsSendMap()
        {
            Id(o => o.Id);
            HasMany(o => o.NpcSmsSendReceivers).KeyColumn("NpcSmsSendId").Cascade.All();
            Map(o => o.MessageId);
            Component(o => o.RecordDescription);
            Map(o => o.TimeOfCallback);
            Map(o => o.TimeOfExpectSend);
            References(o => o.NpcSms).Column("NpcSmsId");
            References(o => o.Unit).Column("UnitId");
            Table("NpcSmsSends");
        }
    }
}
