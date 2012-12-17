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

        #region 分页查询
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
            stringBuilder.Append("join ArticleCategories ac on a.ArticleCategoryId= ac.Id ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Keyword))
            {
                stringBuilder.Append("And a.Title like :Keyword ");
                parameters.Add("Keyword", "%" + queryItem.Keyword + "%");
            }
            if (queryItem.IsShow.HasValue)
            {
                stringBuilder.Append("And a.IsShow =:IsShow ");
                parameters.Add("IsShow", queryItem.IsShow.Value);
            }
            if (queryItem.CategoryId.HasValue)
            {
                stringBuilder.Append("And a.ArticleCategoryId = :CategoryId ");
                parameters.Add("CategoryId", queryItem.CategoryId);
            }
            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And ac.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And a.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

        public IList<Article> GetTopNWithPic(Guid unitId, Guid categoryId, int picTopN, int normalTopN)
        {
            if (picTopN <= 0 && normalTopN <= 0)
            {
                return new List<Article>();
            }
            if (picTopN <= 0)
            {
                return GetTopN(unitId, categoryId, normalTopN);
            }
            if (normalTopN <= 0)
            {
                return GetTopNPic(unitId, categoryId, picTopN);
            }
            return Session.CreateSQLQuery(string.Format(@"select * from (select top {0} a1.* from Articles a1 join ArticleCategories ac1 on a1.ArticleCategoryId=ac1.Id  
                where a1.ArticleCategoryId=:ArticleCategoryId and a1.IsDelete=0 and a1.UrlOfCoverImage is not Null and a1.IsShow=1 and ac1.UnitId=:UnitId Order by a1.DateOfCreate Desc) as t1 
                union 
                select * from (select top {1} * from Articles a2 join ArticleCategories ac2 on a2.ArticleCategoryId=ac2.Id  where a2.Id not in 
                (select top {0} a3.Id from Articles a3 join ArticleCategories ac3 join a3.ArticleCategoryId=ac3.Id where a3.ArticleCategoryId=:ArticleCategoryId 
                and a3.IsDelete=0 and a3.IsShow=1 and ac3.UnitId=:UnitId  ) and a2.IsDelete=0 and a2.IsShow=1 Order by a2.DateOfCreate Desc) as t2", picTopN, normalTopN))
                .AddEntity(typeof(Article))
                .SetGuid("UnitId", unitId)
                .SetGuid("ArticleCategoryId", categoryId)
                .List<Article>();
        }

        public IList<Article> GetTopN(Guid unitId, Guid categoryId, int topN)
        {
            return Session.CreateSQLQuery(string.Format(@"select top {0} * from Articles a join ArticleCategories ac on a.ArticleCategoryId=ac.Id 
                where a.ArticleCategoryId=:ArticleCategoryId and a.IsDelete=0  and a.IsShow=1 Order by a.DateOfCreate Desc", topN))
                .AddEntity(typeof(Article))
                .SetGuid("UnitId", unitId)
                .SetGuid("ArticleCategoryId", categoryId)
                .List<Article>();
        }

        public IList<Article> GetTopNPic(Guid unitId, Guid categoryId, int topN)
        {
            return Session.CreateSQLQuery(string.Format(@"select top {0} * from Articles a join ArticleCategories ac on a.ArticleCategoryId=ac.Id 
                where a.ArticleCategoryId=:ArticleCategoryId 
                and a.IsDelete=0 and a.UrlOfCoverImage is not Null and a.IsShow=1 Order by a.DateOfCreate Desc", topN))
                .AddEntity(typeof(Article))
                .SetGuid("UnitId", unitId)
                .SetGuid("ArticleCategoryId", categoryId)
                .List<Article>();
        }
    }
}
