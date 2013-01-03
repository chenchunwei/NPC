using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models;

namespace NPC.Domain.Models.Articles
{
    public class ArticleQueryItem : QueryItemBase
    {
        public ArticleQueryItem()
        {
            Pagination = new Pagination();
        }
        public bool? IsShow { get; set; }
        public string Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? UnitId { get; set; }
    }
}
