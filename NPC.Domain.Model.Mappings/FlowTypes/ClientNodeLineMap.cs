using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public class ClientNodeLineMap : ClassMap<ClientNodeLine>
    {
        public ClientNodeLineMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.RuleCode);
            References(o => o.ContactTo).Column("ClientNodeIdOfContactTo");
            Component(o => o.RecordDescription);
            Table("ClientNodeLines");
        }
    }
}
