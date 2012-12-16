using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using Fluent.Infrastructure.Web.HttpFiles;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.NodeRecords;
using Saturday.Application;

namespace NPC.Website.Manage.Controllers
{
    public class NodeRecordsController : CommonController
    {
        private readonly NodeRecordAction _nodeRecordAction;
        public NodeRecordsController()
        {
            _nodeRecordAction = new NodeRecordAction();
        }

        public ActionResult EditNodeRecord(Guid? nodeId, Guid? id)
        {
            var model = _nodeRecordAction.InitializeEditNodeRecordModel(nodeId, id);
            return View(model);
        }
        [HttpPost, ActionName("EditNodeRecord")]
        public ActionResult EditNodeRecordPost(EditNodeRecordModel model)
        {
            var fileHelper = new FileHelper();
            fileHelper.Upload();
            model.FormData.FirstImage = string.Join(";", fileHelper.GetFileInfosByKey("FormData.FirstImage").ToArray().Select(o => o.ServerFileName));
            model.FormData.SecondImage = string.Join(";", fileHelper.GetFileInfosByKey("FormData.SecondImage").ToArray().Select(o => o.ServerFileName));
            if (model.Id.HasValue)
                _nodeRecordAction.UpdateNodeRecord(model);
            else
                _nodeRecordAction.NewNodeRecord(model);
            return RedirectToMessage("节点内容保存成功");
        }

        public ActionResult List(NodeRecordListModel listModel)
        {
            listModel.NodeRecordSearchModel.NodeRecordQueryItem.Pagination.PageIndex = PageIndex;
            listModel.NodeRecordSearchModel.NodeRecordQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _nodeRecordAction.InitializeNodeRecordListModel(listModel.NodeRecordSearchModel.NodeRecordQueryItem);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _nodeRecordAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }

        [HttpPost, ActionName("InitializeForm")]
        public ActionResult InitializeForm(Guid nodeId, Guid? id)
        {
            var model = _nodeRecordAction.InitializeEditNodeRecordModel(nodeId, id);
            return PartialView("_EditNodeRecordForm", model);
        }
    }
}
