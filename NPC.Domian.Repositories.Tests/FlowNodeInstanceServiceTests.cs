using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Repository;
using NPC.FlowEngine;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class FlowNodeInstanceServiceTests
    {
        protected FlowNodeInstanceService FlowNodeInstanceService { get; set; }
        protected UserRepository UserRepository { get; set; }
        protected FlowNodeInstanceRepository FlowNodeInstanceRepository { get; set; }

        public FlowNodeInstanceServiceTests()
        {
            FlowNodeInstanceService = new FlowNodeInstanceService();
            UserRepository = new UserRepository();
            FlowNodeInstanceRepository = new FlowNodeInstanceRepository();
        }

        [TestMethod]
        public void TestExecuteTask()
        {
            var args = new Dictionary<string, string>();
            args.Add("Auditor", "bbe7b257-4ce4-4bac-841b-a116017bc605");
            FlowNodeInstanceService.ExecuteFlowNodeInstance(Guid.Parse("a18a5b46-613f-4f09-9f74-a11c01811150")
                , "下一轮审批"
                , UserRepository.Find(Guid.Parse("41e4694f-5607-47ae-8098-a10701766863"))
                , "测试注释"
                , args);
        }

        [TestMethod]
        public void TestExecuteTaskTOfEnd()
        {
            var args = new Dictionary<string, string>();
            args.Add("Auditor", "bbe7b257-4ce4-4bac-841b-a116017bc605");
            FlowNodeInstanceService.ExecuteFlowNodeInstance(Guid.Parse("5dac7dce-e072-4263-b569-a11d01740824")
                , "办结"
                , UserRepository.Find(Guid.Parse("41e4694f-5607-47ae-8098-a10701766863"))
                , "测试注释2"
                , args);
        }

        [TestMethod]
        public void TestFlowNodeInstanceRepositoryQuery()
        {
            var queryItem = new FlowNodeInstanceQueryItem();
            queryItem.UserId = Guid.Parse("41e4694f-5607-47ae-8098-a10701766863");
            var tasks = FlowNodeInstanceRepository.Query(queryItem);
        }
    }
}
