using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Tasks;

namespace NPC.Domain.Model.Mappings.Tasks
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Description);
            Map(o => o.OuterId);
            Map(o => o.Title);
            Map(o => o.TaskStatus).CustomType<TaskStatus>();
            Component(o => o.RecordDescription);
            HasManyToMany(o => o.TaskProcessers).ChildKeyColumn("UserId").ParentKeyColumn("TaskId");
            Table("Tasks");
        }
    }
}
