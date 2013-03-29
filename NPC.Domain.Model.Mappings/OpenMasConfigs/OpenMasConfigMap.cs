using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.OpenMasConfigs;

namespace NPC.Domain.Model.Mappings.OpenMasConfigs
{
    public class OpenMasConfigMap : ClassMap<OpenMasConfig>
    {
        public OpenMasConfigMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.MmsAppAccount);
            Map(o => o.MmsAppPwd);
            Map(o => o.MmsExtensionNo);
            Map(o => o.MmsMasService);
            Map(o => o.SmsAppAccount);
            Map(o => o.SmsAppPwd);
            Map(o => o.SmsExtensionNo);
            Map(o => o.SmsMasService);
            Map(o => o.Signature);
            References(o => o.Unit).Column("UnitId");
            Component(o => o.RecordDescription);
            Table("OpenMasConfigs");
        }
    }
}
