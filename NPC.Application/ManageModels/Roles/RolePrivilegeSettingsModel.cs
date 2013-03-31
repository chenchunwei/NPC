using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Privileges;
using Fluent.Permission.Roles;

namespace NPC.Application.ManageModels.Roles
{
    public class RolePrivilegeSettingsModel
    {
        public RolePrivilegeSettingsModel()
        {
            Privileges=new List<Privilege>();
            SelectedPrivileges=new List<Guid>();
        }

        public Guid Id { get; set; }
        public Role Role { get; set; }
        public IList<Privilege> Privileges { get; set; }
        public IList<Guid> SelectedPrivileges { get; set; }
    }
}
