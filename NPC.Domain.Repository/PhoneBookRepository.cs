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
       

    }
}
