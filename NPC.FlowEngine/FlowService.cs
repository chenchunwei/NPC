using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Application.Contexts;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.FlowEngine
{
    public class FlowService
    {
        private readonly FlowRepository _flowRepository;
        private readonly FlowTypeRepository _flowTypeRepository;
        private readonly ClientNodeInstanceRepository _clientNodeInstanceRepository;
        private readonly TaskRepository _taskRepository;
        public FlowService()
        {
            _flowRepository = new FlowRepository();
            _clientNodeInstanceRepository=new ClientNodeInstanceRepository();
            _flowTypeRepository = new FlowTypeRepository();
            _taskRepository=new TaskRepository();
        }

        public void CreateFlowWithAssignId(Guid flowId, string flowName, User originator, string title,
            Dictionary<string, string> args = null)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var flow = new Flow();
                flow.FlowStatus = FlowStatus.Instance;
                var flowType = _flowTypeRepository.GetByTypeName(flowName);
                if (flowType == null)
                    throw new Exception(string.Format("{0}的流程类型不存在", flowName));
                flow.FlowType = flowType;
                flow.Id = flowId;
                flow.Title = title;
                flow.RecordDescription.CreateBy(originator);
                flow.UserOfFlowAdmin = originator;
                _flowRepository.Save(flow);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
        public void ExecuteTask(Guid taskId, string actionName, User executor,
            Dictionary<string, string> args = null)
        {
            
        }

       
    }
}
