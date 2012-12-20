using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.Nodes
{
    public class Node : IAggregateRoot
    {
        public Node()
        {
            RecordDescription = new RecordDescription();
            Childrens=new List<Node>();
            NodeRecords=new List<NodeRecord>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsCanDelete { get; set; }
        public virtual bool IsRecordNode { get; set; }
        public virtual string Code { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual Guid? OuterCategoryId { get; set; }
        public virtual Node ParentNode { get; set; }
        public virtual IList<Node> Childrens { get; set; }
        public virtual IList<NodeRecord> NodeRecords { get; set; }
        public virtual NodeRecordMark NodeRecordMark { get; set; }
        public virtual Unit Unit { get; set; }
    }

    public class NodeRecordMark
    {
        public string NodeDescription { get; set; }

        public bool IsRecordLinkHidden { get; set; }
        public string RecordLinkTitle { get; set; }
        public string RecordLinkTip { get; set; }
        public bool IsRecordLinkRequired { get; set; }

        public bool IsFirstTitleHidden { get; set; }
        public string FisrtTitleTitle { get; set; }
        public string FirstTitleTip { get; set; }
        public bool IsFirstTitleRequired { get; set; }

        public bool IsSecondTitleHidden { get; set; }
        public string SecondTitleTip { get; set; }
        public string SecondTitleTitle { get; set; }
        public bool IsSecondTitleRequired { get; set; }

        public bool IsFirstContentHidden { get; set; }
        public string FirstContentTitle { get; set; }
        public string FirstContentTip { get; set; }
        public bool IsFirstContentRequired { get; set; }

        public bool IsSecondContentHidden { get; set; }
        public string SecondContentTip { get; set; }
        public string SecondContentTitle { get; set; }
        public bool IsSecondContentRequired { get; set; }

        public bool IsFirstImageHidden { get; set; }
        public string FirstImageTitle { get; set; }
        public string FirstImageTip { get; set; }
        public bool IsFirstImageRequired { get; set; }

        public bool IsSecondImageHidden { get; set; }
        public string SecondImageTip { get; set; }
        public string SecondImageTitle { get; set; }
        public bool IsSecondImageRequired { get; set; }
    }
}
