using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.Users
{
    public class SelectUserOptionsModel
    {
        public SelectUserOptionsModel()
        {
            SelectUserOptionsRows = new List<SelectUserOptionsComponent>();
        }
        public IList<SelectUserOptionsComponent> SelectUserOptionsRows { get; set; }
    }
    [DataContract]
    public class SelectUserOptionsComponent
    {
        public SelectUserOptionsComponent()
        {
            Checkbox = true;
            State = "closed";
            Childrens = new List<SelectUserOptionsComponent>();
        }

        [DataMember(Name = "iconCls")]
        public string IconCls { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "position")]
        public string Position { get; set; }
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "nodeType")]
        public int NodeType { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "checkbox")]
        public bool Checkbox { get; set; }
        [DataMember(Name = "children")]
        public IList<SelectUserOptionsComponent> Childrens = new List<SelectUserOptionsComponent>();
    }
}
