using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Repository
{
    public class NpcMmsRepository : AbstractNhibernateRepository<Guid, NpcMms>
    {

    }
}
