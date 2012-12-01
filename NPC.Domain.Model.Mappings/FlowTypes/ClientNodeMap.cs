using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public sealed class ClientNodeMap : ClassMap<ClientNode>
    {
        public ClientNodeMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.ProcessUrl);
            HasMany(o => o.ClientNodeActions).KeyColumn("ClientNodeId").Cascade.All();
            HasMany(o => o.ClientNodeLines).KeyColumn("ClientNodeId").Cascade.All();
            Component(o => o.RecordDescription);
            Map(o => o.IsFirstNode);
            Table("ClientNodes");
        }
    }
}
