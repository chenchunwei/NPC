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
        public virtual void UpdateBy(User operatorUser)
        {
            DateOfLastestModify = DateTime.Now;
            UserOfLasetestModify = operatorUser;
        }
        public virtual void CreateBy(User operatorUser)
        {
            UserOfLasetestModify = operatorUser;
        }
    }
}
