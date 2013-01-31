using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public class FlowNodeInstanceTaskQueryItem : QueryItemBase
    {
        /// <summary>
        /// 任务所属的流程名
        /// </summary>
        public string FlowTypeName { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 节点名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 任务状态 
        /// </summary>
        public TaskStatus? TaskStatus { get; set; }
    }
}
