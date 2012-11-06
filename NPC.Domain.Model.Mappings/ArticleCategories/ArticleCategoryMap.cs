using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.ArticleCategories;

namespace NPC.Domain.Model.Mappings.ArticleCategories
{
    public class ArticleCategoryMap : ClassMap<ArticleCategory>
    {
        public ArticleCategoryMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.CategoryName);
            Map(o => o.DateOfPublish);
            Map(o => o.Publisher);
            Map(o => o.PublisherId);
            Map(o => o.UintId);
            HasMany(o => o.ChilrenArticleCategories).KeyColumn("ParentArticleCategoryId");
            References(o => o.ParentArticleCategory).Column("ParentArticleCategoryId");
            Table("ArticleCategory");
        }
    }
}
