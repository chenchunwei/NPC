using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowNodeInstances;

namespace NPC.Domain.Model.Mappings.FlowNodeInstances
{
    public class FlowNodeInstanceUserStateMap : ClassMap<FlowNodeInstanceUserState>
    {
        public FlowNodeInstanceUserStateMap()
        {
            Id(o => o.Id).GeneratedBy.Assigned();
            Map(o => o.ExecuteStatus).CustomType<ExecuteStatus>();
            References(o => o.User).Column("UserId");
            References(o => o.FlowNodeAction).Column("FlowNodeActionId");
            Component(o => o.RecordDescription);
            Table("FlowNodeInstanceUserStates");
        }
    }
}
                               