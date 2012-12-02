using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Model.Mappings.Flows;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Model.Mappings.FlowNodeInstances
{
    public sealed class FlowNodeInstanceMap:ClassMap<FlowNodeInstance>
    {
        public FlowNodeInstanceMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            References(o => o.BelongsFlowNode).Column("BelongsFlowNodeId");
            References(o => o.BelongsFlow).Column("BelongsFlowId");
            References(o => o.FlowNodeAction).Column("FlowNodeActionId");
            Map(o => o.TimeOfFinished);
            Map(o => o.InstanceStatus).CustomType<InstanceStatus>();
            HasMany(o => o.FlowNodeInstanceUserStates).KeyColumn("FlowNodeInstanceId");
            Component(o => o.RecordDescription);
            Table("FlowNodeInstances");
        }
     }
}
