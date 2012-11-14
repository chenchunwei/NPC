using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPC.Website.Manage.ActionFilterAttributes
{
    public class RoleFliterAttribute : ActionFilterAttribute
    {
        private readonly List<string> _roles;
        public RoleFliterAttribute(params string[] roles)
        {
            _roles = new List<string>();
            if (roles != null && roles.Any())
                _roles.AddRange(roles);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}