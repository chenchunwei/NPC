using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Privileges;

namespace NPC.Application.ManageModels.Privileges
{
    public class PrivilegeListModel
    {
        public PrivilegeListModel()
        {
            Privileges=new List<Privilege>();
        }
        public IList<Privilege> Privileges { get; set; }
    }
}
