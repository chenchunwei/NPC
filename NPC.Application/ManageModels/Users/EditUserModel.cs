using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Users
{
    /// <summary>
    /// 用户编辑新增视图
    /// </summary>
    public class EditUserModel
    {
        public EditUserModel()
        {
            FormData = new EditUserModelFormData();
        }
        public Guid? Id { get; set; }
        public EditUserModelFormData FormData { get; set; }
    }
    public class EditUserModelFormData
    {
        public Guid? DepartmentId { get; set; }
        public string Account { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string QQ { get; set; }
        public string Pwd { get; set; }
        public string RePwd { get; set; }
    }
}
