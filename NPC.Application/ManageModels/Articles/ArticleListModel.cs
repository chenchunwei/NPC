using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Articles;

namespace NPC.Application.ManageModels.Articles
{
    public class ArticleListModel
    {
        public ArticleListModel()
        {
            Articles=new List<Article>();
            ArtilceSearchModel=new ArtilceSearchModel();
        }
        public IList<Article> Articles { get; set; }
        public ArtilceSearchModel ArtilceSearchModel { get; set; }
    }
}
