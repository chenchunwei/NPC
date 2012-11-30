﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Tasks;

namespace NPC.Domain.Repository
{
    public class ProposalRepository : AbstractNhibernateRepository<Guid, Proposal>
    {
    }
}
