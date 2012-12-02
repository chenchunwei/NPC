using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public class FlowNodeLineMap : ClassMap<FlowNodeLine>
    {
        public FlowNodeLineMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.RuleCode);
            References(o => o.ContactTo).Column("FlowNodeIdOfContactTo");
            Component(o => o.RecordDescription);
            Table("FlowNodeLines");
        }
    }
}
