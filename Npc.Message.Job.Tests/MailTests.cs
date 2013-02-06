using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Mails;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Npc.Message.Job.Tests
{
    [TestClass]
    public class MailTests
    {
        [TestMethod]
        public void TestMailAttachments()
        {
            var attachments = new MailAttachments();
            Assert.AreEqual(0, attachments.Count, "初始化MailAttachments");
            attachments.Add("c:\\autoexec.bat");
            Assert.AreEqual(1, attachments.Count, "增加附件（附件确实存在）");
            attachments.Add("c:\\autoexec.dat.txt");
            Assert.AreEqual(1, attachments.Count, "增加附件（附件不存在）");
            attachments.Clear();
            Assert.AreEqual(0, attachments.Count, "清除附件");
        }

        [TestMethod]
        public void TestMailMessage()
        {
            var message = new MailMessage();
            Assert.AreEqual(0, message.Attachments.Count, "初始化MailAttachments");
            Assert.AreEqual(MailFormat.Text, message.BodyFormat, "邮件格式");
            Assert.AreEqual("GB2312", message.Charset, "缺省的字符集");
        }

        [TestMethod]
        public void TestSendMail()
        {
            SmtpMail.SmtpServer = "smtp.126.com";
            var mail = new MailMessage {From = "bbsuu@126.com", FromName = "陈春伟"};
            mail.AddRecipients("bbsuu@126.com");
            mail.Subject = "主题：测试邮件";
            mail.BodyFormat = MailFormat.Text;
            mail.Body = "测试的内容.";
            //mail.Attachments.Add("c:\\test.txt");
            SmtpMail.Send(mail, "bbsuu@126.com", "");//请填写自己的测试邮件帐号
        }
    }
}
