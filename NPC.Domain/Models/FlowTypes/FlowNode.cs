using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Models.FlowTypes
{
    public class FlowNode
    {
        public FlowNode()
        {
            RecordDescription = new RecordDescription();
            FlowNodeActions = new List<FlowNodeAction>();
            FlowNodeLines = new List<FlowNodeLine>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual List<FlowNodeAction> FlowNodeActions { get; set; }
        public virtual List<FlowNodeLine> FlowNodeLines { get; set; }
        public virtual string ProcessUrl { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual bool IsFirstNode { get; set; }
        public virtual FlowValueType ExecutorType { get; set; }
        public virtual bool IsExecutorWithArray { get; set; }
        public virtual string ExecutorValue { get; set; }
        public virtual bool IsServerNode { get; set; }
    }
}
