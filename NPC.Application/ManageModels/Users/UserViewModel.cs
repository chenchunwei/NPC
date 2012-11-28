using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.Users
{
    [DataContract]
    public class UserViewModel
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
