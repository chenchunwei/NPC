using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.PhoneBooks;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Website.Manage.Controllers
{
    public class PhoneBooksController : CommonController
    {
        private readonly PhoneBookRecordAction _phoneBookRecordAction;
        public PhoneBooksController()
        {
            _phoneBookRecordAction = new PhoneBookRecordAction();
        }
        public ActionResult PhoneBookRecordList(PhoneBookRecordListModel pageModel)
        {
            pageModel.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem.Pagination.PageIndex = PageIndex;
            pageModel.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _phoneBookRecordAction.InitializePhoneBookRecordListModel(pageModel.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem);
            return View(model);
        }

    }
}
