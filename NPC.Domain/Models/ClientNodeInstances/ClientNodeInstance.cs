using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.ClientNodeInstances
{
    public class ClientNodeInstance : IAggregateRoot
    {
        public ClientNodeInstance()
        {
            ClientNodeInstanceUserStates = new List<ClientNodeInstanceUserState>();
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual DateTime? TimeOfFinished { get; set; }
        public virtual Flow BelongsFlow { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual ClientNode BelongsClientNode { get; set; }
        public virtual IList<ClientNodeInstanceUserState> ClientNodeInstanceUserStates { get; set; }

        public virtual void Execute(User user, ClientNodeAction action)
        {
            var userState = ClientNodeInstanceUserStates.Single(o => o.User.Id == user.Id);
            userState.ExecuteStatus = ExecuteStatus.Executed;
            userState.ClientNodeAction = action;
            userState.RecordDescription.UpdateBy(user);
        }
    }
}
