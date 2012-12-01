using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.ClientNodeInstances;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;
using log4net;
using ValueType = NPC.Domain.Models.FlowTypes.ValueType;

namespace NPC.FlowEngine
{
    public class FlowEngineService
    {
        private readonly ClientNodeInstanceRepository _clientNodeInstanceRepository;
        private readonly FlowRepository _flowRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        private readonly ILog _logger;
        public FlowEngineService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _clientNodeInstanceRepository = new ClientNodeInstanceRepository();
            _flowRepository = new FlowRepository();
            _taskRepository = new TaskRepository();
            _userRepository = new UserRepository();
        }

        public void CreateClientNodeInstance()
        {
            var instances = _flowRepository.GetInstanceFlow();
            instances.ToList().ForEach(CreateSingleClinentNodeInstance);
        }

        private void CreateSingleClinentNodeInstance(Flow flow)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var clientNode = flow.FlowType.GetFirstNode();
                //创建节点实例
                var clientNodeInstance = new ClientNodeInstance();
                var task = new Task();
                clientNodeInstance.BelongsClientNode = clientNode;
                clientNodeInstance.BelongsFlow = flow;
                //HACK:陈春伟,这里需要考虑以后把user抽象成一个接口的形式,不需要依赖具体的实现或直接只关注UserId会更简单
                var executorText = string.Empty;
                if (clientNode.ExecutorType == ValueType.ByValue)
                {
                    executorText = clientNode.ExecutorValue;
                }
                if (clientNode.ExecutorType == ValueType.ByDataField)
                {
                    executorText = flow.FlowDataFields.Single(o => o.Name == clientNode.ExecutorValue).Value;
                }

                var executorIds = executorText.Split(';');
                //读取用户信息
                var users = _userRepository.GetUsers(executorIds.Select(Guid.Parse).ToArray());
                users.ToList().ForEach(user =>
                {
                    clientNodeInstance.ClientNodeInstanceUserStates.Add(new ClientNodeInstanceUserState { User = user });
                    task.TaskUserStates.Add(new TaskUserState { User = user });
                });
                clientNodeInstance.RecordDescription.CreateBy(flow.RecordDescription.UserOfCreate);
                _clientNodeInstanceRepository.Save(clientNodeInstance);
                flow.FlowStatus = FlowStatus.Start;
                flow.RecordDescription.DateOfLastestModify = DateTime.Now;
                _flowRepository.Save(flow);

                task.Body = clientNodeInstance.Id.ToString();
                task.Description = "流程任务";
                task.GroupName = TaskConst.FlowTaskGroup;
                task.TypeName = TaskConst.FlowTaskType;
                task.OuterId = clientNodeInstance.Id;
                task.TaskProcessUrl = clientNodeInstance.BelongsClientNode.ProcessUrl;
                task.Title = flow.Title;

                _taskRepository.Save(task);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void DealClientNodeFlowTo()
        {

        }

        private void DealSingleClientNodeFlowTo(ClientNodeInstance clientNodeInstance)
        {
            var clientNode = clientNodeInstance.BelongsClientNode;
            foreach (var clientLine in clientNode.ClientNodeLines)
            {
                //clientLine.RuleCode == "";
            }
        }
    }
}
