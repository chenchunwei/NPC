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
            Id(o => o.OuterId).GeneratedBy.GuidComb();
            Map(o => o.DateOfCreate);
            Map(o => o.Description);
            Map(o => o.OuterId);
            Map(o => o.Title);
            Map(o => o.UserIdOfCreate);
            Map(o => o.UserIdOfSendTo);
            Map(o => o.TaskStatus).CustomType<TaskStatus>();
            Table("Tasks");
        }
    }
}
