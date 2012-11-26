using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Agents;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Model.Mappings.Agents
{
    public class AgentMap : ClassMap<Agent>
    {
        public AgentMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.EndTime);
            Map(o => o.StartTime);
            Component(o => o.RecordDescription);
            References(o => o.ActualUser).Column("ActualUserId");
            References(o => o.UserOfAgent).Column("UserIdOfAgent");
            Table("Agents");
        }
     }
}
