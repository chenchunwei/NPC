using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.ArticleCategories;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Articles
{
    public class Article : IAggregateRoot
    {
        public Article()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual ArticleCategory ArticleCategory { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string UrlOfCoverImage { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual int HitCount { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
