using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Flows;

namespace NPC.Domain.Repository
{
    public class FlowRepository : AbstractNhibernateRepository<Guid, Flow>
    {
        public IList<Flow> GetInstanceFlow()
        {
            return Session.CreateQuery("from Flow where IsDelete=0 and FlowStatus in (:FlowStatus)")
                .SetParameterList("FlowStatus", new object[]{FlowStatus.Instance}).List<Flow>();
        }
    }
}
