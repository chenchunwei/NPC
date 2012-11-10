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
            Component(o => o.RecordDescription);
            Table("Units");
        }
    }
}
