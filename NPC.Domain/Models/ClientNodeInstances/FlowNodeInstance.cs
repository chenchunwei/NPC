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

        public virtual void Execute(User user, string actionName)
        {
            var action = BelongsFlowNode.FlowNodeActions.Single(o => o.Name == actionName);
            var userState = FlowNodeInstanceUserStates.Single(o => o.User.Id == user.Id);
            userState.ExecuteStatus = ExecuteStatus.Executed;
            //HACK:这里的代码限定了一个任务只要有一个人执行过了，就已经是Action状态了
            //还有其它任务的关闭应该谁来完成，引擎来完成的话，那么这个过程中的时间差导致的并发怎么来处理，比如一个同意了，这个应该结束了
            //而实际上任务还没有被结束，这时另外一位处理人否决了操作，这时侯就会造成严重的歧意
            userState.FlowNodeAction = action;
            FlowNodeAction = action;
            InstanceStatus = InstanceStatus.ActionCompleted;
            userState.RecordDescription.UpdateBy(user);
        }

        public virtual void Finished()
        {
            InstanceStatus = InstanceStatus.Finished;
            RecordDescription.DateOfLastestModify = DateTime.Now;
        }
    }
}
