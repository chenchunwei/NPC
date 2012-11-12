using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.ArticleCategories;
using NPC.Domain.Repository;

namespace NPC.Website.Manage.Controllers
{
    public class ArticleCategoriesController : Controller
    {
        private readonly ArticleCategoryAction _articleCategoryAction; 
        public ArticleCategoriesController()
        {
            _articleCategoryAction = new ArticleCategoryAction();
        }
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetCategories(Guid? id)
        {
            var model = _articleCategoryAction.InitializeArticleCategoryTreeModel(id);
            return new NewtonsoftJsonResult() { Data = model.Components };
        }

        [HttpPost, ActionName("EditCategory")]
        public JsonResult EditCategoryPost(EditArticleCategoryModel model)
        {
            try
            {
                _articleCategoryAction.CreateNewCategory(model);
            }
            catch (Exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure" } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
    }
}
