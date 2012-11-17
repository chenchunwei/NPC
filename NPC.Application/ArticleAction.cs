using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Articles;
using NPC.Domain.Models.Articles;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class ArticleAction : BaseAction
    {
        private readonly ArticleRepository _articleRepository;
        private readonly ArticleCategoryRepository _articleCategoryRepository;
        public ArticleAction()
        {
            _articleRepository = new ArticleRepository();
            _articleCategoryRepository = new ArticleCategoryRepository();
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
            if (!model.FormData.ArticleCategoryId.HasValue)
                throw new ArgumentException("model.FormData.ArticleCategoryId不能为空");
            var article = new Article();
            article.ArticleCategory = _articleCategoryRepository.Find(model.FormData.ArticleCategoryId.Value);
            article.Content = model.FormData.Content;
            article.HitCount = 0;
            article.RecordDescription.UserOfCreate = NpcContext.CurrentUser;
            article.Title = model.FormData.Title;
            article.Unit = NpcContext.CurrentUser.Unit;
            article.UrlOfCoverImage = model.FormData.UrlOfCoverImage;
            _articleRepository.Save(article);
        }


        public void UpdateArticle(ArticleEditModel model)
        {
            if (!model.FormData.ArticleCategoryId.HasValue)
                throw new ArgumentException("model.FormData.ArticleCategoryId不能为空");
            if (!model.Id.HasValue)
                throw new ArgumentException("model.Id不能为空");
            var article = _articleRepository.Find(model.Id.Value);
            article.ArticleCategory = _articleCategoryRepository.Find(model.FormData.ArticleCategoryId.Value);
            article.Content = model.FormData.Content;
            article.HitCount = 0;
            article.RecordDescription.UserOfCreate = NpcContext.CurrentUser;
            article.Title = model.FormData.Title;
            article.Unit = NpcContext.CurrentUser.Unit;
            article.UrlOfCoverImage = model.FormData.UrlOfCoverImage;
            _articleRepository.Save(article);
            model.Id = article.Id;
        }
    }
}
