using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NPC.Application.ManageModels.ArticleCategories;
using NPC.Domain.Models.Nodes;

namespace NPC.Application.ManageModels.Nodes
{

    public class NodeTreeModel
    {
        public NodeTreeModel()
        {
            Components = new List<NodeTreeModelComponent>();
        }
        public IList<NodeTreeModelComponent> Components { get; set; }
    }

    [DataContract]
    public class NodeTreeModelComponent
    {
        public NodeTreeModelComponent()
        {
            State = "closed";
            Childrens = new List<NodeTreeModelComponent>();
        }

        [DataMember(Name = "iconCls")]
        public string IconCls { get; set; }
        [DataMember(Name = "text")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "type")]
        public int NodeType { get; set; }
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "categoryId")]
        public Guid? CategoryId { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "categoryName")]
        public string CategoryName { get; set; }
        [DataMember(Name = "children")]
        public IList<NodeTreeModelComponent> Childrens = new List<NodeTreeModelComponent>();
        [DataMember(Name = "nodeRecordMark")]
        public NodeRecordMark NodeRecordMark { get; set; }
        [DataMember(Name = "orderSort")]
        public int OrderSort { get; set; }
    }
}
