using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Flows;

namespace NPC.Domain.Model.Mappings.Flows
{
    public class FlowDataFieldMap : ClassMap<FlowDataField>
    {
        public FlowDataFieldMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.Value);
            Table("FlowDataFields");
        }
    }
}
