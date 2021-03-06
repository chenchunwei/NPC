﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.NpcMmsSends;

namespace NPC.Website.Manage.Controllers
{
    public class NpcMmsSendsController : CommonController
    {
        private readonly NpcMmsSendAction _npcMmsSendAction;
        public NpcMmsSendsController()
        {
            _npcMmsSendAction = new NpcMmsSendAction();
        }

        public ActionResult List(NpcMmsSendListModel listModel)
        {
            listModel.NpcMmsSendSearchModel.NpcMmsSendQueryItem.Pagination.PageIndex = PageIndex;
            listModel.NpcMmsSendSearchModel.NpcMmsSendQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _npcMmsSendAction.InitializeNpcMmsSendListModel(listModel.NpcMmsSendSearchModel.NpcMmsSendQueryItem);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _npcMmsSendAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }

        public ActionResult EditNpcMmsSend(Guid npcMmsId)
        {
            var model = _npcMmsSendAction.InitializeEditNpcMmsSendModel(npcMmsId);
            return View(model);
        }
        [HttpPost, ActionName("EditNpcMmsSend")]

        public ActionResult EditNpcMmsSendPost(EditNpcMmsSendModel model)
        {
            model.Receivers = model.ReceiversStr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            _npcMmsSendAction.Send(model);
            return RedirectToMessage("发送成功！");
        }
    }
}
