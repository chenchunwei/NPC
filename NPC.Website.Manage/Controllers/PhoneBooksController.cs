using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
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

        public ActionResult SelectPhoneBookRecord(PhoneBookRecordListModel pageModel)
        {
            pageModel.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem.Pagination.PageIndex = PageIndex;
            pageModel.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _phoneBookRecordAction.InitializePhoneBookRecordListModel(pageModel.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem);
            return View(model);
        }

        #region 编辑或新增联系人
        [HttpPost, ActionName("EditPhoneBookRecord")]
        public ActionResult EditPhoneBookRecordPost(EditPhoneBookRecordModel viewModel)
        {
            try
            {
                if (viewModel.Id.HasValue)
                {
                    _phoneBookRecordAction.NewPhoneBookRecord(viewModel);
                }
                else
                {
                    _phoneBookRecordAction.UpdatePhoneBookRecord(viewModel);
                }
                return RedirectToMessage("保存成功");
            }
            catch (Exception exception)
            {
                Logger.ErrorFormat("处理数据{0}时异常：{1}", Newtonsoft.Json.JsonConvert.SerializeObject(viewModel), exception);
                return RedirectToMessage(HttpUtility.UrlEncode(exception.Message));
            }
        }
        #endregion
        #region 编辑或新增电话簿
        [HttpPost, ActionName("EditPhoneBook")]
        public ActionResult EditPhoneBookPost(EditPhoneBookModel viewModel)
        {
            try
            {
                viewModel.Unit = new NpcContext().CurrentUser.Unit;
                if (viewModel.Id.HasValue)
                {
                    _phoneBookRecordAction.NewPhoneBook(viewModel);
                }
                else
                {
                    _phoneBookRecordAction.UpdatePhoneBook(viewModel);
                }
                return RedirectToMessage("保存成功");
            }
            catch (Exception exception)
            {
                Logger.ErrorFormat("处理数据{0}时异常：{1}", Newtonsoft.Json.JsonConvert.SerializeObject(viewModel), exception);
                return RedirectToMessage(HttpUtility.UrlEncode(exception.Message));
            }
        }
        #endregion

        #region 删除记录
        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _phoneBookRecordAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
        #endregion

        #region 删除通讯簿
        [HttpPost, ActionName("DeletePhoneBook")]
        public JsonResult DeletePhoneBook()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _phoneBookRecordAction.DeletePhoneBook(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
        #endregion

        #region 电话簿列表
        public ActionResult PhoneBookList()
        {
            var unitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _phoneBookRecordAction.InitializePhoneBookListModel(unitId);
            return View(model);
        }
        #endregion
    }
}
