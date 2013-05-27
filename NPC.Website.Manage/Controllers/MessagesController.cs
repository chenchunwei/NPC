using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Messages;
using NPC.Application.Contexts;
using Fluent.Infrastructure.Mvc;

namespace NPC.Website.Manage.Controllers
{
    public class MessagesController : CommonController
    {
        private readonly MessageAction _messageAction;
        public MessagesController()
        {
            _messageAction = new MessageAction();
        }
        public ActionResult List(MessageListModel listModel)
        {
            listModel.MessageSearchModel.MessageQueryItem.Pagination.PageIndex = PageIndex;
            listModel.MessageSearchModel.MessageQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _messageAction.InitializeMessageListModel(listModel.MessageSearchModel.MessageQueryItem);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _messageAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
    }
}
