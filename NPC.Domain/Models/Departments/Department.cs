using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Departments
{
    public class Department : IAggregateRoot
    {
        public virtual Guid Id { get; set; }
        public virtual User Manager { get; set; }
    }
}
