using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.NpcMmses;
using Newtonsoft.Json;

namespace NPC.Website.Manage.Controllers
{
    public class NpcMmsesController : CommonController
    {
        private NpcMmsAction _npcMmsAction;
        public NpcMmsesController()
        {
            _npcMmsAction = new NpcMmsAction();
        }
        public ActionResult EditNpcMms()
        {
            return View();
        }
        [HttpPost, ActionName("EditNpcMms")]
        public ActionResult EditNpcMmsPost(EditNpcMmsModel model)
        {
            model.FrameSerializers = JsonConvert.DeserializeObject<IList<FrameSerializer>>(model.FormData.Frames);
            if (model.FormData.NpcMmsDraftId == null)
            {
                _npcMmsAction.NewNpcMms(model);
                return RedirectToMessage("手机报彩信保存成功");
            }
            return View();
        }

        public ActionResult List(NpcMmsListModel listModel)
        {
            listModel.NpcMmsSearchModel.NpcMmsQueryItem.Pagination.PageIndex = PageIndex;
            listModel.NpcMmsSearchModel.NpcMmsQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _npcMmsAction.InitializeNpcMmsListModel(listModel.NpcMmsSearchModel.NpcMmsQueryItem );
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
