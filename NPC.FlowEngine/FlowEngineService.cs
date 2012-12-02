using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;
using log4net;
using ValueType = NPC.Domain.Models.FlowTypes.FlowValueType;

namespace NPC.FlowEngine
{
    public class FlowEngineService
    {
        private readonly FlowNodeInstanceRepository _flowNodeInstanceRepository;
        private readonly FlowRepository _flowRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        private readonly ILog _logger;
        public FlowEngineService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _flowNodeInstanceRepository = new FlowNodeInstanceRepository();
            _flowRepository = new FlowRepository();
            _taskRepository = new TaskRepository();
            _userRepository = new UserRepository();
        }

        public void CreateFlowNodeInstance()
        {
            var instances = _flowRepository.GetInstanceFlow();
            instances.ToList().ForEach(CreateSingleClinentNodeInstance);
        }

        private void CreateSingleClinentNodeInstance(Flow flow)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var flowNode = flow.FlowType.GetFirstNode();
                //创建节点实例
                var flowNodeInstance = new FlowNodeInstance();
                var task = new Task();
                flowNodeInstance.BelongsFlowNode = flowNode;
                flowNodeInstance.BelongsFlow = flow;
                //HACK:陈春伟,这里需要考虑以后把user抽象成一个接口的形式,不需要依赖具体的实现或直接只关注UserId会更简单
                var executorText = string.Empty;
                if (flowNode.ExecutorType == FlowValueType.ByValue)
                {
                    executorText = flowNode.ExecutorValue;
                }
                if (flowNode.ExecutorType == FlowValueType.ByDataField)
                {
                    executorText = flow.FlowDataFields.Single(o => o.Name == flowNode.ExecutorValue).Value;
                }

                var executorIds = executorText.Split(';');
                //读取用户信息
                var users = _userRepository.GetUsers(executorIds.Select(Guid.Parse).ToArray());
                users.ToList().ForEach(user =>
                {
                    flowNodeInstance.FlowNodeInstanceUserStates.Add(new FlowNodeInstanceUserState { User = user });
                    task.TaskUserStates.Add(new TaskUserState { User = user });
                });
                flowNodeInstance.RecordDescription.CreateBy(flow.RecordDescription.UserOfCreate);
                _flowNodeInstanceRepository.Save(flowNodeInstance);
                flow.FlowStatus = FlowStatus.Start;
                flow.RecordDescription.DateOfLastestModify = DateTime.Now;
                _flowRepository.Save(flow);

                task.Body = flowNodeInstance.Id.ToString();
                task.Description = "流程任务";
                task.GroupName = TaskConst.FlowTaskGroup;
                task.TypeName = TaskConst.FlowTaskType;
                task.OuterId = flowNodeInstance.Id;
                task.TaskProcessUrl = flowNodeInstance.BelongsFlowNode.ProcessUrl;
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

        public void DealFlowNodeFlowTo()
        {
            var flowNodeInstances = _flowNodeInstanceRepository.GetUnDeals();
            flowNodeInstances.ToList().ForEach(DealSingleFlowNodeFlowTo);
        }

        private void DealSingleFlowNodeFlowTo(FlowNodeInstance flowNodeInstance)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var flowNode = flowNodeInstance.BelongsFlowNode;
                foreach (var targetClinetNode in from clientLine in flowNode.FlowNodeLines where clientLine.RuleCode == flowNodeInstance.FlowNodeAction.Name select clientLine.ContactTo)
                {
                    var task = new Task();
                    //创建新节点实例
                    var newClientInstance = new FlowNodeInstance();
                    newClientInstance.BelongsFlow = flowNodeInstance.BelongsFlow;
                    newClientInstance.BelongsFlowNode = targetClinetNode;
                    var userIds = newClientInstance.BelongsFlow.GetActionUsers(targetClinetNode.Id);
                    userIds.ToList().ForEach(o =>
                    {
                        var user = _userRepository.Find(o);
                        task.TaskUserStates.Add(new TaskUserState { User = user });
                        newClientInstance.FlowNodeInstanceUserStates.Add(new FlowNodeInstanceUserState() { User = user });
                    });
                    flowNodeInstance.InstanceStatus = InstanceStatus.Runing;
                    _flowNodeInstanceRepository.Save(newClientInstance);

                    //创建任务
                    task.Title = flowNodeInstance.BelongsFlow.Title;
                    task.Body = newClientInstance.Id.ToString();
                    task.GroupName = TaskConst.FlowTaskGroup;
                    task.TypeName = TaskConst.FlowTaskType;
                    task.OuterId = flowNodeInstance.Id;
                    task.TaskProcessUrl = flowNode.ProcessUrl;

                    _taskRepository.Save(task);
                }
                //设置原节点为完成
                flowNodeInstance.Finished();
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
