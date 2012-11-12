using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPC.Website.Manage.Controllers
{
    public class ArtilcesController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult EditArticle()
        {
            return View();
        }
        [HttpPost, ActionName("EditArticle")]
        public ActionResult EditArticlePost()
        {
            return View();
        }

    }
}
