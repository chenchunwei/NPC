﻿using System;
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
            Map(o => o.Path);
            References(o => o.Unit).Column("UnitId");
            HasMany(o => o.ChilrenArticleCategories).KeyColumn("ParentArticleCategoryId");
            References(o => o.ParentArticleCategory).Column("ParentArticleCategoryId");
            Component(o => o.RecordDescription);
            Table("ArticleCategories");
        }
    }
}
