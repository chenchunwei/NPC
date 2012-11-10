using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.ArticleCategories;
using NPC.Domain.Models.Articles;

namespace NPC.Domain.Model.Mappings.Articles
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Content);
            Map(o => o.DateOfPublish);
            Map(o => o.HitCount);
            References(o => o.Publisher).Column("PublisherId");
            Map(o => o.Title);
            
            Table("ArticleCategory");
        }
    }
}
