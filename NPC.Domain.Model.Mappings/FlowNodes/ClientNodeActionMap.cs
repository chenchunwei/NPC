using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowNodes;

namespace NPC.Domain.Model.Mappings.FlowNodes
{
    public class ClientNodeActionMap : ClassMap<ClientNodeAction>
    {
        public ClientNodeActionMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Table("ClientNodeActions");
        }
    }
}
