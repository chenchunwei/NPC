using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Roles;

namespace NPC.Application.ManageModels.Roles
{
    public class RoleListModel
    {
        public RoleListModel()
        {
            Roles = new List<Role>();
        }
        public IList<Role> Roles { get; set; }
    }
}
