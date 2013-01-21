using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Users
{
    public class UserSearchModel
    {
        public UserSearchModel()
        {
            UserQueryItem=new UserQueryItem();
        }
        public UserQueryItem UserQueryItem { get; set; }
    }
}
