using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.NpcMmses;
using NPC.Domain.Models.NpcMmses;
using Newtonsoft.Json;

namespace NPC.Website.Manage.Controllers
{
    public class NpcMmsesController : CommonController
    {
        private readonly NpcMmsAction _npcMmsAction;
        public NpcMmsesController()
        {
            _npcMmsAction = new NpcMmsAction();
        }
        public ActionResult EditNpcMms(Guid? id)
        {
            var model = _npcMmsAction.InitializeEditNpcMmsModel(id);
            return View(model);
        }
        [HttpPost, ActionName("EditNpcMms")]
        public ActionResult EditNpcMmsPost(EditNpcMmsModel model)
        {
            model.FrameSerializers = JsonConvert.DeserializeObject<IList<FrameSerializer>>(model.FormData.Frames);
            var npcMms = model.Id.HasValue ? _npcMmsAction.UpdateNpcMms(model) : _npcMmsAction.NewNpcMms(model);
            if (model.IsSend)
                return RedirectToAction("EditNpcMmsSend", "NpcMmsSends", new { npcMmsId = npcMms.Id });
            return RedirectToMessage("手机报彩信保存成功");
        }

        public ActionResult List(NpcMmsListModel listModel)
        {
            listModel.NpcMmsSearchModel.NpcMmsQueryItem.Pagination.PageIndex = PageIndex;
            listModel.NpcMmsSearchModel.NpcMmsQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _npcMmsAction.InitializeNpcMmsListModel(listModel.NpcMmsSearchModel.NpcMmsQueryItem);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _npcMmsAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
    }
}
