using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Roles;

namespace NPC.Application.ManageModels.Roles
{
    public class EditRoleModel
    {
        public Guid? Id { get; set; }
        public Role Role { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public Guid UnitId { get; set; }
    }
}
