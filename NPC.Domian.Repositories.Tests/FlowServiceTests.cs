using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.FlowEngine;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class FlowServiceTests
    {
        protected FlowService FlowService { get; set; }
        protected FlowEngineService FlowEngineService { get; set; }

        public FlowServiceTests()
        {
            FlowEngineService = new FlowEngineService();
            FlowService = new FlowService();
        }
        [TestMethod]
        public void TestNewFlow()
        {
            var args = new Dictionary<string, string>();
            args.Add("Auditor", "41e4694f-5607-47ae-8098-a10701766863");

            FlowService.CreateFlowWithAssignId(Guid.NewGuid(),
                "ProposalFlow",
                null,
                "测试流程" + DateTime.Now.ToString("HH:mm:ss"),
                args,
               "注释");
        }

        [TestMethod]
        public void TestNewFlowNodeInstance()
        {
            FlowEngineService.CreateFlowNodeInstance();
        }

        [TestMethod]
        public void DealFlowNodeFlowTo()
        {
            FlowEngineService.DealFlowNodeFlowTo();

        }
    }
}
