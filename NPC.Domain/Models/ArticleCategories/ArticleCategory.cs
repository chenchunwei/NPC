using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;

namespace NPC.Domain.Models.ArticleCategories
{
    public class ArticleCategory : IAggregateRoot
    {
        public virtual ArticleCategory ParentArticleCategory { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual string DateOfPublish { get; set; }
        public virtual string Publisher { get; set; }
        public virtual string PublisherId { get; set; }
        public virtual Guid UintId { get; set; }
        public virtual IList<ArticleCategory> ChilrenArticleCategories { get; set; }
    }
}
