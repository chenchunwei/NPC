using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
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
            var attachmentsPath = System.Configuration.ConfigurationManager.AppSettings["AttachmentsPath"];
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
                    content.ByteSize = content.CalculateSize(attachmentsPath);
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

        public void SendMms(NpcMms npcMms)
        {

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
    }
}
