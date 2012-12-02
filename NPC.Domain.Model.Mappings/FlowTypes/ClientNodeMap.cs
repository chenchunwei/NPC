using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public sealed class FlowNodeMap : ClassMap<FlowNode>
    {
        public FlowNodeMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.ProcessUrl);
            HasMany(o => o.FlowNodeActions).KeyColumn("FlowNodeId").Cascade.All();
            HasMany(o => o.FlowNodeLines).KeyColumn("FlowNodeId").Cascade.All();
            Component(o => o.RecordDescription);
            Map(o => o.IsFirstNode);
            Table("FlowNodes");
        }
    }
}
