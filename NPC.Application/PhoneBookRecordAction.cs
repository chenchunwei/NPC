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

        public PhoneBookRecordListModel InitializePhoneBookRecordListModel(PhoneBookRecordQueryItem queryItem)
        {
            var model = new PhoneBookRecordListModel();
            _phoneBookRepository.GetAll(queryItem.UnitId.Value).ToList().ForEach(o => model.PhoneBookRecordSearchModel.PhoneBookOptions.Add(o.Id.ToString(), o.Name));
            model.PhoneBookRecords = _phoneBookRecordRepository.Query(queryItem);
            return model;
        }

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
            record.Mobile = viewModel.Moblie;
            record.Name = viewModel.ContactName;
            record.PhoneBook = _phoneBookRepository.Find(viewModel.PhoneBookId);
            record.RecordDescription.CreateBy(user);
            record.RecordDescription.UpdateBy(user);
            _phoneBookRecordRepository.Save(record);
        }

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
    }
}
