using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Tasks;

namespace NPC.Application.ManageModels.Tasks
{
    public class MyTasksModel
    {
        public MyTasksModel()
        {
            Tasks = new List<Task>();
        }
        public IList<Task> Tasks { get; set; }
    }
}
