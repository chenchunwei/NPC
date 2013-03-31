using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.RoleUsers;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Users
{
    public class UserListModel
    {
        public UserListModel()
        {
            Users = new List<User>();
            UserSearchModel=new UserSearchModel();
            RoleUsers=new List<RoleUser>();
        }
        public UserSearchModel UserSearchModel { get; set; }
        public IList<User> Users { get; set; }
        public IList<RoleUser> RoleUsers { get; set; }
        public RoleUser GetRoleUser(User user)
        {
            return RoleUsers.FirstOrDefault(o => o.UserId == user.Id);
        }
    }
}
