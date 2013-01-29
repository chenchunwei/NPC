using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Tasks
{
    public class TaskQueryItem : QueryItemBase
    {
        public string Title { get; set; }
        public string GroupName { get; set; }
        public string TypeName { get; set; }
        public Guid? UserId { get; set; }
    }
}
