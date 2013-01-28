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
                if (flowNode == null)
                    throw new ApplicationException("流程不存在任务节点,流程 id=" + flow.Id);
                if (flowNode.IsServerNode)
                    DealServerNodeLoop(flow, flowNode);
                else
                    DealClientNode(flow, flowNode);

                flow.FlowStatus = FlowStatus.Start;
                flow.RecordDescription.DateOfLastestModify = DateTime.Now;
                _flowRepository.Save(flow);

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
                var targetFlowNode = flowNodeInstance.GetNextNodeTypeWhenActioned();

                flowNodeInstance.Finished();
                _flowNodeInstanceRepository.Save(flowNodeInstance);

                if (targetFlowNode == null)
                {
                    Finished(flowNodeInstance.BelongsFlow);
                    trans.Commit();
                    return;
                }

                if (targetFlowNode.IsServerNode)
                    DealServerNodeLoop(flowNodeInstance.BelongsFlow, targetFlowNode);
                else
                {
                    var newInstance = DealClientNode(flowNodeInstance.BelongsFlow, targetFlowNode);
                }

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

        private FlowNodeInstance DealServerNode(Flow flow, FlowNode targetFlowNode)
        {
            if (targetFlowNode == null)
                return null;
            //生成服务端节点实例
            var newFlowInstanceOfServer = new FlowNodeInstance();
            newFlowInstanceOfServer.BelongsFlow = flow;
            newFlowInstanceOfServer.BelongsFlowNode = targetFlowNode;
            newFlowInstanceOfServer.TimeOfFinished = DateTime.Now;
            newFlowInstanceOfServer.Execute();
            newFlowInstanceOfServer.InstanceStatus = InstanceStatus.Finished;
            _flowNodeInstanceRepository.Save(newFlowInstanceOfServer);
            return newFlowInstanceOfServer;
        }

        private FlowNodeInstance DealClientNode(Flow flow, FlowNode targetFlowNode)
        {
            //创建新节点实例
            var newFlowInstance = new FlowNodeInstance();
            newFlowInstance.BelongsFlow = flow;
            newFlowInstance.BelongsFlowNode = targetFlowNode;
            var userIds = newFlowInstance.GetNodeActionUserIds();
            userIds.ToList().ForEach(o =>
            {
                var user = _userRepository.Find(o);

                newFlowInstance.FlowNodeInstanceUserStates.Add(new FlowNodeInstanceUserState() { User = user });
            });
            _flowNodeInstanceRepository.Save(newFlowInstance);
            CreateTask(newFlowInstance);
            return newFlowInstance;
        }

        private void CreateTask(FlowNodeInstance flowNodeInstance)
        {
            //创建任务
            var task = new Task();
            var userIds = flowNodeInstance.GetNodeActionUserIds();
            userIds.ToList().ForEach(o =>
            {
                var user = _userRepository.Find(o);
                task.TaskUserStates.Add(new TaskUserState { User = user });

            });
            task.Title = flowNodeInstance.BelongsFlow.Title;
            task.Description = flowNodeInstance.BelongsFlow.Title;
            task.Body = flowNodeInstance.Id.ToString();
            task.GroupName = TaskConst.FlowTaskGroup;
            task.TypeName = TaskConst.FlowTaskType;
            task.OuterId = flowNodeInstance.Id;
            task.TaskProcessUrl = flowNodeInstance.BelongsFlowNode.ProcessUrl;
            _taskRepository.Save(task);
        }

        private void DealServerNodeLoop(Flow flow, FlowNode targetFlowNode)
        {
            var nodes = new Stack<FlowNodeInstance>();
            nodes.Push(DealServerNode(flow, targetFlowNode));
            while (nodes.Any())
            {
                //这里循环处理，主要是为了使服务器节点无延迟的即时处理，因为服务器节点仅依赖流程变量与客户无关
                //而客户节点肯定是依赖于客户的操作来处理
                var tempFlowNodeInstance = nodes.Pop();
                if (!tempFlowNodeInstance.BelongsFlowNode.IsServerNode)
                {
                    DealClientNode(flow, tempFlowNodeInstance.GetNextNodeTypeWhenActioned());
                    return;
                }
                var returnNodeInstanace = DealServerNode(flow, tempFlowNodeInstance.GetNextNodeTypeWhenActioned());
                //服务端与客户端的区别，服务端的处理完之后就已经Finished了，所以直接接着处理下一个节点，
                //如果是客户端则需要生成客户端任务，服务端则继续执行服务端的处理
                if (returnNodeInstanace == null)
                {
                    Finished(flow);
                    return;
                }
                nodes.Push(returnNodeInstanace);
            }
        }
    }
}
