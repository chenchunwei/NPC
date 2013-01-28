using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Model.Mappings.Flows
{
    public class FlowMap : ClassMap<Flow>
    {
        public FlowMap()
        {
            Id(o => o.Id).GeneratedBy.Assigned();
            Map(o => o.DateTimeofFinished);
            Map(o => o.Title);
            Map(o => o.FlowStatus).CustomType<FlowStatus>();
            References(o => o.FlowType).Column("FlowTypeId");
            References(o => o.UserOfFlowAdmin).Column("UserIdOfFlowAdmin");
            Component(o => o.RecordDescription);
            HasMany(o => o.FlowDataFields).KeyColumn("FlowId").Cascade.All();
            HasMany(o => o.FlowHistories).KeyColumn("FlowId").Cascade.All();
            Table("Flows");
        }
    }
}
