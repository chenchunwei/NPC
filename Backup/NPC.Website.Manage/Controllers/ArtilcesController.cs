using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Web.HttpFiles;
using NPC.Application;
using NPC.Application.ManageModels.Articles;

namespace NPC.Website.Manage.Controllers
{
    public class ArtilcesController : BaseController
    {
        private readonly ArticleAction _articleAction;
        public ArtilcesController()
        {
            _articleAction = new ArticleAction();
        }
        public ActionResult List(ArtilceSearchModel searchModel)
        {
            searchModel.ArticleQueryItem.Pagination.PageIndex = PageIndex;
            var model = _articleAction.InitializeArticleListModel(searchModel.ArticleQueryItem);
            return View(model);
        }

        public ActionResult EditArticle(Guid? id)
        {
            var model = _articleAction.InitializeArticleEditModel(id);
            return View(model);
        }

        [HttpPost, ActionName("EditArticle")]
        public ActionResult EditArticlePost(ArticleEditModel model)
        {
            var fileHelper = new FileHelper();
            fileHelper.Upload();
            model.FormData.UrlOfCoverImage = string.Join(";", fileHelper.GetFileInfosByKey("FormData.CoverImg").ToArray().Select(o => o.ServerFileName));
            if (model.Id.HasValue)
                _articleAction.UpdateArticle(model);
            else
                _articleAction.NewArticle(model);
            return View(model);
        }
    }
}
