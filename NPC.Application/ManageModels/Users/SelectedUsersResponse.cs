using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.Users
{
    [DataContract]
    public class SelectedUsersResponse
    {
        public SelectedUsersResponse()
        {
            UserViewModels=new List<UserViewModel>();
        }
        [DataMember(Name = "users")]
        public IList<UserViewModel> UserViewModels { get; set; }
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }
    }
}
