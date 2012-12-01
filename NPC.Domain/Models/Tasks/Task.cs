using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Tasks
{
    /// <summary>
    /// 任务对象
    /// </summary>
    public class Task : IAggregateRoot
    {
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 分组名
        /// </summary>
        public virtual string GroupName { get; set; }
        /// <summary>
        /// 类型名
        /// </summary>
        public virtual string TypeName { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 任务描述 
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 任务处理人
        /// </summary>
        public virtual IList<User> TaskProcessers { get; set; }
        /// <summary>
        /// 与任务关联的外部对象id
        /// </summary>
        public virtual string OuterId { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public virtual TaskStatus TaskStatus { get; set; }
        /// <summary>
        /// 记录创建及修改的基本信息
        /// </summary>
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
