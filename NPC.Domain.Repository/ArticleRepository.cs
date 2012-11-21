using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Articles;
using NPC.Query.Articles;

namespace NPC.Domain.Repository
{
    public class ArticleRepository : AbstractNhibernateRepository<Guid, Article>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<Article> Query(ArticleQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct a.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct a.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(Article)).List<Article>();
        }

        private static Tuple<string, Hashtable> FormatQuery(ArticleQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From Articles a ");
             stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            //if (!string.IsNullOrEmpty(infrelQueryParameters.Keyword))
            //{
            //    stringBuilder.Append("And i.REL_TITLE like :Keyword ");
            //    parameters.Add("Keyword", "%" + infrelQueryParameters.Keyword + "%");
            //}
            stringBuilder.Append("And a.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
    }
}
