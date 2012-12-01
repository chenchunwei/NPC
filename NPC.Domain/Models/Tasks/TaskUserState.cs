using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Tasks
{
    public class TaskUserState
    {
        public TaskUserState()
        {
            RecordDescription=new RecordDescription();
        }

        public virtual Guid Id { get; set; }
        public virtual User User { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }
        public RecordDescription RecordDescription { get; set; }
    }
}
