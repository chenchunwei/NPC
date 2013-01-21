using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Users
{
    public class UserQueryItem : QueryItemBase
    {
        public UserQueryItem()
        {
            Pagination = new Pagination();
        }

        public Guid? UnitId { get; set; }
        public Guid? DepartmentLikeId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
}
