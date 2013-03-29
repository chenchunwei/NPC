using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcSmses;

namespace NPC.Domain.Model.Mappings.NpcSmses
{
    public class NpcSmsMap : ClassMap<NpcSms>
    {
        public NpcSmsMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Content);
            Map(o => o.IsNeedSignature);
            Component(o => o.RecordDescription);
            Map(o => o.SignatureType).CustomType<SignatureType>();
            Map(o => o.SmsType).CustomType<SmsType>();
            Table("NpcSmses");
        }
    }
}
