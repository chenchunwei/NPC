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
            instances.ToList().ForEach(CreateSingleFlowNodeInstance);
        }

        private void CreateSingleFlowNodeInstance(Flow flow)
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
            //处理结点的下一个节点信息
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var targetFlowNode = flowNodeInstance.GetNextNodeTypeWhenActioned();
                //设置原节点为完成
                flowNodeInstance.Finished();
                _flowNodeInstanceRepository.Save(flowNodeInstance);

                //当下一个节点信息不存时，流程结束
                if (targetFlowNode == null)
                {
                    Finished(flowNodeInstance.BelongsFlow);
                    trans.Commit();
                    return;
                }
                #region 服务端节点处理
                //当下一结点是服务器节点时,无需创建任务，直接标记成已完成进入下一个结点
                if (targetFlowNode.IsServerNode)
                {
                    var nodes = new Stack<FlowNodeInstance>();
                    nodes.Push(flowNodeInstance);
                    while (nodes.Any())
                    {
                        var tempFlowNodeInstance = nodes.Pop();
                        if (!tempFlowNodeInstance.BelongsFlowNode.IsServerNode)
                        {
                            DealClientNode(tempFlowNodeInstance);
                        }
                        var returnNodeInstanace = DealServerNode(tempFlowNodeInstance);
                        //服务端与客户端的区别，服务端的处理完之后就已经Finished了，所以直接接着处理下一个节点，
                        //如果是客户端则需要生成客户端任务，服务端则继续执行服务端的处理
                        if (returnNodeInstanace.GetNextNodeTypeWhenActioned() == null)
                        {
                            Finished(flowNodeInstance.BelongsFlow);
                            trans.Commit();
                            return;
                        }
                        nodes.Push(returnNodeInstanace);
                    }
                    trans.Commit();
                    return;
                }
                #endregion

                //客户节点处理
                DealClientNode(flowNodeInstance);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        private void Finished(Flow flow)
        {
            flow.Finished();
            _flowRepository.Save(flow);
        }

        private FlowNodeInstance DealServerNode(FlowNodeInstance flowNodeInstance)
        { //处理结点的下一个节点信息
            var flowNode = flowNodeInstance.BelongsFlowNode;
            var targetFlowNode = flowNodeInstance.GetNextNodeTypeWhenActioned();
            //生成节点实例
            var newFlowInstanceOfServer = new FlowNodeInstance();
            newFlowInstanceOfServer.BelongsFlow = flowNodeInstance.BelongsFlow;
            newFlowInstanceOfServer.BelongsFlowNode = targetFlowNode;
            newFlowInstanceOfServer.TimeOfFinished = DateTime.Now;
            newFlowInstanceOfServer.Execute();
            newFlowInstanceOfServer.InstanceStatus = InstanceStatus.Finished;
            _flowNodeInstanceRepository.Save(newFlowInstanceOfServer);
            return newFlowInstanceOfServer;
        }

        private FlowNodeInstance DealClientNode(FlowNodeInstance flowNodeInstance)
        {
            var targetFlowNode = flowNodeInstance.GetNextNodeTypeWhenActioned();

            var task = new Task();
            //创建新节点实例
            var newFlowInstance = new FlowNodeInstance();
            newFlowInstance.BelongsFlow = flowNodeInstance.BelongsFlow;
            newFlowInstance.BelongsFlowNode = targetFlowNode;
            var userIds = newFlowInstance.GetNodeActionUserIds();
            userIds.ToList().ForEach(o =>
            {
                var user = _userRepository.Find(o);
                task.TaskUserStates.Add(new TaskUserState { User = user });
                newFlowInstance.FlowNodeInstanceUserStates.Add(new FlowNodeInstanceUserState() { User = user });
            });
            flowNodeInstance.InstanceStatus = InstanceStatus.Runing;
            _flowNodeInstanceRepository.Save(newFlowInstance);

            //创建任务
            task.Title = flowNodeInstance.BelongsFlow.Title;
            task.Body = newFlowInstance.Id.ToString();
            task.GroupName = TaskConst.FlowTaskGroup;
            task.TypeName = TaskConst.FlowTaskType;
            task.OuterId = flowNodeInstance.Id;
            task.TaskProcessUrl = targetFlowNode.ProcessUrl;
            _taskRepository.Save(task);
            return newFlowInstance;
        }
    }
}
