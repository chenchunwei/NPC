using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcMmsSends;

namespace NPC.Domain.Model.Mappings.NpcMmsSends
{
    public class NpcMmsSendMap : ClassMap<NpcMmsSend>
    {
        public NpcMmsSendMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            HasMany(o => o.NpcMmsReceivers).KeyColumn("NpcMmsSendId").Cascade.All();
            Component(o => o.RecordDescription);
            Map(o => o.Title);
            Map(o => o.Extension);
            Map(o => o.SendStatus).CustomType<SendStatus>();
            Map(o => o.TimeOfExceptSend);
            References(o => o.Unit).Column("UnitId");
            References(o => o.NpcMms).Column("NpcMmsId");
            Table("NpcMmsSends");
        }
    }
}
