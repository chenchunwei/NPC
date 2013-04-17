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
            Id(o => o.Id).Column("UnitId").GeneratedBy.Foreign("Unit");
            HasOne(o => o.Unit).ForeignKey("UnitId");
            References(o => o.GovUnit).Column("GovUnitId");
            References(o => o.NpcUnit).Column("NpcUnitId"); ;
            HasManyToMany(o => o.SponsorUnits).ParentKeyColumn("UnitId")
                .ChildKeyColumn("SponsorUnitId")
                .Table("SponsorUnitsMap");
            HasManyToMany(o => o.SubsidiaryUnits).ParentKeyColumn("UnitId")
               .ChildKeyColumn("SubsidiaryUnitId")
               .Table("SubsidiaryUnitsMap");
            
        }
    }
}
