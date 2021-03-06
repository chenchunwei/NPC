﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Utilities;
using Fluent.Infrastructure.Web.HttpMoudles;
using NPC.Domain.Repository;
using UserInHttpMoudle = Fluent.Infrastructure.Web.HttpMoudles.User;

namespace NPC.Application
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }
        public UserInHttpMoudle Authencation(string account, string password, string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return null;
            var unitId = Guid.Parse(extension);

            var user = _userRepository.FindByAccountAndPwd(account, Md5Utility.GetMd5HashCode(password), unitId);
            if (user == null)
                return null;
            return new User() { Account = user.Account, Pwd = user.Pwd, Id = user.Id };
        }

        public UserInHttpMoudle GetAuthencationUser(string account, string password, string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return null;
            var unitId = Guid.Parse(extension);
            var user = _userRepository.FindByAccountAndPwd(account, Md5Utility.GetMd5HashCode(password), unitId);
            if (user == null)
                return null;
            return new User() { Account = user.Account, Pwd = user.Pwd, Id = user.Id };
        }

        public UserInHttpMoudle GetAuthencationUser(string account, string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return null;
            var unitId = Guid.Parse(extension);
            var user = _userRepository.FindByAccount(account, unitId);
            if (user == null)
                return null;
            return new User() { Account = user.Account, Pwd = user.Pwd, Id = user.Id };
        }
    }
}
