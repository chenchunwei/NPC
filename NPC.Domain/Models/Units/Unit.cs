using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Units
{
    public class Unit : IAggregateRoot
    {
        public Unit()
        {
            UnitStatus = UnitStatus.Disable;
            Departments = new List<Department>();
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual IList<Department> Departments { get; set; }
        public virtual string Name { get; set; }
        public virtual string BannerImgUrl { get; set; }
        public virtual Unit ParentUint { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual UnitStatus UnitStatus { get; set; }
        public virtual User Manager { get; set; }
    }
}
