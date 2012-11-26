using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Agents
{
    public class Agent
    {
        public Agent()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual User ActualUser { get; set; }
        public virtual User UserOfAgent { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
