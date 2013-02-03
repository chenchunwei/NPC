using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using Fluent.Infrastructure.Log.Messages;
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
        private readonly FlowNodeInstanceTaskRepository _flowNodeInstanceTaskRepository;
        private readonly ILog _logger;
        public FlowEngineService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _flowNodeInstanceRepository = new FlowNodeInstanceRepository();
            _flowRepository = new FlowRepository();
            _flowNodeInstanceTaskRepository = new FlowNodeInstanceTaskRepository();
        }

        public void CreateFlowNodeInstance()
        {
            var messageContainer = new MessageContainer("CreateFlowNodeInstance");
            var instances = _flowRepository.GetInstanceFlow();
            messageContainer.Debug("共需处理CreateFlowNodeInstance的记录条数:{0}", instances.Count);
            instances.ToList().ForEach(instance => CreateSingleFlowNodeInstance(instance, messageContainer));
            messageContainer.Log4Net();
        }

        public void DealFlowNodeFlowTo()
        {
            var messageContainer = new MessageContainer("DealFlowNodeFlowTo");
            var flowNodeInstances = _flowNodeInstanceRepository.GetUnDeals();
            messageContainer.Debug("共需处理DealFlowNodeFlowTo的记录条数:{0}", flowNodeInstances.Count);
            flowNodeInstances.ToList().ForEach(flowNodeInstance => DealSingleFlowNodeFlowTo(flowNodeInstance, messageContainer));
            messageContainer.Log4Net();
        }

        public void DealFlow()
        {
            var messageContainer = new MessageContainer("DealFlow");
            var flows = _flowRepository.GetUnFinisheds();
            messageContainer.Debug("共需处理DealFlow的记录条数:{0}", flows.Count);
            flows.ToList().ForEach(flow => DealSingleFlowNode(flow, messageContainer));
            messageContainer.Log4Net();
        }

        private void DealSingleFlowNode(Flow flow, MessageContainer messageContainer)
        {
            if (!flow.IsCompleted())
                return;
            flow.Finished();
            _flowRepository.Save(flow);
        }

        private void CreateSingleFlowNodeInstance(Flow flow, MessageContainer messageContainer)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var flowNode = flow.FlowType.GetFirstNode();
                if (flowNode == null)
                    throw new ApplicationException("流程不存在任务节点,流程 id=" + flow.Id);
                if (flowNode.IsServerNode)
                    DealServerNodeLoop(flow, flowNode, messageContainer);
                else
                    DealClientNode(flow, flowNode, messageContainer);

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

        private void DealSingleFlowNodeFlowTo(FlowNodeInstance flowNodeInstance, MessageContainer messageContainer)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                //判断流程对象是否ActionCompleted
                if (!flowNodeInstance.TriggerActionCompletedRule())
                    return;
                var nextNode = flowNodeInstance.GetNextNodeTypeWhenActioned();
                flowNodeInstance.Finished();
                flowNodeInstance.FlowNodeInstanceTasks.ToList().ForEach(task =>
                {
                    if (task.RecordDescription.IsUpdated)
                    {
                        _flowNodeInstanceTaskRepository.Save(task);
                    }
                });
                _flowNodeInstanceRepository.Save(flowNodeInstance);
                //如果不存在下一个节点表示流程完成
                if (nextNode == null)
                {
                    Finished(flowNodeInstance.BelongsFlow);
                    trans.Commit();
                    return;
                }

                if (nextNode.IsServerNode)
                    DealServerNodeLoop(flowNodeInstance.BelongsFlow, nextNode, messageContainer);
                else
                    DealClientNode(flowNodeInstance.BelongsFlow, nextNode, messageContainer);

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

        private void DealServerNodeLoop(Flow flow, FlowNode targetFlowNode, MessageContainer messageContainer)
        {
            var nodes = new Stack<FlowNodeInstance>();
            var node = DealServerNode(flow, targetFlowNode, messageContainer);
            if (node == null)
            {
                Finished(flow);
                return;
            }
            nodes.Push(node);
            while (nodes.Any())
            {
                //这里循环处理，主要是为了使服务器节点无延迟的即时处理，因为服务器节点仅依赖流程变量与客户无关
                //而客户节点肯定是依赖于客户的操作来处理
                var tempFlowNodeInstance = nodes.Pop();
                if (!tempFlowNodeInstance.BelongsFlowNode.IsServerNode)
                {
                    DealClientNode(flow, tempFlowNodeInstance.GetNextNodeTypeWhenActioned(), messageContainer);
                    return;
                }
                var returnNodeInstanace = DealServerNode(flow, tempFlowNodeInstance.GetNextNodeTypeWhenActioned(), messageContainer);
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

        private FlowNodeInstance DealServerNode(Flow flow, FlowNode targetFlowNode, MessageContainer messageContainer)
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

        private FlowNodeInstance DealClientNode(Flow flow, FlowNode targetFlowNode, MessageContainer messageContainer)
        {
            //创建新节点实例
            var newFlowInstance = new FlowNodeInstance();
            newFlowInstance.BelongsFlow = flow;
            newFlowInstance.BelongsFlowNode = targetFlowNode;
            var newTasks = newFlowInstance.BuilderTasksAndReturnNewTasks();
            _flowNodeInstanceRepository.Save(newFlowInstance);
            newTasks.ToList().ForEach(task => _flowNodeInstanceTaskRepository.Save(task));
            return newFlowInstance;
        }
    }
}
