using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Users
{
    public class UserListModel
    {
        public UserListModel()
        {
            Users = new List<User>();
            UserSearchModel=new UserSearchModel();
        }
        public UserSearchModel UserSearchModel { get; set; }
        public IList<User> Users { get; set; }
    }
}
