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
            Map(o => o.AppAccount);
            Map(o => o.AppPwd);
            Map(o => o.ExtensionNo);
            Map(o => o.MasService);
            Map(o => o.Signature);
            References(o => o.Unit).Column("UnitId");
            Component(o => o.RecordDescription);
            Table("OpenMasConfigs");
        }
    }
}
