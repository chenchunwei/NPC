using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Departments;

namespace NPC.Domain.Repository
{
    public class DepartmentRepository : AbstractNhibernateRepository<Guid, Department>
    {

    }
}
