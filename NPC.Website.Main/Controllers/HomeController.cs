using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.MianModels.Homes;

namespace NPC.Website.Main.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IndexAction _indexAction;
        public HomeController()
        {
            _indexAction = new IndexAction();
        }
        public ActionResult Index()
        {
            var model = _indexAction.InitializeIndexModel();
            return View(model);
        }
        public ActionResult Header()
        {
            var model = _indexAction.InitializeHeaderModel();
            return PartialView("_Header", model);
        }
        public ActionResult Footer()
        {
            Tuple<string, string> model = _indexAction.InitializeFooterModel();
            return PartialView("_Footer", model);
        }

        public ActionResult Detail(Guid id)
        {
            var model = _indexAction.InitializeDetailModel(id);
            return View(model);
        }
        public ActionResult List(ListModel viewModel)
        {
            viewModel.ArticleQueryItem.Pagination.PageIndex = PageIndex;
            var model = _indexAction.InitializeListModel(viewModel.ArticleQueryItem);
            return View(model);
        }
        public ActionResult SideBar()
        {
            var model = _indexAction.InitializeSideBarModel();
            return PartialView("_SideBar", model);
        }

        public ActionResult Records(RecordsModel recordsModel)
        {
            recordsModel.NodeRecordQueryItem.Pagination.PageIndex = PageIndex;
            var model = _indexAction.InitializeRecordsModel(recordsModel.NodeRecordQueryItem);
            return View(model);
        }
    }
}
