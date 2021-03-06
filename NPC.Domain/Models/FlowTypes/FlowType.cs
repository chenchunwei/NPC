﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.FlowTypes
{
    public class FlowType : IAggregateRoot
    {
        public FlowType()
        {
            FlowNodes = new List<FlowNode>();
            FlowTypeDataFields = new List<FlowTypeDataField>();
            RecordDescription = new RecordDescription();
        }
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
        public virtual IList<FlowTypeDataField> FlowTypeDataFields { get; set; }
        /// <summary>
        /// 流程节点
        /// </summary>
        public virtual IList<FlowNode> FlowNodes { get; set; }
        /// <summary>
        /// 记录描述信息
        /// </summary>
        public virtual RecordDescription RecordDescription { get; set; }
        /// <summary>
        /// 获取第一个节点
        /// </summary>
        /// <returns></returns>
        public virtual FlowNode GetFirstNode()
        {
            if(!FlowNodes.Any())
                throw  new ArgumentException("流程没有任何节点");
            return FlowNodes.FirstOrDefault(o => o.IsFirstNode);
        }
    }
}
