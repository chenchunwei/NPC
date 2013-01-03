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
            model.FormData.Author = article.Author;
            model.FormData.IsShow = article.IsShow;
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
            article.Author = model.FormData.Author;
            article.IsShow = model.FormData.IsShow;
            if (!string.IsNullOrEmpty(model.FormData.UrlOfCoverImage))
                article.UrlOfCoverImage = model.FormData.UrlOfCoverImage;
            article.RecordDescription.CreateBy(NpcContext.CurrentUser);
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
            article.Author = model.FormData.Author;
            article.IsShow = model.FormData.IsShow;
            if (!string.IsNullOrEmpty(model.FormData.UrlOfCoverImage))
                article.UrlOfCoverImage = model.FormData.UrlOfCoverImage;
            article.RecordDescription.UpdateBy(NpcContext.CurrentUser);
            _articleRepository.Save(article);
            model.Id = article.Id;
        }

        public ArticleListModel InitializeArticleListModel(ArticleQueryItem queryItem)
        {
            var model = new ArticleListModel();
            queryItem.UnitId = NpcContext.CurrentUser.Unit.Id;
            model.ArtilceSearchModel.ArticleQueryItem = queryItem;
            model.Articles = _articleRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _articleRepository.Find(id);
            target.RecordDescription.Delete();
            _articleRepository.SaveOrUpdate(target);
        }
    }
}
