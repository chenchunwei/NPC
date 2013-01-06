using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Nodes;

namespace NPC.Domain.Model.Mappings.Nodes
{
    public class NodeMap : ClassMap<Node>
    {
        public NodeMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Code);
            Map(o => o.IsCanDelete);
            Map(o => o.IsRecordNode);
            Map(o => o.Name);
            Map(o => o.OuterCategoryId);
            References(o => o.ParentNode).Column("ParentNodeId");
            HasMany(o => o.Childrens).KeyColumn("ParentNodeId").Where("IsDelete=0");
            HasMany(o => o.NodeRecords).KeyColumn("BelongsToNodeId").Where("IsDelete=0");
            References(o => o.Unit).Column("UnitId");
            Component(o => o.RecordDescription);
            Component(o => o.NodeRecordMark);
            Table("Nodes");
        }
    }
}
