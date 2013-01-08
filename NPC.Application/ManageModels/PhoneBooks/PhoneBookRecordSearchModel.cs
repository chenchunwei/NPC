using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Application.ManageModels.PhoneBooks
{
    public class PhoneBookRecordSearchModel
    {
        public PhoneBookRecordSearchModel()
        {
            PhoneBookRecordQueryItem = new PhoneBookRecordQueryItem();
        }
        public PhoneBookRecordQueryItem PhoneBookRecordQueryItem { get; set; }
    }
}
