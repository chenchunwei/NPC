using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Nodes;
using NPC.Domain.Models.Nodes;

namespace NPC.Website.Manage.Controllers
{
    public class NodesController : CommonController
    {
        private readonly NodeAction _nodeAction;
        public NodesController()
        {
            _nodeAction = new NodeAction();
        }
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetNodes(Guid? id)
        {
            var model = _nodeAction.InitializeNodeTreeModel(id);
            return new NewtonsoftJsonResult() { Data = model.Components };
        }

        [HttpPost, ActionName("EditNode")]
        public JsonResult EditNodePost(EditNodeModel model)
        {
            try
            {
                if (model.Id.HasValue)
                    _nodeAction.UpdateNode(model);
                else
                    _nodeAction.CreateNewNode(model);
            }
            catch (Exception exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure", message = exception.Message } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
        [HttpPost, ActionName("RelateNode")]
        public JsonResult RelateNodePost(RelateNodeModel model)
        {
            try
            {
                _nodeAction.RelateNode(model);
            }
            catch (Exception exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure", message = exception.Message } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }

        [HttpPost, ActionName("RemoveRelateNode")]
        public JsonResult RemoveRelateNodePost(Guid id)
        {
            try
            {
                _nodeAction.RemoveRelateNode(id);
            }
            catch (Exception exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure", message = exception.Message } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
        [HttpPost, ActionName("SettingNode")]
        public JsonResult SettingNodePost(Guid nodeId4Mark, NodeRecordMark nodeRecordMark)
        {
            try
            {
                _nodeAction.SettingNode(nodeId4Mark, nodeRecordMark);
            }
            catch (Exception exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure", message = exception.Message } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
        [HttpPost, ActionName("Delete")]
        public JsonResult DeletePost(Guid id)
        {
            try
            {
                _nodeAction.Delete(id);
            }
            catch (Exception exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure", message = exception.Message } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };

        }

    }
}
