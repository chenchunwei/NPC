using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Domain.Models.Tasks;

namespace NPC.Website.Manage.Controllers
{
    public class TasksController : CommonController
    {
        private readonly TaskAction _taskAction;
        public TasksController()
        {
            _taskAction=new TaskAction();
        }
        public ActionResult MyTasks(TaskQueryItem queryItem)
        {
            queryItem.Pagination.PageIndex = PageIndex;
            var model = _taskAction.InitializeMyTasksModel(queryItem);
            return View(model);
        }
    }
}
