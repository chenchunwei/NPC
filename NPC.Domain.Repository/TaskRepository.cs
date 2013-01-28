using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Tasks;

namespace NPC.Domain.Repository
{
    public class TaskRepository : AbstractNhibernateRepository<Guid, Task>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<Task> Query(TaskQueryItem queryItem)
        {
            return null;
        }
    }
}
