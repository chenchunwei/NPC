﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Models.NpcMmses;
using NPC.Domain.Models.OpenMasConfigs;
using NPC.Domain.Repository;
using NPC.Service;
using Npc.OpenMas;
using Npc.OpenMas.OmsHelper;
using OpenMas;
using log4net;
using RegionInfo = Npc.OpenMas.OmsHelper.RegionInfo;
using TextInfo = Npc.OpenMas.OmsHelper.TextInfo;

namespace NPC.Application.Services
{
    public class NpcMmsSendService
    {
        private readonly ILog _logger;
        private readonly NpcMmsSendRepository _npcMmsSendRepository;
        private readonly OpenMasConfigRepository _openMasConfigRepository;
        private static readonly Object Locker = new Object();
        private readonly string _baseDirectory;
        private readonly OpenMasConfigService _openMasConfigService;
        public NpcMmsSendService()
        {
            _baseDirectory = System.Configuration.ConfigurationManager.AppSettings["AttachmentsPath"];
            _logger = new DefaultLoggerFactory().GetLogger();
            _openMasConfigRepository = new OpenMasConfigRepository();
            _npcMmsSendRepository = new NpcMmsSendRepository();
            _openMasConfigService=new OpenMasConfigService();
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
                var parList = new List<ParInfo>();
                var config = _openMasConfigService.GetConfigOfUnit(npcMmsSend.Unit.Id);
                #region 创建彩信
                var count = 1;
                foreach (var content in npcMmsSend.NpcMms.NpcMmsContents.OrderBy(o => o.OrderSort))
                {
                    if (npcMmsSend.NpcMms.NpcMmsContents.Count == 1&&string.IsNullOrEmpty(content.Content))
                        content.LayoutType = LayoutType.PicTop;
                    var parInfo = new ParInfo();
                    parInfo.Dur = content.DueTime + "s";
                    var textContent = GetTextContent(content.Content + (count == npcMmsSend.NpcMms.NpcMmsContents.Count && !string.IsNullOrEmpty(config.Signature) ? "【" + config.Signature + "】" : string.Empty));
                    var picContent = GetMediaContent(content.UrlOfPic, MediaType.Pic);
                    var voiceContent = GetMediaContent(content.UrlOfVoice, MediaType.Voice);

                    if (content.LayoutType == LayoutType.PicBottom)
                    {
                        if (textContent != null)
                        {
                            mmsBuilder.AddContent(textContent);
                            parInfo.Text = new TextInfo { Src = textContent.ContentId, Region = "text" };
                        }
                        if (picContent != null)
                        {
                            mmsBuilder.AddContent(picContent);
                            parInfo.Img = new ImgInfo { Src = picContent.ContentId, Region = "image" };
                        }
                    }
                    else
                    {
                        if (picContent != null)
                        {
                            mmsBuilder.AddContent(picContent);
                            parInfo.Img = new ImgInfo { Src = picContent.ContentId, Region = "image" };
                        }
                        if (textContent != null)
                        {
                            mmsBuilder.AddContent(textContent);
                            parInfo.Text = new TextInfo { Src = textContent.ContentId, Region = "text" };
                        }
                    }
                    if (voiceContent != null)
                    {
                        parInfo.Audio = new AudioInfo { Src = voiceContent.ContentId };
                        mmsBuilder.AddContent(voiceContent);
                    }
                    count++;
                    parList.Add(parInfo);
                }
                var smil = CommonUtil.BuilderSmil(GetLayoutInfo(npcMmsSend.NpcMms.LayoutType, "image", "text"), parList);
                mmsBuilder.AddContent(GetSmilContent(smil));
                var mmsXml = mmsBuilder.BuildContentToXml();
                var mms = new Mms(config.MmsMasService);
                string messageId;
                if (npcMmsSend.TimeOfExceptSend == null)
                {
                    messageId = mms.SendMessage(npcMmsSend.NpcMmsReceivers.Select(o => o.TelNum).ToArray(), npcMmsSend.Title, mmsXml, config.MmsExtensionNo.ToString(CultureInfo.InvariantCulture), config.MmsAppAccount, config.MmsAppPwd);
                }
                else
                {
                    messageId = mms.SendMessage(npcMmsSend.NpcMmsReceivers.Select(o => o.TelNum).ToArray(), npcMmsSend.Title, mmsXml, config.MmsExtensionNo.ToString(CultureInfo.InvariantCulture), config.MmsAppAccount, config.MmsAppPwd, npcMmsSend.TimeOfExceptSend.Value);
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

        #region GetLayoutInfo
        private LayoutInfo GetLayoutInfo(LayoutType layoutType, string imageRegionName, string textRegionName)
        {
            var layout = new LayoutInfo();
            layout.Rootlayout = new RootlayoutInfo("128", "128", string.Empty);
            if (layoutType == LayoutType.PicBottom)
            {
                var text = new RegionInfo { Id = textRegionName, Left = "0", Top = "70%", Height = "30%", Width = "128", Fit = FitType.Scroll };
                var image = new RegionInfo { Id = imageRegionName, Left = "0", Top = "0", Height = "70%", Width = "128", Fit = FitType.Meet };
                layout.RegionList.Add(text);
                layout.RegionList.Add(image);
            }
            else
            {
                var image = new RegionInfo { Id = imageRegionName, Left = "0", Top = "30%", Height = "70%", Width = "128", Fit = FitType.Meet };
                var text = new RegionInfo { Id = textRegionName, Left = "0", Top = "0", Height = "30%", Width = "128", Fit = FitType.Scroll };
                layout.RegionList.Add(image);
                layout.RegionList.Add(text);
            }
            return layout;
        }
        #endregion

        #region GetSmilContent
        private MmsContent GetSmilContent(string smilStr)
        {
            var smilContent = MmsContent.CreateFromstring(smilStr);
            var simlName = Guid.NewGuid().ToString().Replace("-", "") + ".smil";
            smilContent.ContentId = simlName;
            smilContent.ContentLocation = simlName;
            smilContent.ContentType = MmsConst.SMIL;
            return smilContent;
        }
        #endregion

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
      }
}
