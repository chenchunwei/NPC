using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Application.ManageModels.PhoneBooks
{
    public class PhoneBookRecordListModel
    {
        public PhoneBookRecordListModel()
        {
            PhoneBookRecords = new List<PhoneBookRecord>();
            PhoneBookRecordSearchModel = new PhoneBookRecordSearchModel();
        }
        public IList<PhoneBookRecord> PhoneBookRecords { get; set; }
        public PhoneBookRecordSearchModel PhoneBookRecordSearchModel { get; set; }
    }
}
