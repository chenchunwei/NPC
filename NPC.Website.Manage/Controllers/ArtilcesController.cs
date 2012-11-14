using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Articles;

namespace NPC.Website.Manage.Controllers
{
    public class ArtilcesController : Controller
    {
        private readonly ArticleAction _articleAction;
        public ArtilcesController()
        {
            _articleAction = new ArticleAction();
        }
        public ActionResult List()
        {
            return View();
        }

        public ActionResult EditArticle(Guid? id)
        {
            var model = _articleAction.InitializeArticleEditModel(id);
            return View(model);
        }
        [HttpPost, ActionName("EditArticle")]
        public ActionResult EditArticlePost(ArticleEditModel model)
        {
            if (model.Id.HasValue)
            {
                _articleAction.UpdateArticle(model);
            }
            else
            {
                _articleAction.NewArticle(model);
                
            }
            return View();
        }

    }
}
