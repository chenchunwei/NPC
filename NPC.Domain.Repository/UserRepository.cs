using System;
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
        public IList<User> GetUsers(params Guid[] ids)
        {
            return Session.CreateQuery("from User Where Id in(:ids) And RecordDescription.IsDelete=0")
              .SetParameterList("ids", ids).List<User>();
        }

        public  User FindByAccount(string account,Guid unitId)
        {
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(account);
            //HACK:添加Account字段
            return Session.CreateSQLQuery("Select * from Users u Where u.Account=:account and UnitId=:UnitId").AddEntity(typeof(User))
                .SetGuid("UnitId", unitId)
                .SetString("account", account.Trim()).UniqueResult<User>();
        }

        public User FindByAccountAndPwd(string account,string pwd,Guid unitId)
        {
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(account);
            if (string.IsNullOrEmpty(pwd))
                throw new ArgumentNullException(pwd);
            //HACK:添加Account字段
            return Session.CreateSQLQuery("Select * from Users u Where u.Account=:account and Pwd=:Pwd and UnitId=:UnitId").AddEntity(typeof(User))
                .SetGuid("UnitId", unitId)
                .SetString("account", account.Trim())
                .SetString("Pwd", pwd.Trim())
                .UniqueResult<User>();
        }
    }
}
