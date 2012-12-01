using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Model.Mappings.Flows;
using NPC.Domain.Models.ClientNodeInstances;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Model.Mappings.ClientNodeInstances
{
    public sealed class ClientNodeInstanceMap:ClassMap<ClientNodeInstance>
    {
        public ClientNodeInstanceMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            References(o => o.BelongsClientNode).Column("BelongsClientNodeId");
            References(o => o.BelongsFlow).Column("BelongsFlowId");
            Map(o => o.TimeOfFinished);
            HasMany(o => o.ClientNodeInstanceUserState).KeyColumn("ClientNodeInstanceId");
            Component(o => o.RecordDescription);
            Table("ClientNodeInstances");
        }
     }
}
