using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NotifyMessages;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Repository
{
    public class NotifyMessageRepository : AbstractNhibernateRepository<Guid, NotifyMessage>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        #region 分页查询
        public IList<NotifyMessage> Query(NotifyMessageQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct nm.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct nm.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(NotifyMessage)).List<NotifyMessage>();
        }

        private static Tuple<string, Hashtable> FormatQuery(NotifyMessageQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From NotifyMessage nm ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And nm.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }
             
            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And nm.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And nm.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

    }
}
