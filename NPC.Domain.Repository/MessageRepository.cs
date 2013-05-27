using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Messages;
using System.Collections;

namespace NPC.Domain.Repository
{
    public class MessageRepository : AbstractNhibernateRepository<Guid, Message>
    {
        #region 分页查询
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<Message> Query(MessageQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct me.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct me.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(Message)).List<Message>();
        }

        private static Tuple<string, Hashtable> FormatQuery(MessageQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From Message me ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And me.UnitId =:UnitId ");
                parameters.Add("UnitId", queryItem.UnitId);
            }
            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And me.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }
            if (!string.IsNullOrEmpty(queryItem.MessageContent))
            {
                stringBuilder.Append("And me.MessageContent like :MessageContent ");
                parameters.Add("MessageContent", "%" + queryItem.MessageContent + "%");
            }
            stringBuilder.Append("And me.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

        #region 根据id 集合批量获取记录
        public IList<Message> Find(IList<Guid> ids)
        {
            if (!ids.Any())
            {
                return new List<Message>();
            }
            return Session.CreateSQLQuery("select * from Message where id in (:ids)")
                .AddEntity(typeof(Message))
                .SetParameterList("ids", ids)
                .List<Message>();
        }
        #endregion
    }
}