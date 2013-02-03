using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Common
{
    public class RecordDescription
    {
        public RecordDescription()
        {
            DateOfCreate = DateTime.Now;
        }
        public virtual DateTime DateOfCreate { get; set; }
        public virtual DateTime? DateOfLastestModify { get; set; }
        public virtual User UserOfLasetestModify { get; set; }
        public virtual User UserOfCreate { get; set; }
        public virtual bool IsDelete { get; set; }
        /// <summary>
        /// 标记是否有更新,用于判断是否需要持久化
        /// </summary>
        public virtual bool IsUpdated { get; set; }
        public virtual bool IsCreated { get; set; }
        public virtual void UpdateBy(User operatorUser)
        {
            DateOfLastestModify = DateTime.Now;
            IsUpdated = true;
            UserOfLasetestModify = operatorUser;
        }
        public virtual void CreateBy(User operatorUser)
        {
            IsCreated = true;
            DateOfCreate = DateTime.Now;
            UserOfCreate = operatorUser;
        }
        public virtual void Delete()
        {
            IsUpdated = true;
            IsDelete = true;
            DateOfLastestModify = DateTime.Now;
        }
    }
}
