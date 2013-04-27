using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Repository
{
    public class UserRepository : AbstractNhibernateRepository<Guid, User>
    {
        public IList<User> GetUserByDeparment(Guid departmentId)
        {
            return Session.CreateQuery("from User where Department.Id=:departmentId and  RecordDescription.IsDelete=0")
                       .SetGuid("departmentId", departmentId).List<User>();
        }

        public IList<User> GetUserByDeparmentLike(Guid departmentId)
        {
            return Session.CreateQuery("from User where Department.Path like :departmentIdLike and  RecordDescription.IsDelete=0")
                       .SetString("departmentIdLike", "%" + departmentId + "%").List<User>();
        }
        public IList<User> GetUsers(params Guid[] ids)
        {
            if (ids == null || !ids.Any())
                return new List<User>();
            return Session.CreateQuery("from User Where Id in(:ids) And RecordDescription.IsDelete=0")
              .SetParameterList("ids", ids).List<User>();
        }

        public User FindByAccount(string account, Guid unitId)
        {
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(account);
            //HACK:添加Account字段
            return Session.CreateSQLQuery("Select * from Users u Where u.Account=:account and UnitId=:UnitId and u.IsDelete=0").AddEntity(typeof(User))
                .SetGuid("UnitId", unitId)
                .SetString("account", account.Trim()).UniqueResult<User>();
        }

        public User FindByAccountAndPwd(string account, string pwd, Guid unitId)
        {
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(account);
            if (string.IsNullOrEmpty(pwd))
                throw new ArgumentNullException(pwd);
            //HACK:添加Account字段
            return Session.CreateSQLQuery("Select * from Users u Where u.Account=:account and Pwd=:Pwd and UnitId=:UnitId and u.IsDelete=0").AddEntity(typeof(User))
                .SetGuid("UnitId", unitId)
                .SetString("account", account.Trim())
                .SetString("Pwd", pwd.Trim())
                .UniqueResult<User>();
        }

        public bool IsRepeatAccount(string account, Guid unitId)
        {
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(account);
            //HACK:添加Account字段
            return Session.CreateSQLQuery("Select count(*) from Users u Where u.Account=:account and UnitId=:UnitId and u.IsDelete=0")
                .SetGuid("UnitId", unitId)
                .SetString("account", account.Trim()).UniqueResult<int>() > 0;
        }

        #region 分页查询
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<User> Query(UserQueryItem queryItem)
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

            return query.AddEntity(typeof(User)).List<User>();
        }

        private static Tuple<string, Hashtable> FormatQuery(UserQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From Users u ");
            stringBuilder.Append("left join PhoneBookRecords pbr on u.Id= pbr.UserId ");
            stringBuilder.Append("left join Departments d on u.DepartmentId= d.Id ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Name))
            {
                stringBuilder.Append("And u.Name like :Name ");
                parameters.Add("Name", "%" + queryItem.Name + "%");
            }
            if (!string.IsNullOrEmpty(queryItem.Mobile))
            {
                stringBuilder.Append("And pbr.Mobile like :Mobile ");
                parameters.Add("Mobile", "%" + queryItem.Mobile + "%");
            }
            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And u.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId);
            }
            if (queryItem.Ids.Any())
            {
                stringBuilder.Append("And u.Id in (:Ids) ");
                parameters.Add("Ids", queryItem.Ids);
            }
            if (queryItem.DepartmentLikeId.HasValue)
            {
                stringBuilder.Append("And d.Path like :DepartmentLikeId ");
                parameters.Add("DepartmentLikeId", "%" + queryItem.DepartmentLikeId.Value + "%");
            }
            stringBuilder.Append("And u.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

        #region 根据手机号码获取用户
        public User FindByMobile(string mobile)
        {
            var user = Session.CreateSQLQuery(@"select top 1 * from users where account=:mobile")
                .AddEntity(typeof(User)).SetString("mobile", mobile).UniqueResult<User>();
            user = user ?? Session.CreateSQLQuery(@"select top 1 u.* from users u join PhoneBookRecords pbr 
                    on u.Id=pbr.UserId where pbr.Mobile=:mobile")
                .AddEntity(typeof(User)).SetString("mobile", mobile).UniqueResult<User>();
            return user;
        }
        #endregion

    }
}
