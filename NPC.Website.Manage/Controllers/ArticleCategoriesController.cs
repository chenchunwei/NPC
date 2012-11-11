using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Domain.Repository;

namespace NPC.Website.Manage.Controllers
{
    public class ArticleCategoriesController : Controller
    {
        private ArticleCategoryAction _articleCategoryAction; 
        public ArticleCategoriesController()
        {
            _articleCategoryAction = new ArticleCategoryAction();
        }
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetUnits(Guid? id)
        {
            var model = _articleCategoryAction.InitializeArticleCategoryTreeModel(id);
            return new NewtonsoftJsonResult() { Data = model.Components };
        }
    }
}
