using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application.Contexts;
using NPC.Domain.Repository;

namespace NPC.Website.Main.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            var unitRepository = new UnitRepository();
            Guid unitId;
            var unitIdInParam = System.Web.HttpContext.Current.Request.QueryString["unitId"];
            if (string.IsNullOrEmpty(unitIdInParam))
            {
                var unitIdStr = System.Configuration.ConfigurationManager.AppSettings["DefaultUnitId"];
                unitId = Guid.Parse(unitIdStr);
            }
            else
            {
                unitId = Guid.Parse(unitIdInParam);
            }
            var unit = unitRepository.Find(unitId);
            System.Web.HttpContext.Current.Items[NpcMainWebContext.KeyOfUnit] = unit;
        }
        protected int PageIndex
        {
            get
            {
                int pageIndex;
                if (!string.IsNullOrEmpty(Request["pageIndex"]) && int.TryParse(Request["pageIndex"], out pageIndex))
                    return pageIndex;
                return 1;
            }
        }

        public ActionResult RedirectToMessage(string message, string returnUrl = "", string textOfReturnUrl = "")
        {
            return new RedirectResult(Url.Action("Message", "Home") + string.Format("?message={0}&returnUrl={1}&textOfReturnUrl={2}", message, returnUrl, textOfReturnUrl));
        }
    }
}