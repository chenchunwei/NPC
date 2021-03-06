﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.ArticleCategories
{
    public class ArticleCategory : IAggregateRoot
    {
        public ArticleCategory()
        {
            RecordDescription=new RecordDescription();
            ChilrenArticleCategories=new List<ArticleCategory>();
        }

        public virtual Guid Id { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual IList<ArticleCategory> ChilrenArticleCategories { get; set; }
        public virtual ArticleCategory ParentArticleCategory { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual string Path
        {
            get
            {
                var path = string.Empty;
                var nodes = new Stack<ArticleCategory>();
                nodes.Push(this);
                while (nodes.Any())
                {
                    var o = nodes.Pop();
                    path = string.Format("{0};{1}", o.Id, path);
                    if (o.ParentArticleCategory != null)
                        nodes.Push(o.ParentArticleCategory);
                }
                return path.TrimEnd(';');
            }
            set { return; }
        }
    }
}
