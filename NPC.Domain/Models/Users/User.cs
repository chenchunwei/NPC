using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using Fluent.Infrastructure.Domain;
using Fluent.Infrastructure.Web.HttpMoudles;
using NPC.Domain.Models.Agents;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.Users
{
    public class User : IAggregateRoot
    {
        public User()
        {
            RecordDescription = new RecordDescription();
            Agents=new List<Agent>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Account { get; set; }
        public virtual string Name { get; set; }
        public virtual string Pwd { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Department Department { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual bool Sex { get; set; }
        public virtual IList<Agent> Agents { get; set; }
    }
}
