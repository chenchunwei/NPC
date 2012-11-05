using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;

namespace NPC.Domain.Models.ArticleCategories
{
    public class ArticleCategory : IAggregateRoot
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string DateOfPublish { get; set; }
        public string Publisher { get; set; }
        public string PublisherId { get; set; }
    }
}
