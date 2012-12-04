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
        private readonly FlowRepository _flowRepository;
        public FlowNodeInstanceService( )
        {
            _flowRepository = new FlowRepository();
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
                flowNodeInstance.Execute(actionName, executor);
                _flowNodeInstanceRepository.Save(flowNodeInstance);
                flowNodeInstance.BelongsFlow.WriteDataFields(args);
                _flowRepository.Save(flowNodeInstance.BelongsFlow);
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
