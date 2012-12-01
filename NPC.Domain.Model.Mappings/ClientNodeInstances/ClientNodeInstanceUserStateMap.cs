using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.ClientNodeInstances;

namespace NPC.Domain.Model.Mappings.ClientNodeInstances
{
    public class ClientNodeInstanceUserStateMap : ClassMap<ClientNodeInstanceUserState>
    {
        public ClientNodeInstanceUserStateMap()
        {
            Id(o => o.Id).GeneratedBy.Assigned();
            Map(o => o.ExecuteStatus).CustomType<ExecuteStatus>();
            References(o => o.User).Column("UserId");
            References(o => o.ClientNodeAction).Column("ClientNodeActionId");
            Component(o => o.RecordDescription);
            Table("ClientNodeInstanceUserStates");
        }
    }
}
                               