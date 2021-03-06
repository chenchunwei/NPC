﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Articles
{
    public class ArticleEditModel
    {
        public ArticleEditModel()
        {
            FormData = new ArticleEditModelFormData();
            FormData.IsShow = true;
        }
        public Guid? Id { get; set; }
        public ArticleEditModelFormData FormData { get; set; }
    }

    public class ArticleEditModelFormData
    {
        public string Title { get; set; }
        public string UrlOfCoverImage { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsShow { get; set; }
        public int OrderSort { get; set; }
        public Guid? ArticleCategoryId { get; set; }
    }
}
