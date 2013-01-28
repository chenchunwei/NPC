using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Flows;

namespace NPC.Domain.Repository
{
    public class FlowRepository : AbstractNhibernateRepository<Guid, Flow>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();
        public IList<Flow> GetInstanceFlow()
        {
            return Session.CreateQuery("from Flow where IsDelete=0 and FlowStatus in (:FlowStatus)")
                .SetParameterList("FlowStatus", new object[]{FlowStatus.Instance}).List<Flow>();
        }

        public IList<Flow> Query(FlowQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct f.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct f.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(Flow)).List<Flow>();
        }

        private static Tuple<string, Hashtable> FormatQuery(FlowQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From Flow f ");
            stringBuilder.Append("left join PhoneBookRecords pbr on u.Id= pbr.UserId ");
            stringBuilder.Append("left join Departments d on u.DepartmentId= d.Id ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And f.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }
           
            stringBuilder.Append("And f.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
    }
}
