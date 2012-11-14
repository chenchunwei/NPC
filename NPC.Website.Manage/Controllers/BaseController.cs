using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPC.Website.Manage.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToMessage(string message, string returnUrl = "", string textOfReturnUrl = "")
        {
            return new RedirectResult(Url.Action("Message", "System") + string.Format("?message={0}&returnUrl={1}&textOfReturnUrl={2}", message, returnUrl, textOfReturnUrl));
        }
    }
}