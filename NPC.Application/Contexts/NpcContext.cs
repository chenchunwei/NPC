using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Application.Contexts
{
    public class NpcContext
    {
        protected UserRepository UserRepository { get; set; }
        public NpcContext()
        {
            UserRepository = new UserRepository();

        }

        public User CurrentUser
        {
            get { return UserRepository.Find(new Guid("68155f4b-b352-4c27-b3af-a1050188e6ad")); }
        }

    }
}
