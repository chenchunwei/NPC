using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Models.NpcMmses;
using NPC.Domain.Models.NpcSmsSends;

namespace NPC.Domain.Repository
{
    public class NpcSmsSendRepository : AbstractNhibernateRepository<Guid, NpcSmsSend>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        #region 分页查询
        public IList<NpcSmsSend> Query(NpcSmsSendQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct smss.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct smss.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(NpcSmsSend)).List<NpcSmsSend>();
        }

        private static Tuple<string, Hashtable> FormatQuery(NpcSmsSendQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From NpcSmsSends smss ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And smss.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }

            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And smss.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And smss.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

        #region 查询待发送的信息
        public IList<NpcSmsSend> GetNpcMmsSendsWaitingSend()
        {
            return Session.CreateSQLQuery("Select * from NpcSmsSends where SendStatus=0 and IsDelete=0 ")
                       .AddEntity(typeof(NpcSmsSend))
                       .List<NpcSmsSend>();
        }
        #endregion

        #region 根据messageId获取彩信发送记录
        public NpcSmsSend GetByMessageId(string messageId)
        {
            return Session.CreateSQLQuery("Select * from NpcSmsSends where MessageId=:messageId ")
                      .AddEntity(typeof(NpcMmsSend))
                      .SetString("messageId",messageId)
                      .UniqueResult<NpcSmsSend>();
        }
        #endregion
    }
}
