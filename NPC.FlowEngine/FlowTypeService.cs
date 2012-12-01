using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Repository;

namespace NPC.FlowEngine
{
    public class FlowTypeService
    {
        private readonly FlowTypeRepository _flowTypeRepository;
        public FlowTypeService()
        {
            _flowTypeRepository = new FlowTypeRepository();
        }

       
    }
}
