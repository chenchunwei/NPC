﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Articles;

namespace NPC.Application.ManageModels.Articles
{
    public class ArtilceSearchModel
    {
        public ArtilceSearchModel()
        {
            ArticleQueryItem = new ArticleQueryItem();
        }
        public ArticleQueryItem ArticleQueryItem { get; set; }
    }
}
