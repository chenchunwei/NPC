﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Model.Mappings.FlowTypes
{
    public class FlowDataFieldMap : ClassMap<FlowDataField>
    {
        public FlowDataFieldMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Table("FlowDataFields");
        }
    }
}
