using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.PhoneBooks;
using NPC.Domain.Models.Units;
using NPC.Domain.Repository;

namespace NPC.Service
{
    public class PhoneBookService
    {
        private readonly PhoneBookRepository _phoneBookRepository;
        public PhoneBookService()
        {
            _phoneBookRepository = new PhoneBookRepository();
        }
        public PhoneBook CreateOrGetDefaultPhoneBook(Unit unit)
        {
            var phoneBook = _phoneBookRepository.GetDefaultPhoneBook(unit.Id);
            if (phoneBook == null)
            {
                var newDefaultPhoneBook = new PhoneBook();
                newDefaultPhoneBook.Name = "默认";
                newDefaultPhoneBook.Unit = unit;
                newDefaultPhoneBook.IsDefault = true;
                newDefaultPhoneBook.PhoneBookType = PhoneBookType.Unit;
                newDefaultPhoneBook.RecordDescription.CreateBy(null);
                _phoneBookRepository.Save(newDefaultPhoneBook);
                return newDefaultPhoneBook;
            }
            return phoneBook;
        }
    }
}
