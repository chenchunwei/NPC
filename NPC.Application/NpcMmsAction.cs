using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Application.Common;
using NPC.Application.ManageModels.NpcMmses;
using NPC.Domain.Models.NpcMmses;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class NpcMmsAction : BaseAction
    {
        private readonly NpcMmsRepository _npcMmsRepository;
        public NpcMmsAction()
        {
            _npcMmsRepository = new NpcMmsRepository();
        }
        public NpcMms NewNpcMms(EditNpcMmsModel model)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var npcMms = new NpcMms();
                npcMms.Title = model.FormData.Title;
                npcMms.Unit = NpcContext.CurrentUser.Unit;
                npcMms.RecordDescription.CreateBy(NpcContext.CurrentUser);
                model.FrameSerializers.ToList().ForEach(frame =>
                {
                    if (string.IsNullOrEmpty(frame.Image) &&
                        string.IsNullOrEmpty(frame.Image) &&
                        string.IsNullOrEmpty(frame.Voice))
                    {
                        return;
                    }
                    var content = new NpcMmsContent()
                    {
                        Content = frame.Text,
                        DueTime = frame.TimeDuring,
                        LayoutType = model.FormData.LayoutType,
                        OrderSort = frame.OrderSort,
                        UrlOfPic = frame.Image,
                        UrlOfVoice = frame.Voice
                    };
                    npcMms.LayoutType = model.FormData.LayoutType;
                    content.ByteSize = content.CalculateSize(AppConfig.AttachmentsPath);
                    npcMms.NpcMmsContents.Add(content);
                });
                _npcMmsRepository.Save(npcMms);
                trans.Commit();
                return npcMms;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public NpcMms UpdateNpcMms(EditNpcMmsModel model)
        {
            if (model.Id == null)
                throw new ArgumentException("id不能为null");
            var trans = TransactionManager.BeginTransaction();
            
            try
            {
                var npcMms = _npcMmsRepository.Find(model.Id.Value);
                npcMms.Title = model.FormData.Title;
                npcMms.RecordDescription.UpdateBy(NpcContext.CurrentUser);
                //删除所有已为空的帧及被删除的删
                npcMms.NpcMmsContents.Where(o => model.FrameSerializers.All(oo => oo.Id != o.Id
                    || (o.Id == oo.Id && string.IsNullOrEmpty(oo.Image)
                    && string.IsNullOrEmpty(oo.Image) && string.IsNullOrEmpty(oo.Voice)))).ToList()
                      .ForEach(o => npcMms.NpcMmsContents.Remove(o));
                model.FrameSerializers.ToList().ForEach(frame =>
                {
                    if (string.IsNullOrEmpty(frame.Image) && string.IsNullOrEmpty(frame.Text) && string.IsNullOrEmpty(frame.Voice))
                    {
                        return;
                    }
                    var existContent = npcMms.NpcMmsContents.FirstOrDefault(o => o.Id == frame.Id);
                    var content = existContent ?? new NpcMmsContent();
                    content.Content = frame.Text;
                    content.DueTime = frame.TimeDuring;
                    content.LayoutType = model.FormData.LayoutType;
                    content.OrderSort = frame.OrderSort;
                    content.UrlOfPic = frame.Image;
                    content.UrlOfVoice = frame.Voice;
                    npcMms.LayoutType = model.FormData.LayoutType;
                    content.ByteSize = content.CalculateSize(AppConfig.AttachmentsPath);
                    if (existContent == null)
                        npcMms.NpcMmsContents.Add(content);
                });
                _npcMmsRepository.Save(npcMms);
                trans.Commit();
                return npcMms;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public NpcMmsListModel InitializeNpcMmsListModel(NpcMmsQueryItem queryItem)
        {
            var model = new NpcMmsListModel();
            queryItem.UnitId = NpcContext.CurrentUser.Unit.Id;
            model.NpcMmsSearchModel.NpcMmsQueryItem = queryItem;
            model.NpcMmses = _npcMmsRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _npcMmsRepository.Find(id);
            target.RecordDescription.Delete();
            _npcMmsRepository.SaveOrUpdate(target);
        }

        public EditNpcMmsModel InitializeEditNpcMmsModel(Guid? id)
        {
            var model = new EditNpcMmsModel();
            if (id.HasValue)
            {
                var npcMms = _npcMmsRepository.Find(id.Value);
                npcMms.NpcMmsContents.ToList().ForEach(npcContent =>
                    model.FrameSerializers.Add(new FrameSerializer()
                    {
                        Image = npcContent.UrlOfPic,
                        OrderSort = npcContent.OrderSort,
                        Text = npcContent.Content,
                        TimeDuring = npcContent.DueTime,
                        Voice = npcContent.UrlOfVoice,
                        Id = npcContent.Id
                    }));
                model.FormData.LayoutType = npcMms.LayoutType;
                model.FormData.Title = npcMms.Title;
            }
            return model;
        }
    }
}
