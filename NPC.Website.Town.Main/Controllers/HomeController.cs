﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.ManageModels;
using NPC.Application.MianModels.Homes;

namespace NPC.Website.Town.Main.Controllers
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
            var model = _indexAction.InitializeMainTownIndexModel();
            return View(model);
        }
        public ActionResult Header()
        {
            var model = _indexAction.InitializeMianTownHeaderModel();
            return PartialView("_Header", model);
        }
        public ActionResult Footer()
        {
           var model = _indexAction.InitializeMainTownFooterModel();
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
            viewModel.ArticleQueryItem.UnitId = UnitId;
            var model = _indexAction.InitializeListModel(viewModel.ArticleQueryItem);
            return View(model);
        }
        public ActionResult RightBar()
        {
            var model = _indexAction.InitializeRightBarModel();
            return PartialView("_RightBar", model);
        }

        public ActionResult Records(RecordsModel recordsModel)
        {
            recordsModel.NodeRecordQueryItem.Pagination.PageIndex = PageIndex;
            recordsModel.NodeRecordQueryItem.UnitId = UnitId;
            var model = _indexAction.InitializeRecordsModel(recordsModel.NodeRecordQueryItem);
            return View(model);
        }

        public ActionResult Message(RedirectMessageModel model)
        {
            return View(model);
        }
    }
}
