using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.PhoneBooks;
using NPC.Domain.Models.PhoneBooks;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class PhoneBookRecordAction : BaseAction
    {
        private readonly PhoneBookRecordRepository _phoneBookRecordRepository;
        private readonly PhoneBookRepository _phoneBookRepository;
        public PhoneBookRecordAction()
        {
            _phoneBookRecordRepository = new PhoneBookRecordRepository();
            _phoneBookRepository = new PhoneBookRepository();
        }

        #region 通讯录列表
        public PhoneBookRecordListModel InitializePhoneBookRecordListModel(PhoneBookRecordQueryItem queryItem)
        {
            if (queryItem.UnitId == null)
                throw new ArgumentException("queryItem.UnitId不能为null");
            var model = new PhoneBookRecordListModel();
            _phoneBookRepository.GetAll(queryItem.UnitId.Value).ToList().ForEach(o => model.PhoneBookRecordSearchModel.PhoneBookOptions.Add(o.Id.ToString(), o.Name));
            model.PhoneBookRecords = _phoneBookRecordRepository.Query(queryItem);
            model.PhoneBookRecordSearchModel.PhoneBookRecordQueryItem = queryItem;
            return model;
        }
        #endregion

        #region 新增更新通讯录记录
        public void NewPhoneBookRecord(EditPhoneBookRecordModel viewModel)
        {
            SaveOrUpdatePhoneBookRecord(viewModel);
        }

        public void UpdatePhoneBookRecord(EditPhoneBookRecordModel viewModel)
        {
            SaveOrUpdatePhoneBookRecord(viewModel);
        }

        private void SaveOrUpdatePhoneBookRecord(EditPhoneBookRecordModel viewModel)
        {
            var record = viewModel.Id.HasValue
                             ? _phoneBookRecordRepository.Find(viewModel.Id.Value)
                             : new PhoneBookRecord();
            var user = NpcContext.CurrentUser;
            record.Mobile = viewModel.Mobile;
            record.Name = viewModel.ContactName;
            record.PhoneBook = _phoneBookRepository.Find(viewModel.PhoneBookId);
            record.RecordDescription.CreateBy(user);
            record.RecordDescription.UpdateBy(user);
            _phoneBookRecordRepository.Save(record);
        }
        #endregion

        #region 删除联系人
        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }
        private void SingleDelete(Guid id)
        {
            var target = _phoneBookRecordRepository.Find(id);
            target.RecordDescription.Delete();
            _phoneBookRecordRepository.SaveOrUpdate(target);
        }
        #endregion

        #region 新增或更新通讯簿
        public void NewPhoneBook(EditPhoneBookModel viewModel)
        {
            SaveOrUpdatePhoneBook(viewModel);
        }

        public void UpdatePhoneBook(EditPhoneBookModel viewModel)
        {
            SaveOrUpdatePhoneBook(viewModel);
        }

        private void SaveOrUpdatePhoneBook(EditPhoneBookModel viewModel)
        {
            var record = viewModel.Id.HasValue
                             ? _phoneBookRepository.Find(viewModel.Id.Value)
                             : new PhoneBook();
            var user = NpcContext.CurrentUser;
            record.Name = viewModel.PhoneBookName;
            record.PhoneBookType = PhoneBookType.Unit;
            record.Unit = NpcContext.CurrentUser.Unit;
            record.RecordDescription.CreateBy(user);
            record.RecordDescription.UpdateBy(user);
            _phoneBookRepository.Save(record);
        }
        #endregion

        #region 通迅簿列表
        public PhoneBookListModel InitializePhoneBookListModel(Guid unitId)
        {
            var model = new PhoneBookListModel();
            model.PhoneBooks = _phoneBookRepository.GetAll(unitId);
            return model;
        }
        #endregion

        #region 删除通讯录
        public void DeletePhoneBook(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDeletePhoneBook);
        }
        private void SingleDeletePhoneBook(Guid id)
        {
            var target = _phoneBookRepository.Find(id);
            target.RecordDescription.Delete();
            _phoneBookRepository.SaveOrUpdate(target);
        }
        #endregion

        #region InitializeSelectedRecordsResponse
        public SelectedRecordsResponse InitializeSelectedRecordsResponse(SelectePhoneBookRecordModel selectedUsersModel)
        {

            var model = new SelectedRecordsResponse();
            var queryItem = new PhoneBookRecordQueryItem();
            queryItem.Pagination.PageSize = 1000;
            if (selectedUsersModel.CheckedAllPage)
            {
                queryItem.PhoneBookId = selectedUsersModel.WhereOptions.PhoneBookId;
                queryItem.Name = selectedUsersModel.WhereOptions.Name;
                queryItem.Mobile = selectedUsersModel.WhereOptions.Mobile;
            }
            else
            {
                if (!selectedUsersModel.Ids.Any())
                    return model;
                queryItem.Ids = selectedUsersModel.Ids;
            }
            queryItem.UnitId = selectedUsersModel.UnitId;
            _phoneBookRecordRepository.Query(queryItem).ToList().ForEach(phoneBookRecord =>
                model.Telnumbers.Add(phoneBookRecord.Mobile));
            return model;

        }
        #endregion
    }
}
