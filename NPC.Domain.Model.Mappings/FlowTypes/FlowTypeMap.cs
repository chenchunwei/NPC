using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public class FlowTypeMap : ClassMap<FlowType>
    {
        public FlowTypeMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.UrlOfDetail);
            Map(o => o.Description);
            Component(o => o.RecordDescription);
            Table("FlowTypes");
        }
    }
}
