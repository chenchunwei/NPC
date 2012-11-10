using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.Departments
{

    public class DepartmentTreeModel
    {
        public DepartmentTreeModel()
        {
            Components=new List<DepartmentTreeModelComponent>();
        }
        public IList<DepartmentTreeModelComponent> Components { get; set; }
    }

    [DataContract]
    public class DepartmentTreeModelComponent
    {
        public DepartmentTreeModelComponent()
        {
            State = "closed";
            Childrens = new List<DepartmentTreeModelComponent>();
        }

        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "iconCls")]
        public string IconCls { get; set; }
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "children")]
        public IList<DepartmentTreeModelComponent> Childrens { get; set; }
    }
}
