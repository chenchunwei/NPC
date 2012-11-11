using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.ArticleCategories;

namespace NPC.Domain.Repository
{
    public class ArticleCategoryRepository : AbstractNhibernateRepository<Guid, ArticleCategory>
    {
        public IEnumerable<ArticleCategory> GetRoot(Guid unitId)
        {
            return Session.CreateQuery("from ArticleCategory Where Unit.Id=:unitId and ParentArticleCategory is Null And RecordDescription.IsDelete=0")
              .SetGuid("unitId", unitId).List<ArticleCategory>();
        }

        public IEnumerable<ArticleCategory> GetSubs(Guid unitId, Guid id)
        {
            return Session.CreateQuery("from ArticleCategory Where Unit.Id=:unitId And RecordDescription.IsDelete=0 and ParentArticleCategory.Id=:id")
                 .SetGuid("id", id).SetGuid("unitId", unitId).List<ArticleCategory>();
        }

    }
}
