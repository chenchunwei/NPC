using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Nodes;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.NodeRecords
{
    public class NodeRecord : IAggregateRoot
    {
        public NodeRecord()
        {
            RecordDescription = new RecordDescription();
        }

        public virtual Unit Unit { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string FirstTitle { get; set; }
        public virtual string SecondTitle { get; set; }
        public virtual string FirstContent { get; set; }
        public virtual string SecondContent { get; set; }
        public virtual string FirstImage { get; set; }
        public virtual string SecondImage { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual Node BelongsToNode { get; set; }
        public virtual bool IsShow { get; set; }
        public string RecordLink { get; set; }
        public DateTime? StartTimeOfShow { get; set; }
        /// <summary>
        /// 最后显示时间
        /// </summary>
        public DateTime? EndOfShowTime { get; set; }
    }
}
