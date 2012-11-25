using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowNodes;

namespace NPC.Domain.Model.Mappings.FlowNodes
{
    public class ClientNodeMap : ClassMap<ClientNode>
    {
        public ClientNodeMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.ProcessUrl);
            HasMany(o => o.ClientNodeActions).KeyColumn("ClientNodeId");
            HasMany(o => o.ClientNodeLines).KeyColumn("ClientNodeId");
            Component(o => o.RecordDescription);
            Table("ClientNodes");
        }
    }
}
