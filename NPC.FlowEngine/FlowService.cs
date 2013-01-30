﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
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
        public FlowService()
        {
            _flowRepository = new FlowRepository();
            _flowNodeInstanceRepository = new FlowNodeInstanceRepository();
            _flowTypeRepository = new FlowTypeRepository();
        }

        public void CreateFlowWithAssignId(Guid flowId, string flowName, User originator, string title, Dictionary<string, string> args = null, string comment = null)
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
                var history = new FlowHistory()
                {
                    Comment = comment,
                    Operator = "提交流程",
                    Stage = "发起流程"
                };
                history.RecordDescription.CreateBy(originator);
                flow.FlowHistories.Add(history);
                _flowRepository.Save(flow);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
        public void ExecuteTask(Guid taskId, string actionName, User executor, Dictionary<string, string> args = null, string comment = null)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var flowNodeInstance = _flowNodeInstanceRepository.Find(taskId);
                flowNodeInstance.Execute(actionName, executor);
                flowNodeInstance.BelongsFlow.WriteDataFields(args);
                _flowNodeInstanceRepository.Save(flowNodeInstance);
                var flow = flowNodeInstance.BelongsFlow;
                var history = new FlowHistory()
                {
                    Comment = comment,
                    Operator = actionName,
                    Stage = flowNodeInstance.BelongsFlowNode.Name
                };
                history.RecordDescription.CreateBy(executor);
                flow.FlowHistories.Add(history);
                _flowRepository.Save(flow);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public IList<Task> GetTasks(Guid userId, string flowTypeName, int pageIndex = 0, int pageSize = 0)
        {
            return null;
        }
    }
}
