using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Repository
{
    public class FlowTypeRepository : AbstractNhibernateRepository<Guid, FlowType>
    {
        public FlowType GetTypeName(string flowName)
        {
            return Session.CreateQuery("from FlowType where Name=:Name and IsDelete=0").SetString("Name", flowName)
                          .UniqueResult<FlowType>();
        }
    }
}
