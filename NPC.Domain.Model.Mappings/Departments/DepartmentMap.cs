using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Departments;

namespace NPC.Domain.Model.Mappings.Departments
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.Path);
            References(o => o.Manager).Column("ManagerId");
            References(o => o.Unit).Column("UnitId");
            References(o => o.Parent).Column("ParentId");
            HasMany(o => o.Departments).KeyColumn("ParentId");
            Component(o => o.RecordDescription);
            Table("Departments");
        }
    }
}
