using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application.Contexts;
using NPC.Domain.Models.Units;
using NPC.Domain.Repository;

namespace NPC.Website.Town.Main.Controllers
{
    public class BaseController : Controller
    {
        protected Guid UnitId;
        private const string KeyOfUnitId = "UnitId";

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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var unit = System.Web.HttpContext.Current.Items[NpcMainWebContext.KeyOfUnit] as Unit;
            if (unit != null)
                return;
            var mappingUnitId = UnitMapping.UnitId;
            if (mappingUnitId == null)
            {
                if (filterContext.ActionDescriptor.ActionName == "Error")
                    return;
                var bCookie = filterContext.HttpContext.Request.Cookies[KeyOfUnitId] != null && Guid.TryParse(filterContext.HttpContext.Request.Cookies[KeyOfUnitId].Value, out UnitId);
                var bRequest = Guid.TryParse(filterContext.HttpContext.Request[KeyOfUnitId], out UnitId);
                if (!bRequest && !bCookie)
                {
                    Guid.TryParse(System.Configuration.ConfigurationManager.AppSettings["DefaultUnitId"], out UnitId);
                 }
                var cookie = new HttpCookie(KeyOfUnitId, UnitId.ToString());
                cookie.HttpOnly = true;
                filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                UnitId = mappingUnitId.Value;
            }

            var unitRepository = new UnitRepository();
            unit = unitRepository.Find(UnitId);
            System.Web.HttpContext.Current.Items[NpcMainWebContext.KeyOfUnit] = unit;
            base.OnActionExecuting(filterContext);
        }
    }
}