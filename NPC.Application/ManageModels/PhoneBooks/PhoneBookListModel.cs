using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Application.ManageModels.PhoneBooks
{
    public class PhoneBookListModel
    {
        public PhoneBookListModel()
        {
            PhoneBooks = new List<PhoneBook>();
        }
        public IList<PhoneBook> PhoneBooks { get; set; }
    }
}
