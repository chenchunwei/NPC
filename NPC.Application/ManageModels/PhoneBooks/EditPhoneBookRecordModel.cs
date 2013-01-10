using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.PhoneBooks
{
    public class EditPhoneBookRecordModel
    {
        public Guid? Id { get; set; }
        public Guid PhoneBookId { get; set; }
        public string ContactName { get; set; }
        public string Moblie { get; set; }
    }
}
