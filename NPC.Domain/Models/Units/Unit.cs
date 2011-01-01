using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using Fluent.Infrastructure.Domain;
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
        public virtual bool IsWebUint { get; set; }
        public virtual bool IsFlowUint { get; set; }
        public virtual User JieKouRen { get; set; }
        public virtual string AliasName { get; set; }
        public virtual UnitFlowSettings UnitFlowSettings { get; set; }

        public virtual string Path
        {
            get
            {
                var path = string.Empty;
                var nodes = new Stack<Unit>();
                nodes.Push(this);
                while (nodes.Any())
                {
                    var o = nodes.Pop();
                    path = string.Format("{0};{1}", o.Id, path);
                    if (o.ParentUint != null)
                        nodes.Push(o.ParentUint);
                }
                return path.TrimEnd(';');
            }
            set { return; }
        }
    }
}

