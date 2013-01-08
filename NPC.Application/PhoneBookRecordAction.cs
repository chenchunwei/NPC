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
        public PhoneBookRecordAction()
        {
            _phoneBookRecordRepository=new PhoneBookRecordRepository();
        }

        public PhoneBookRecordListModel InitializePhoneBookRecordListModel(PhoneBookRecordQueryItem queryItem)
        {
            var model = new PhoneBookRecordListModel();
            _phoneBookRecordRepository.Query(queryItem);
            return model;
        }
    }
}
