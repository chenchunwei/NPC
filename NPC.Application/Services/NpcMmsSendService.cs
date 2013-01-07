using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Models.OpenMasConfigs;
using NPC.Domain.Repository;
using OpenMas;
using log4net;

namespace NPC.Application.Services
{
    public class NpcMmsSendService
    {
        private readonly ILog _logger;
        private readonly NpcMmsSendRepository _npcMmsSendRepository;
        private readonly OpenMasConfigRepository _openMasConfigRepository;
        private readonly IDictionary<Guid, OpenMasConfig> _openMasConfigs;
        private static readonly Object Locker = new Object();
        private readonly string _baseDirectory;

        public NpcMmsSendService()
        {
            _baseDirectory = System.Configuration.ConfigurationManager.AppSettings["AttachmentsPath"];
            _openMasConfigs = new Dictionary<Guid, OpenMasConfig>();
            _logger = new DefaultLoggerFactory().GetLogger();
            _openMasConfigRepository = new OpenMasConfigRepository();
            _npcMmsSendRepository = new NpcMmsSendRepository();
        }

        public void Execute()
        {
            lock (Locker)
            {
                var npcMmsSends = _npcMmsSendRepository.GetNpcMmsSendsWaitingSend();
                npcMmsSends.ToList().ForEach(SingleSend);
            }
        }

        public void SingleSend(NpcMmsSend npcMmsSend)
        {
            var trans = TransactionManager.BeginTransaction();
            trans.Begin();
            var mmsBuilder = new MmsBuilder();
            try
            {
                var config = GetConfigOfUnit(npcMmsSend.Unit.Id);
                #region 创建彩信
                foreach (var content in npcMmsSend.NpcMms.NpcMmsContents)
                {
                    var textContent = GetTextContent(content.Content);
                    var picContent = GetMediaContent(content.UrlOfPic, MediaType.Pic);
                    var voiceContent = GetMediaContent(content.UrlOfVoice, MediaType.Voice);
                    if (content.LayoutType == Domain.Models.NpcMmses.LayoutType.PicBottom)
                    {
                        if (textContent != null)
                            mmsBuilder.AddContent(textContent);
                        if (picContent != null)
                            mmsBuilder.AddContent(picContent);
                    }
                    else
                    {
                        if (picContent != null)
                            mmsBuilder.AddContent(picContent);
                        if (textContent != null)
                            mmsBuilder.AddContent(textContent);
                    }
                    if (voiceContent != null)
                        mmsBuilder.AddContent(voiceContent);

                }
                var mmsXml = mmsBuilder.BuildContentToXml();
                var mms = new Mms(config.MasService);
                string messageId;
                if (npcMmsSend.TimeOfExceptSend == null)
                {
                    messageId = mms.SendMessage(npcMmsSend.NpcMmsReceivers.Select(o => o.TelNum).ToArray(), npcMmsSend.Title, mmsXml, config.ExtensionNo.ToString(CultureInfo.InvariantCulture), config.AppAccount, config.AppPwd);
                }
                else
                {
                    messageId = mms.SendMessage(npcMmsSend.NpcMmsReceivers.Select(o => o.TelNum).ToArray(), npcMmsSend.Title, mmsXml, config.ExtensionNo.ToString(CultureInfo.InvariantCulture), config.AppAccount, config.AppPwd, npcMmsSend.TimeOfExceptSend.Value);
                }
                #endregion
                npcMmsSend.SendStatus = SendStatus.Done;
                npcMmsSend.MessageId = messageId;
                _npcMmsSendRepository.Save(npcMmsSend);
                trans.Commit();
                _logger.ErrorFormat("npcMmsSendId={0}发送成功,返回MessageId:{1}", npcMmsSend.Id, messageId);

            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("id={0}发送时出错:{1}", npcMmsSend.Id, exception);
                trans.Rollback();
                throw;
            }
        }

        #region GetTextContent
        private MmsContent GetTextContent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            var mmsContent = MmsContent.CreateFromstring(text);
            mmsContent.ContentId = Guid.NewGuid().ToString();
            mmsContent.ContentType = MmsConst.TEXT;
            return mmsContent;
        }
        #endregion

        #region GetMediaContent
        private MmsContent GetMediaContent(string reletivePath, MediaType mediaType)
        {
            if (string.IsNullOrEmpty(reletivePath))
                return null;

            var pathOfMedia = Path.Combine(_baseDirectory, reletivePath.TrimStart(new[] { '/', '\\' }));
            if (!File.Exists(pathOfMedia))
                throw new FileNotFoundException("未找到文件", pathOfMedia);

            var content = MmsContent.CreateFromFile(pathOfMedia);
            content.ContentId = System.Guid.NewGuid().ToString();
            content.ContentType = mediaType == MediaType.Pic ? GetPicType(pathOfMedia) : GetVoiceType(pathOfMedia);
            return content;
        }
        #endregion

        #region 判断彩信图片类型
        /// <summary>
        /// 判断彩信内容类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MmsContentType GetPicType(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            var extension = System.IO.Path.GetExtension(path) ?? string.Empty;
            switch (extension.ToLower(System.Globalization.CultureInfo.CurrentCulture))
            {
                case ".jpg":
                case ".jpeg":
                    return MmsConst.JPEG;
                case ".gif":
                    return MmsConst.GIF;
                case ".png":
                    return MmsConst.PNG;
                case ".wbmp":
                    return MmsConst.WBMP;
                default:
                    return MmsConst.JPEG;
            }
        }
        #endregion

        #region 判断铃声类型
        /// <summary>
        /// 判断彩信内容类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MmsContentType GetVoiceType(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            var extension = System.IO.Path.GetExtension(path) ?? string.Empty;
            switch (extension.ToLower(System.Globalization.CultureInfo.CurrentCulture))
            {
                case ".mid":
                    return MmsConst.MIDI;
                case ".amr":
                    return MmsConst.AMR;
                default:
                    throw new ArgumentException("铃声不能接受文件{0}的类型");
            }
        }
        #endregion

        #region MediaType
        enum MediaType
        {
            Voice = 0,
            Pic = 1
        }
        #endregion

        #region 获取彩信配置文件
        public OpenMasConfig GetConfigOfUnit(Guid unitId)
        {
            OpenMasConfig config;
            if (_openMasConfigs.ContainsKey(unitId))
            {
                config = _openMasConfigs[unitId];
            }
            else
            {
                config = _openMasConfigRepository.GetOpenMasConfigByUnit(unitId);
                _openMasConfigs.Add(unitId, config);
            }
            return config;
        }
        #endregion
    }
}
