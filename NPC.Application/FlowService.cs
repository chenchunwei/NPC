using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Contexts;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class FlowService
    {
        private readonly FlowRepository _flowRepository;
        private readonly FlowTypeRepository _flowTypeRepository;
        public FlowService()
        {
            _flowRepository = new FlowRepository();
            _flowTypeRepository = new FlowTypeRepository();
        }

        public void CreateFlowWithAssignId(Guid flowId, string flowName, User originator, string title)
        {
            var flow = new Flow();
            flow.FlowStatus = FlowStatus.Instance;
            var flowType = _flowTypeRepository.GetTypeName(flowName);
            if (flowType == null)
            {
                throw new Exception(string.Format("{0}的流程类型不存在", flowName));
            }
            flow.FlowType = flowType;
            flow.Id = flowId;
            flow.Title = title;
            flow.RecordDescription.CreateBy(originator);
            flow.UserOfFlowAdmin = originator;
            _flowRepository.Save(flow);
        }
    }
}
