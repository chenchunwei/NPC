using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Domain.Models.NotifyMessages;

namespace NPC.Website.Manage.Controllers
{
    public class NotifyMessagesController : CommonController
    {
        private readonly NotifyMessageAction _notifyMessageAction;
        public NotifyMessagesController()
        {
            _notifyMessageAction=new NotifyMessageAction();
        }
        public ActionResult List(NotifyMessageQueryItem queryItem)
        {
            queryItem.Pagination.PageIndex = PageIndex;
            var model = _notifyMessageAction.InitializeNpcMmsListModel(queryItem);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _notifyMessageAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
    }
}
