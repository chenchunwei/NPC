using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.ArticleCategories
{

    public class ArticleCategoryTreeModel
    {
        public ArticleCategoryTreeModel()
        {
            Components = new List<ArticleCategoryTreeModelComponent>();
        }
        public IList<ArticleCategoryTreeModelComponent> Components { get; set; }
    }

    [DataContract]
    public class ArticleCategoryTreeModelComponent
    {
        public ArticleCategoryTreeModelComponent()
        {
            State = "closed";
            Childrens = new List<ArticleCategoryTreeModelComponent>();
        }

        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "iconCls")]
        public string IconCls { get; set; }
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "text")]
        public string Name { get; set; }
        [DataMember(Name = "children")]
        public IList<ArticleCategoryTreeModelComponent> Childrens { get; set; }
    }
}
