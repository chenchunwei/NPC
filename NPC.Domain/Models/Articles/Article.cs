using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Articles
{
    public class Article : IAggregateRoot
    {
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual User Publisher { get; set; }
        public virtual DateTime DateOfPublish { get; set; }
        public virtual int HitCount { get; set; }
    }
}
