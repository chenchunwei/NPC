using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;

namespace NPC.Domain.Models.FlowNodes
{
    public class ClientNode
    {
        public ClientNode()
        {
            RecordDescription=new RecordDescription();
            ClientNodeActions=new List<ClientNodeAction>();
            ClientNodeLines = new List<ClientNodeLine>();
         }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<ClientNodeAction> ClientNodeActions { get; set; }
        public virtual IList<ClientNodeLine> ClientNodeLines { get; set; }
        public virtual string ProcessUrl { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
