using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Repository
{
    public class NpcMmsRepository : AbstractNhibernateRepository<Guid, NpcMms>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        #region 分页查询
        public IList<NpcMms> Query( NpcMmsQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct mms.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct mms.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(NpcMms)).List<NpcMms>();
        }

        private static Tuple<string, Hashtable> FormatQuery(NpcMmsQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From NpcMmses mms ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And mms.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }
             
            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And mms.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And mms.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

    }
}
