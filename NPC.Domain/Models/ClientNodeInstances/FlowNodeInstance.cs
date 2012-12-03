using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public class FlowNodeInstance : IAggregateRoot
    {
        public FlowNodeInstance()
        {
            FlowNodeInstanceUserStates = new List<FlowNodeInstanceUserState>();
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual DateTime? TimeOfFinished { get; set; }
        public virtual Flow BelongsFlow { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual FlowNode BelongsFlowNode { get; set; }
        public virtual IList<FlowNodeInstanceUserState> FlowNodeInstanceUserStates { get; set; }
        public virtual InstanceStatus InstanceStatus { get; set; }
        public virtual FlowNodeAction FlowNodeAction { get; set; }
        /// <summary>
        /// 获取节点的执行人id
        /// </summary>
        /// <returns></returns>
        public virtual IList<Guid> GetNodeActionUserIds()
        {
            var executorText = string.Empty;
            if (BelongsFlowNode.ExecutorType == FlowValueType.ByValue)
            {
                executorText = BelongsFlowNode.ExecutorValue;
            }
            if (BelongsFlowNode.ExecutorType == FlowValueType.ByDataField)
            {
                executorText = BelongsFlow.FlowDataFields.Single(o => o.Name == BelongsFlowNode.ExecutorValue).Value;
            }
            return executorText.Split(';').Select(Guid.Parse).ToList();
        }
        /// <summary>
        /// 服务端节点执行
        /// </summary>
        public virtual void Execute()
        {
            if (!BelongsFlowNode.IsServerNode) return;
            InstanceStatus = InstanceStatus.ActionCompleted;
            RecordDescription.DateOfLastestModify = DateTime.Now;
        }
        //HACK:服务端节点和客户端节点可以用多态来处理
        public virtual void Execute(string actionName, User user)
        {
            var action = BelongsFlowNode.FlowNodeActions.Single(o => o.Name == actionName);
            FlowNodeAction = action;
            InstanceStatus = InstanceStatus.ActionCompleted;

            var userState = FlowNodeInstanceUserStates.Single(o => o.User.Id == user.Id);
            userState.ExecuteStatus = ExecuteStatus.Executed;
            //HACK:这里的代码限定了一个任务只要有一个人执行过了，就已经是Action状态了
            //还有其它任务的关闭应该谁来完成，引擎来完成的话，那么这个过程中的时间差导致的并发怎么来处理，比如一个同意了，这个应该结束了
            //而实际上任务还没有被结束，这时另外一位处理人否决了操作，这时侯就会造成严重的歧意
            userState.FlowNodeAction = action;
            userState.RecordDescription.UpdateBy(user);
        }

        public virtual void Finished()
        {
            InstanceStatus = InstanceStatus.Finished;
            RecordDescription.DateOfLastestModify = DateTime.Now;
        }
        /// <summary>
        /// 获取节点执行完成后的下一个节点的类型
        /// </summary>
        /// <returns></returns>
        public virtual FlowNode GetNextNodeTypeWhenActioned()
        {
            if (InstanceStatus == InstanceStatus.Runing)
                throw new ApplicationException("节点暂未执行完毕，无法确定下一节点的路径，待节点完成后再获取信息");
            FlowNode returnFlowNode = null;
            if (BelongsFlowNode.IsServerNode)
            {
                //HACK:服务器节点暂时都使用流程变量作为节点的流转条件，根据RuleCode中的值配置DataFields的值，该值必须为boolean
                //值，以此来判断流程走向
                //判断所有的ActionLine,执行第一个符合条件的line
                var isAnyMatched = false;
                BelongsFlowNode.FlowNodeLines.ToList().ForEach(line =>
                {
                    var datas = BelongsFlow.FlowDataFields.Where(
                          dataField => dataField.Name == line.RuleCode);
                    if (datas.Any())
                    {
                        bool isMathced;
                        if (bool.TryParse(datas.First().Value, out isMathced))
                        {
                            isAnyMatched = true;
                            returnFlowNode = line.ContactTo;
                        }
                    }
                });

                if (isAnyMatched)
                    throw new ArgumentException(string.Format("服务器节点执行时未到对应的RuleCode声明的流程变量值，节点id={0}", Id));
                return returnFlowNode;
            }
            var mathcedLine = BelongsFlowNode.FlowNodeLines.First(line => line.RuleCode == FlowNodeAction.Name);
            if (mathcedLine == null)
                throw new ArgumentException(string.Format("客户端节点执行时未找到Action对应的下一节点值，节点id={0}", Id));
            return BelongsFlowNode.FlowNodeLines.First(line => line.RuleCode == FlowNodeAction.Name).ContactTo;
        }
    }
}
