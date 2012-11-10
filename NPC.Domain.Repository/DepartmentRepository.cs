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

        public IEnumerable<Department> GetSubDepartment(Guid unitId, Guid id)
        {
            return Session.CreateQuery("from Department Where Unit.Id=:unitId And RecordDescription.IsDelete=0 and ParentId=:id")
                .SetGuid("id", id).SetGuid("unitId", unitId).List<Department>();
        }

        public IEnumerable<Department> GetRootDepartment(Guid unitId)
        {
            return Session.CreateQuery("from Department Where Unit.Id=:unitId and Parent is Null And RecordDescription.IsDelete=0")
               .SetGuid("unitId", unitId).List<Department>();
        }
    }
}
