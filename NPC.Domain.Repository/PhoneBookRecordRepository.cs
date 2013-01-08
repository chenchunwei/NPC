using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Domain.Repository
{
    public class PhoneBookRecordRepository : AbstractNhibernateRepository<Guid, PhoneBookRecord>
    {
        #region 分页查询
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<PhoneBookRecord> Query(PhoneBookRecordQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct pbr.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct pbr.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(PhoneBookRecord)).List<PhoneBookRecord>();
        }

        private static Tuple<string, Hashtable> FormatQuery(PhoneBookRecordQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From PhoneBookRecords pbr ");
            stringBuilder.Append("join PhoneBooks pb on pb.Id= pbr.PhoneBookId ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Name))
            {
                stringBuilder.Append("And pbr.Name like :Name ");
                parameters.Add("Name", "%" + queryItem.Name + "%");
            }
            if (!string.IsNullOrEmpty(queryItem.Mobile))
            {
                stringBuilder.Append("And pbr.Mobile =:Mobile ");
                parameters.Add("Mobile", queryItem.Mobile);
            }
            if (queryItem.PhoneBookId.HasValue)
            {
                stringBuilder.Append("And pbr.PhoneBookId = :PhoneBookId ");
                parameters.Add("PhoneBookId", queryItem.PhoneBookId);
            }
            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And pb.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And pbr.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

    }
}
