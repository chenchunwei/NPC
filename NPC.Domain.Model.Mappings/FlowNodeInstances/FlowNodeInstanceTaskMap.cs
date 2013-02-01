using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowNodeInstances;

namespace NPC.Domain.Model.Mappings.FlowNodeInstances
{
    public class FlowNodeInstanceTaskMap : ClassMap<FlowNodeInstanceTask>
    {
        public FlowNodeInstanceTaskMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.TaskStatus).CustomType<TaskStatus>();
            Map(o => o.UserId);
            Map(o => o.IsOpened);
            References(o => o.FlowNodeAction).Column("FlowNodeActionId");
            Component(o => o.RecordDescription);
            References(o => o.FlowNodeInstance).Column("FlowNodeInstanceId");
            Table("FlowNodeInstanceTasks");
        }
    }
}
                               