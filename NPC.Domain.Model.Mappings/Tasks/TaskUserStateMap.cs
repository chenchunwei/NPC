using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Tasks;

namespace NPC.Domain.Model.Mappings.Tasks
{
    public sealed class TaskUserStateMap : ClassMap<TaskUserState>
    {
        public TaskUserStateMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.TaskStatus).CustomType<TaskStatus>();
            Component(o => o.RecordDescription);
            References(o => o.User).Column("UserId");
            Table("TaskUserStates");
        }
     }
}
