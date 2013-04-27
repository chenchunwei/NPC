using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Models.NotifyMessages;
using NPC.Domain.Repository;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class NotifyMessageRepositoryTests
    {
        [TestMethod]
        public void SaveNotifyMessage()
        {
            var notifyMessage = new NotifyMessage();
            notifyMessage.ApplicationId = string.Empty;
            notifyMessage.Content = "我短信发起的一个测试议案";
            notifyMessage.ExtendCode = "11";
            notifyMessage.From = "15906690647";
            notifyMessage.MessageType = MessageType.Sms;
            notifyMessage.ReceivedTime = DateTime.Now;
            notifyMessage.Title = notifyMessage.Title;
            notifyMessage.To = "06666";
            var repository = new NotifyMessageRepository();
            repository.Save(notifyMessage);
        }
    }
}
