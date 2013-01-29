using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Tasks;

namespace NPC.Domain.Repository
{
    public class TaskRepository : AbstractNhibernateRepository<Guid, Task>
    {
        #region 分页查询
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<Task> Query(TaskQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct u.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct u.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(Task)).List<Task>();
        }

        private static Tuple<string, Hashtable> FormatQuery(TaskQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From Tasks t ");
            stringBuilder.Append("left join TaskUserStates tu on t.Id= tu.TaskId ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And t.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }
            if ( queryItem.UserId.HasValue)
            {
                stringBuilder.Append("And tu.UserId = :UserId ");
                parameters.Add("UserId", queryItem.UserId.Value);
            } 
            stringBuilder.Append("And t.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion
    }
}
