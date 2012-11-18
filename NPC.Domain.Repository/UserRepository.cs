using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Repository
{
    public class UserRepository : AbstractNhibernateRepository<Guid, User>
    {
        public IList<User> GetUserByDeparment(Guid departmentId)
        {
            return Session.CreateQuery("from User where Department.Id=:departmentId and  RecordDescription.IsDelete=0")
                       .SetGuid("departmentId", departmentId).List<User>();
        }
    }
}
