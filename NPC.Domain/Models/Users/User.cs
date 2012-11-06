using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.Users
{
    public class User : IAggregateRoot
    {
        public virtual string Name { get; set; }
        public virtual string Pwd { get; set; }
        public virtual DateTime DateOfCreate { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Department Department { get; set; }
    }
}
