using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Users
{
    public class EditPasswordModel
    {
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
        public string ReNewPwd { get; set; }
        public User User { get; set; }
    }
}
