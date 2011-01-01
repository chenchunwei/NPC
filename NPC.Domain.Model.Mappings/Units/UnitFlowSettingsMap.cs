using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Model.Mappings.Units
{
    public class UnitFlowSettingsMap : ClassMap<UnitFlowSettings>
    {
        public UnitFlowSettingsMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            References(o => o.Unit).Column("UnitId");
            References(o => o.GovUnit).Column("GovUnitId");
            References(o => o.NpcUnit).Column("NpcUnitId"); ;
            HasManyToMany(o => o.SponsorUnits).ChildKeyColumn("UnitId")
                .ParentKeyColumn("UnitFlowSettingsId")
                .Table("SponsorUnitsMap");
            HasManyToMany(o => o.SubsidiaryUnits).ChildKeyColumn("UnitId")
               .ParentKeyColumn("UnitFlowSettingsId")
               .Table("SubsidiaryUnitsMap");
            
        }
    }
}
