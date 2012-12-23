using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Domain.Model.Mappings.NodeRecords
{
    public class NodeRecordMap : ClassMap<NodeRecord>
    {
       public NodeRecordMap()
       {
           Id(o => o.Id).GeneratedBy.GuidComb();
           Map(o => o.FirstContent).CustomType("StringClob").CustomSqlType("varchar(max)"); ;
           Map(o => o.FirstImage);
           Map(o => o.FirstTitle);
           Map(o => o.SecondContent).CustomType("StringClob").CustomSqlType("varchar(max)"); ;
           Map(o => o.SecondImage);
           Map(o => o.SecondTitle);
           Map(o => o.IsShow);
           Map(o => o.RecordLink);
           References(o => o.BelongsToNode).Column("BelongsToNodeId");
           Component(o => o.RecordDescription);
           Table("NodeRecords");
       }
    }
}
