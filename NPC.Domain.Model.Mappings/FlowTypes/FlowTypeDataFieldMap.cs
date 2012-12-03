using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public class FlowTypeDataFieldMap : ClassMap<FlowTypeDataField>
    {
        public FlowTypeDataFieldMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.DefaultValue);
            Table("FlowTypeDataFields");
        }
    }
}
