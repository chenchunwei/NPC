using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Departments
{
    public class Department : IAggregateRoot
    {
        public Department()
        {
            RecordDescription = new RecordDescription();
            Departments = new List<Department>();
        }

        public virtual Department Parent { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual User Manager { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual IList<Department> Departments { get; set; }
    }
}
