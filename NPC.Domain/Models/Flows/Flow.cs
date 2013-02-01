using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Flows
{
    public class Flow : IAggregateRoot
    {
        public Flow()
        {
            RecordDescription = new RecordDescription();
            FlowDataFields = new List<FlowDataField>();
            FlowNodeInstances = new List<FlowNodeInstance>();
            FlowHistories = new List<FlowHistory>();
        }

        #region 属性
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 流程标是
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 流程管理员
        /// </summary>
        public virtual User UserOfFlowAdmin { get; set; }
        /// <summary>
        /// 流程对应的类型
        /// </summary>
        public virtual FlowType FlowType { get; set; }
        /// <summary>
        /// 流程状态 
        /// </summary>
        public virtual FlowStatus FlowStatus { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public virtual DateTime? DateTimeofFinished { get; set; }
        /// <summary>
        /// 基本信息
        /// </summary>
        public virtual RecordDescription RecordDescription { get; set; }
        /// <summary>
        /// 流程变量
        /// </summary>
        public virtual IList<FlowDataField> FlowDataFields { get; set; }
        /// <summary>
        /// 所程节点实例 
        /// </summary>
        public virtual IList<FlowNodeInstance> FlowNodeInstances { get; set; }
        /// <summary>
        /// 流程 操作历史记录
        /// </summary>
        public virtual IList<FlowHistory> FlowHistories { get; set; }
        #endregion

        #region 流程完成
        public virtual void Finished()
        {
            //设置所有流程实例为忽略状态
            FlowNodeInstances.ToList().ForEach(nodeInstance =>
            {
                if (!nodeInstance.TriggerActionCompletedRule())
                {
                    nodeInstance.Ignore();
                }
            });
            FlowStatus = FlowStatus.Finished;
            RecordDescription.DateOfLastestModify = DateTime.Now;
            DateTimeofFinished = DateTime.Now;
        }
        #endregion

        public virtual bool IsCompleted()
        {
            return FlowNodeInstances.ToList().All(nodeInstance => nodeInstance.InstanceStatus == InstanceStatus.Finished);
        }

        #region 跳转到某个结点
        public virtual FlowNodeInstance Goto(string nodeName, Dictionary<string, string> args = null)
        {
            //HACK:这种跳转动作因为涉及到多个聚合根，所以不适合放在单独某个聚合根中，这里这样做是因为大家配置了双向关联，实现比较方便，但是这里违背了聚合的边界
            //会造成持久化上的一些困镜。除非抛弃持久化时聚合的边界
            var targetNode = FlowType.FlowNodes.First(o => o.Name.ToLower() == nodeName.ToLower());
            if (targetNode == null)
                throw new ArgumentException("要跳转的目标节点不存在");
            //合并流程变量
            WriteDataFields(args);
            //把当前执行的节点设置成忽略状态并创建新的流程实例
            FlowNodeInstances.ToList().ForEach(nodeInstance =>
            {
                if (!nodeInstance.TriggerActionCompletedRule())
                {
                    nodeInstance.Ignore();
                }
            });
            var newInstance = new FlowNodeInstance();
            newInstance.BelongsFlow = this;
            newInstance.BelongsFlowNode = targetNode;
            newInstance.RecordDescription.CreateBy(null);
            newInstance.BuilderTasksAndReturnNewTasks();
            FlowNodeInstances.Add(newInstance);
            return newInstance;
        }
        #endregion

        #region 写入流程变量
        public virtual void WriteDataFields(Dictionary<string, string> args)
        {
            if (args == null)
                return;
            args.ToList().ForEach(pair =>
            {
                var dataField = FlowDataFields.SingleOrDefault(o => o.Name == pair.Key);
                if (dataField != null)
                {
                    dataField.Value = pair.Value;
                    return;
                }
                FlowDataFields.Add(new FlowDataField()
               {
                   Value = pair.Value,
                   Name = pair.Key
               });
            });
        }
        #endregion

        #region 流程执行流转
        /// <summary>
        /// 执行流程，让流程继续流转
        /// </summary>
        public FlowNodeInstance ExecuteFlowTo()
        {
            //HACK:这个方法本来不应该存在的。因为这里需要流程节点实例，及节点任务来共同决定的，涉及到三个聚合根，不能在某一个聚合 中来完成
            //HACK:以及这里只考虑到单节点激活，不考虑并行节点等因素,后期需要改造支持并行节点以及自由节点多点触发的问题(国强提到过的多点触发，可以解决并行节点不能实现自由节点间排列组合的问题)
            //但自由组合带来的新问题是节点会可能没有约束的向着多个方法流动，无法控制流程的正常合并与结束
            var instancesInActioned = FlowNodeInstances.Single(o => o.InstanceStatus == InstanceStatus.ActionCompleted || o.BelongsFlowNode.IsServerNode && o.InstanceStatus != InstanceStatus.Finished);
            if (instancesInActioned.TriggerActionCompletedRule())
            {
                instancesInActioned.Finished();
                var next = instancesInActioned.GetNextNodeTypeWhenActioned();
                var newInstance = new FlowNodeInstance();
                newInstance.BelongsFlow = this;
                newInstance.BelongsFlowNode = next;
                newInstance.RecordDescription.CreateBy(null);
                FlowNodeInstances.Add(newInstance);
                if (next.IsServerNode)
                {
                    //HACK:存在堆栈溢出的风险，当然机率非常小，除非流程中海量的服务节点
                    return ExecuteFlowTo();
                }
                return newInstance;
            }
            return null;
        }
        #endregion
    }
}
