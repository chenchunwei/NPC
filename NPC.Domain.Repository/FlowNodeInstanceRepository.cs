using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.FlowNodeInstances;

namespace NPC.Domain.Repository
{
    public class FlowNodeInstanceRepository : AbstractNhibernateRepository<Guid, FlowNodeInstance>
    {
        public IList<FlowNodeInstance> GetUnDeals()
        {
            return Session.CreateQuery("from FlowNodeInstance where InstanceStatus in(:InstanceStatus) and RecordDescription.IsDelete=0")
                    .SetParameterList("InstanceStatus", new object[] {InstanceStatus.ActionCompleted})
                    .List<FlowNodeInstance>();
        }
    }
}
