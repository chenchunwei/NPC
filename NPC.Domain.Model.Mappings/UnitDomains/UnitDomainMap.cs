using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.UnitDomains;

namespace NPC.Domain.Model.Mappings.UnitDomains
{
    public class UnitDomainMap : ClassMap<UnitDomain>
    {
        public UnitDomainMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            HasManyToMany(o => o.Units).ParentKeyColumn("UnitDomainId")
                                       .ChildKeyColumn("UnitId")
                                       .Table("UnitDomainUnitMap");
        }
    }
}
