using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;

namespace NPC.Domain.Models.Tasks
{
    /// <summary>
    /// 任务对象
    /// </summary>
    public class Task : IAggregateRoot
    {
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 任务标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 任务描述 
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 任务创建日期
        /// </summary>
        public virtual string DateOfCreate { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        public virtual Guid UserIdOfCreate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid UserIdOfSendTo { get; set; }
        /// <summary>
        /// 与任务关联的外部对象id
        /// </summary>
        public virtual string OuterId { get; set; }

        public virtual TaskStatus TaskStatus { get; set; }
    }
}
