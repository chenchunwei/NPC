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
            Map(o => o.UrlOfCoverImage);
            Map(o => o.Title);
            Map(o => o.Content);
            Map(o => o.HitCount);
            Component(o => o.RecordDescription);
            References(o => o.ArticleCategory).Column("ArticleCategoryId");
            References(o => o.Unit).Column("UnitId");
            Table("Articles");
        }
    }
}
