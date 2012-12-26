using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NpcMmses;
using NPC.Domain.Models.NpcSmses;

namespace NPC.Domain.Repository
{
    public class NpcSmsRepository : AbstractNhibernateRepository<Guid, NpcSms>
    {
    }
}
