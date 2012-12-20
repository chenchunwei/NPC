using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using Fluent.Infrastructure.Web.HttpFiles;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Articles;

namespace NPC.Website.Manage.Controllers
{
    public class ArtilcesController : CommonController
    {
        private readonly ArticleAction _articleAction;
        public ArtilcesController()
        {
            _articleAction = new ArticleAction();
        }
        public ActionResult List(ArticleListModel listModel)
        {
            listModel.ArtilceSearchModel.ArticleQueryItem.Pagination.PageIndex = PageIndex;
            listModel.ArtilceSearchModel.ArticleQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _articleAction.InitializeArticleListModel(listModel.ArtilceSearchModel.ArticleQueryItem);
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
            return RedirectToMessage("文章保存成功");
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _articleAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
    }
}
