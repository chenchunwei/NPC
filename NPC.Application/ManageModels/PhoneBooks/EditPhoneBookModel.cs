using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;

namespace NPC.Application.ManageModels.PhoneBooks
{
    public class EditPhoneBookModel
    {
        public string PhoneBookName { get; set; }
        public Unit Unit { get; set; }
    }
}
