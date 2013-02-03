using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.Units
{

    public class UnitTreeModel
    {
        public UnitTreeModel()
        {
            Components=new List<UnitTreeModelComponent>();
        }
        public IList<UnitTreeModelComponent> Components { get; set; }
    }

    [DataContract]
    public class UnitTreeModelComponent
    {
        public UnitTreeModelComponent()
        {
            State = "closed";
            Childrens = new List<UnitTreeModelComponent>();
        }

        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "iconCls")]
        public string IconCls { get; set; }
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "isWebUnit")]
        public bool IsWebUnit { get; set; }
        [DataMember(Name = "isFlowUnit")]
        public bool IsFlowUnit { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "children")]
        public IList<UnitTreeModelComponent> Childrens { get; set; }
    }
}
