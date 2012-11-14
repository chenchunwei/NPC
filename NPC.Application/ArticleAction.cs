using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.Articles;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class ArticleAction
    {
        private readonly ArticleRepository _articleRepository;
        public ArticleAction()
        {
            _articleRepository = new ArticleRepository();
        }
        public ArticleEditModel InitializeArticleEditModel(Guid? articleId)
        {
            var model = new ArticleEditModel();
            if (!articleId.HasValue)
                return model;
            var article = _articleRepository.Find(articleId.Value);
            model.FormData.ArticleCategoryId = article.ArticleCategory.Id;
            model.FormData.Content = article.Content;
            model.FormData.Title = article.Title;
            model.FormData.UrlOfCoverImage = article.UrlOfCoverImage;
            return model;
        }

        public void NewArticle(ArticleEditModel model)
        {
            
        }


        public void UpdateArticle(ArticleEditModel model)
        {

        }
    }
}
