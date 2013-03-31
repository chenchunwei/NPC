using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Privileges;

namespace NPC.Application.ManageModels.Privileges
{
    public class EditPrivilegeModel
    {
        public Guid? Id { get; set; }
        public Privilege Privilege { get; set; }
        public string PrivilegeName { get; set; }
        public string PrivilegeCode { get; set; }
        public Guid UnitId { get; set; }
        public string PrivilegeDescription { get; set; }
    }
}
