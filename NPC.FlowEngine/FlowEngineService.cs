using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Repository;

namespace NPC.FlowEngine
{
    public class FlowEngineService
    {
        private readonly ClientNodeInstanceRepository _clientNodeInstanceRepository;
        private readonly FlowRepository _flowRepository;
        private readonly FlowTypeRepository _flowTypeRepository;
        public FlowEngineService()
        {
            _clientNodeInstanceRepository = new ClientNodeInstanceRepository();
            _flowRepository = new FlowRepository();
            _flowTypeRepository = new FlowTypeRepository();
        }

        public void CreateClientNodeInstance()
        {
            var instances = _flowRepository.GetInstanceFlow();
            instances.ToList().ForEach(flow =>
            {
                var flowType = _flowTypeRepository.Find(flow.FlowType.Id);

            });
        }

        public ClientNode GetFirstNodeOfFlowType(FlowType flowType)
        {
            return new ClientNode();
        }
    }
}
