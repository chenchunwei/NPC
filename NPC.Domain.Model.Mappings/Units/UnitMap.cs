using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Model.Mappings.Units
{
    public class UnitMap : ClassMap<Unit>
    {
        public UnitMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.BannerImgUrl);
            References(o => o.ParentUint).Column("ParentUintId");
            Map(o => o.Name);
            Map(o => o.Path);
            Map(o => o.IsFlowUint);
            Map(o => o.IsWebUint);
            Map(o => o.UnitStatus).CustomType<UnitStatus>();
            References(o => o.JieKouRen).Column("JieKouRenId");
            Component(o => o.RecordDescription);
            HasMany(o => o.Departments).KeyColumn("UnitId");
            Table("Units");
        }
    }
}
