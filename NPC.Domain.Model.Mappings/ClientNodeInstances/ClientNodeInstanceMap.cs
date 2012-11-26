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
    public class ClientNodeInstanceMap:ClassMap<ClientNodeInstance>
    {
        public ClientNodeInstanceMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            References(o => o.BelongsClientNode).Column("BelongsClientNodeId");
            References(o => o.BelongsFlow).Column("BelongsFlowId");
            Component(o => o.RecordDescription);
            Map(o => o.TimeOfFinished);
            HasManyToMany(o => o.Users)
                .ChildKeyColumn("UserId")
                .ParentKeyColumn("ClientNodeId")
                .Table("ClientNodeInstanceUsers");
            Table("ClientNodeInstances");
        }
     }
}
