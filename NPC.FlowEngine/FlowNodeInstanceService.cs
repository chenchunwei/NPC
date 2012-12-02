using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.FlowEngine
{
    public class FlowNodeInstanceService
    {
        private readonly FlowNodeInstanceRepository _flowNodeInstanceRepository;
        private readonly UserRepository _userRepository;
        public FlowNodeInstanceService()
        {
            _flowNodeInstanceRepository = new FlowNodeInstanceRepository();
            _userRepository = new UserRepository();
        }
        //HACK:想办法支持嵌套事务，可以统一提交，这样就可以重用更多的代码了
        public void ExecuteFlowNodeInstance(Guid flowNodeInstanceId, string actionName, User executor, string comment,
           Dictionary<string, string> args = null)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var flowNodeInstance = _flowNodeInstanceRepository.Find(flowNodeInstanceId);
                if (flowNodeInstance == null)
                    throw new ArgumentException("该任务未找到对应的流程节点对象");
                var action = flowNodeInstance.BelongsFlowNode.FlowNodeActions.Single(o => o.Name == actionName);
                flowNodeInstance.Execute(actionName, executor);
                var executorText = string.Empty;
                if (flowNodeInstance.BelongsFlowNode.ExecutorType == FlowValueType.ByValue)
                {
                    executorText = flowNodeInstance.BelongsFlowNode.ExecutorValue;
                }
                if (flowNodeInstance.BelongsFlowNode.ExecutorType == FlowValueType.ByDataField)
                {
                    executorText = flowNodeInstance.BelongsFlow.FlowDataFields.Single(o => o.Name == flowNodeInstance.BelongsFlowNode.ExecutorValue).Value;
                }
                var executorIds = executorText.Split(';');
                //读取用户信息
                var users = _userRepository.GetUsers(executorIds.Select(Guid.Parse).ToArray());
                users.ToList().ForEach(user => flowNodeInstance.FlowNodeInstanceUserStates.Add(new FlowNodeInstanceUserState { User = user }));
                _flowNodeInstanceRepository.Save(flowNodeInstance);
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
