using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Repository;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class NpcServiceTests
    {
        [TestMethod]
        public void TestMessageNotify()
        {
            var openMasConfig = new OpenMasConfigRepository().GetOpenMasConfigByUnit(Guid.Parse("59116356-F270-4845-BB7C-A1050188E6AD"));
            //调用上行短信获取接口获取短消息
            var sms = new OpenMas.Sms(openMasConfig.SmsMasService);
            var message = sms.GetMessage("8c063966-b5da-46a6-ae16-05c5e112886b");
            Console.WriteLine(message.Content);
        }
    }
}
