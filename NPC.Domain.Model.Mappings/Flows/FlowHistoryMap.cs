using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Flows;

namespace NPC.Domain.Model.Mappings.Flows
{
    public class FlowHistoryMap : ClassMap<FlowHistory>
    {
        public FlowHistoryMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Comment);
            Map(o => o.Operator);
            Map(o => o.Stage);
            Component(o => o.RecordDescription);
            Table("FlowHistories");
        }
    }
}
