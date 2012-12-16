using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Nodes;

namespace NPC.Domain.Model.Mappings.Nodes
{
    public class NodeRecordMarkMap : ComponentMap<NodeRecordMark>
    {
        public NodeRecordMarkMap()
        {
            Map(o => o.FirstContentTip);
            Map(o => o.FirstContentTitle);
            Map(o => o.FirstImageTip);
            Map(o => o.FirstImageTitle);
            Map(o => o.FirstTitleTip);
            Map(o => o.FisrtTitleTitle);
            Map(o => o.IsFirstContentHidden);
            Map(o => o.IsFirstImageHidden);
            Map(o => o.IsFirstTitleHidden);
            Map(o => o.IsRecordLinkHidden);
            Map(o => o.IsSecondContentHidden);
            Map(o => o.IsSecondImageHidden);
            Map(o => o.IsSecondTitleHidden);
            Map(o => o.NodeDescription);
            Map(o => o.RecordLinkTip);
            Map(o => o.RecordLinkTitle);
            Map(o => o.SecondContentTip);
            Map(o => o.SecondContentTitle);
            Map(o => o.SecondImageTitle);
            Map(o => o.SecondImageTip);
            Map(o => o.SecondTitleTip);
            Map(o => o.SecondTitleTitle);

            Map(o => o.IsRecordLinkRequired);
            Map(o => o.IsFirstTitleRequired);
            Map(o => o.IsFirstContentRequired);
            Map(o => o.IsFirstImageRequired);
            Map(o => o.IsSecondContentRequired);
            Map(o => o.IsSecondImageRequired);
            Map(o => o.IsSecondTitleRequired);
        }
    }
}
