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
        #region 构造函数
        public FlowNodeInstance()
        {
            FlowNodeInstanceTasks = new List<FlowNodeInstanceTask>();
            RecordDescription = new RecordDescription();
            InstanceStatus = InstanceStatus.Runing;
        }
        #endregion

        #region 节点属性
        /// <summary>
        /// 主键Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 流程完成时间
        /// </summary>
        public virtual DateTime? TimeOfFinished { get; set; }
        /// <summary>
        /// 节点所属流程
        /// </summary>
        public virtual Flow BelongsFlow { get; set; }
        /// <summary>
        /// 节点基本信息
        /// </summary>
        public virtual RecordDescription RecordDescription { get; set; }
        /// <summary>
        /// 所属的流程节点
        /// </summary>
        public virtual FlowNode BelongsFlowNode { get; set; }
        /// <summary>
        /// 节点附属的任务
        /// </summary>
        public virtual IList<FlowNodeInstanceTask> FlowNodeInstanceTasks { get; set; }
        /// <summary>
        /// 节点实例的状态
        /// </summary>
        public virtual InstanceStatus InstanceStatus { get; set; }
        /// <summary>
        /// 节点执行的Action
        /// </summary>
        public virtual FlowNodeAction FlowNodeAction { get; set; }
        #endregion

        #region 获取节点的执行人id
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
        #endregion

        #region 服务端节点执行(仅用于服务器节点)
        /// <summary>
        /// 服务端节点执行(用于服务器节点)
        /// </summary>
        public virtual void Execute()
        {
            //HACK:将任务节点设置完成应该是根据规则来,不应该直接就设置成完成,考虑到投票机制
            if (!BelongsFlowNode.IsServerNode)
                throw new ApplicationException("非服务器节点，请执行Execute(string actionName, User user)");
            TriggerActionCompletedRule();
        }
        #endregion

        #region 客户端节点执行
        //HACK:服务端节点和客户端节点可以用多态来处理
        /// <summary>
        /// 客户端节点执行
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="user"></param>
        public virtual void Execute(string actionName, User user)
        {
            var action = BelongsFlowNode.FlowNodeActions.Single(o => o.Name == actionName);
            FlowNodeAction = action;
            InstanceStatus = InstanceStatus.ActionCompleted;

            var userState = FlowNodeInstanceTasks.Single(o => o.UserId == user.Id);
            userState.TaskStatus = TaskStatus.Executed;
            //HACK:这里的代码限定了一个任务只要有一个人执行过了，就已经是Action状态了
            //还有其它任务的关闭应该谁来完成，引擎来完成的话，那么这个过程中的时间差导致的并发怎么来处理，比如一个同意了，这个应该结束了
            //而实际上任务还没有被结束，这时另外一位处理人否决了操作，这时侯就会造成严重的歧意
            userState.FlowNodeAction = action;
            userState.RecordDescription.UpdateBy(user);
            TriggerActionCompletedRule();
        }
        #endregion

        #region 检查完成规则
        public virtual bool TriggerActionCompletedRule()
        {
            if (!FlowNodeInstanceTasks.Any())
            {
                ActionCompleted();
            }
            //Hack:暂时只支持一票制,只要有一个人执行了,就作为节点值
            var completedTask = FlowNodeInstanceTasks.Where(task => task.TaskStatus == TaskStatus.Executed);
            if (!completedTask.Any())
                return false;
            FlowNodeAction = completedTask.First().FlowNodeAction;
            ActionCompleted();
            return true;
        }
        #endregion

        #region 设置为Actioned
        public virtual void ActionCompleted()
        {
            InstanceStatus = InstanceStatus.ActionCompleted;
            RecordDescription.DateOfLastestModify = DateTime.Now;
            //将所有任务设置成忽略
            FlowNodeInstanceTasks.ToList().ForEach(task =>
            {
                if (task.TaskStatus != TaskStatus.Executed)
                    task.TaskStatus = TaskStatus.Ignore;
            });
        }
        #endregion

        #region 设置节点完成状态
        public virtual void Finished()
        {
            InstanceStatus = InstanceStatus.Finished;
            RecordDescription.DateOfLastestModify = DateTime.Now;
            //将所有任务设置成忽略
            FlowNodeInstanceTasks.ToList().ForEach(task =>
            {
                if (task.TaskStatus != TaskStatus.Executed)
                    task.TaskStatus = TaskStatus.Ignore;
            });
        }
        #endregion

        #region 设置节点完成状态
        public virtual void Ignore()
        {
            InstanceStatus = InstanceStatus.Ignore;
            RecordDescription.DateOfLastestModify = DateTime.Now;
            //将所有任务设置成忽略
            FlowNodeInstanceTasks.ToList().ForEach(task =>
            {
                if (task.TaskStatus != TaskStatus.Executed)
                    task.TaskStatus = TaskStatus.Ignore;
            });
        }
        #endregion

        #region 获取节点执行完成后的下一个节点的类型
        /// <summary>
        /// 获取节点执行完成后的下一个节点的类型
        /// </summary>
        /// <returns></returns>
        public virtual FlowNode GetNextNodeTypeWhenActioned()
        {
            FlowNode returnFlowNode = null;
            if (BelongsFlowNode.IsServerNode)
            {
                //HACK:服务器节点暂时都使用流程变量作为节点的流转条件，根据RuleCode中的值配置DataFields的值，该值必须为boolean,逻辑需要确认
                //值，以此来判断流程走向
                //判断所有的ActionLine,执行第一个符合条件的line
                var isAnyMatched = false;
                BelongsFlowNode.FlowNodeLines.ToList().ForEach(line =>
                {
                    var datas = BelongsFlow.FlowDataFields.Where(dataField => dataField.Name == line.RuleCode);
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
            if (InstanceStatus == InstanceStatus.Runing)
                throw new ApplicationException("节点暂未执行完毕，无法确定下一节点的路径，待节点完成后再获取信息");
            var mathcedLine = BelongsFlowNode.FlowNodeLines.First(line => line.RuleCode == FlowNodeAction.Name);
            if (mathcedLine == null)
                throw new ArgumentException(string.Format("客户端节点执行时未找到Action对应的下一节点值，节点id={0}", Id));
            return BelongsFlowNode.FlowNodeLines.First(line => line.RuleCode == FlowNodeAction.Name).ContactTo;
        }
        #endregion

        #region 创建任务
        /// <summary>
        /// 创建任务 并返回新增的任务
        /// </summary>
        /// <returns></returns>
        public virtual IList<FlowNodeInstanceTask> BuilderTasksAndReturnNewTasks()
        {
            //HACK:这些动作不应该被包含在一个另一个聚合根中,应该边界清晰的放在一个系统对象中以便解耦
            var newTasks = new List<FlowNodeInstanceTask>();
            var userIds = GetNodeActionUserIds();
            userIds.ToList().ForEach(userId =>
            {
                if (FlowNodeInstanceTasks.Any(o => o.UserId == userId))
                    return;
                var newTask = new FlowNodeInstanceTask()
                {
                    FlowNodeInstance = this,
                    TaskStatus = TaskStatus.Created,
                    UserId = userId
                };
                newTasks.Add(newTask);
                FlowNodeInstanceTasks.Add(newTask);
            });
            return newTasks;
        }
        #endregion
    }
}
