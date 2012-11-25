using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.FlowTypes
{
    public class FlowType : IAggregateRoot
    {
        /// <summary>
        /// 流程 id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public virtual string UrlOfDetail { get; set; }
        /// <summary>
        /// 流程变量
        /// </summary>
        public virtual IList<FlowDataField> FlowDataFields { get; set; }
        /// <summary>
        /// 流程节点
        /// </summary>
        public virtual IList<ClientNode> ClientNodes { get; set; }
        /// <summary>
        /// 记录描述信息
        /// </summary>
        public virtual RecordDescription RecordDescription { get; set; }

    }
}
