using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Articles;
using NPC.Query.Articles;

namespace NPC.Application.MianModels.Homes
{
    public class ListModel
    {
        public ListModel()
        {
            ArticleQueryItem=new ArticleQueryItem();
        }
        public ArticleQueryItem ArticleQueryItem { get; set; }
        public List<Article> Articles { get; set; }
        public string ListTitle { get; set; }
    }
}
