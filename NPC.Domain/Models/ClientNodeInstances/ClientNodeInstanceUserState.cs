using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.ClientNodeInstances
{
    public class ClientNodeInstanceUserState
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public ExecuteStatus ExecuteStatus { get; set; }
    }
}
