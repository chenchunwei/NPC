using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodes
{
    /// <summary>
    /// 节点动作
    /// </summary>
    public class ClientNodeActions
    {
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 所属的流程节点Id
        /// </summary>
        public virtual ClientNode ClientNode { get; set; }
       
    }
}
