using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.Departments;

namespace NPC.Domain.Models.Units
{
    public class Unit : IAggregateRoot
    {
        public virtual Guid Id { get; set; }
        public virtual IList<Department> Departments { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DateOfCreate { get; set; }
        public virtual string BannerImgUrl { get; set; }
    }
}
