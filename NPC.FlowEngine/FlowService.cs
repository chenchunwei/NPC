using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Application.Contexts;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.FlowEngine
{
    public class FlowService
    {
        private readonly FlowRepository _flowRepository;
        private readonly FlowTypeRepository _flowTypeRepository;
        private readonly FlowNodeInstanceRepository _flowNodeInstanceRepository;
        private readonly TaskRepository _taskRepository;
        public FlowService()
        {
            _flowRepository = new FlowRepository();
            _flowNodeInstanceRepository = new FlowNodeInstanceRepository();
            _flowTypeRepository = new FlowTypeRepository();
            _taskRepository = new TaskRepository();
        }

        public void CreateFlowWithAssignId(Guid flowId, string flowName, User originator, string title, Dictionary<string, string> args = null)
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
                args = args ?? new Dictionary<string, string>();
                args.ToList().ForEach(o => flow.FlowDataFields.Add(new FlowDataField { Name = o.Key, Value = o.Value }));
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
        public void ExecuteTask(Guid taskId, string actionName, User executor, Dictionary<string, string> args = null)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var task = _taskRepository.Find(taskId);
                if (task.GroupName != TaskConst.FlowTaskGroup)
                    throw new ApplicationException("该任务非流程任务");
                var flowNodeInstance = _flowNodeInstanceRepository.Find(Guid.Parse(task.Body));
                flowNodeInstance.Execute(actionName, executor);
                flowNodeInstance.BelongsFlow.WriteDataFields(args);
                task.Done(executor, TaskStatus.Finished);
                _flowNodeInstanceRepository.Save(flowNodeInstance);
                _taskRepository.Save(task);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
    }
}
