using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.ClientNodeInstances
{
    public class ClientNodeInstance : IAggregateRoot
    {
        public ClientNodeInstance()
        {
            Users = new List<User>();
        }
        public virtual Guid Id { get; set; }
        public virtual DateTime? TimeOfFinished { get; set; }
        public virtual Flow BelongsFlow { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual ClientNode BelongsClientNode { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
