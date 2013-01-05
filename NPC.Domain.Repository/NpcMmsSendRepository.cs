using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Repository
{
    public class NpcMmsSendRepository : AbstractNhibernateRepository<Guid, NpcMmsSend>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        #region 分页查询
        public IList<NpcMmsSend> Query(NpcMmsSendQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct mmss.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct mmss.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(NpcMmsSend)).List<NpcMmsSend>();
        }

        private static Tuple<string, Hashtable> FormatQuery(NpcMmsSendQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From NpcMmsSends mmss ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And mmss.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }

            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And mmss.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And mmss.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

        #region 查询待发送的信息
        public IList<NpcMmsSend> GetNpcMmsSendsWaitingSend()
        {
            return Session.CreateSQLQuery("Select * from NpcMmsSends where SendStatus=0 and IsDelete=0 ")
                       .AddEntity(typeof (NpcMmsSend))
                       .List<NpcMmsSend>();
        }
        #endregion
    }
}
