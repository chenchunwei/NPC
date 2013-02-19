using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Domain.Repository
{
    public class PhoneBookRepository : AbstractNhibernateRepository<Guid, PhoneBook>
    {
       public IList<PhoneBook> GetAll(Guid unitId)
       {
           return Session.CreateQuery("from PhoneBook where RecordDescription.IsDelete=0 and Unit.Id=:unitId").SetGuid("unitId", unitId).List<PhoneBook>();
       }

        public PhoneBook GetDefaultPhoneBook(Guid unitId)
        {
            return Session.CreateQuery("from PhoneBook where RecordDescription.IsDelete=0 and Unit.Id=:unitId and IsDefault=1").SetGuid("unitId", unitId).List<PhoneBook>().FirstOrDefault();            
        }
    }
}
