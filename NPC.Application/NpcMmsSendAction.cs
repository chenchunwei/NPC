using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Application.ManageModels.NpcMmsSends;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class NpcMmsSendAction : BaseAction
    {
        private readonly NpcMmsSendRepository _npcMmsSendRepository;
        private readonly NpcMmsRepository _npcMmsRepository;
        public NpcMmsSendAction()
        {
            _npcMmsSendRepository = new NpcMmsSendRepository();
            _npcMmsRepository = new NpcMmsRepository();
        }
        public NpcMmsSendListModel InitializeNpcMmsSendListModel(NpcMmsSendQueryItem queryItem)
        {
            var model = new NpcMmsSendListModel();
            queryItem.UnitId = NpcContext.CurrentUser.Unit.Id;
            model.NpcMmsSendSearchModel.NpcMmsSendQueryItem = queryItem;
            model.NpcMmsSends = _npcMmsSendRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _npcMmsSendRepository.Find(id);
            target.RecordDescription.Delete();
            _npcMmsSendRepository.SaveOrUpdate(target);
        }

        public EditNpcMmsSendModel InitializeEditNpcMmsSendModel(Guid npcMmsId)
        {
            var model = new EditNpcMmsSendModel();
            model.NpcMms = _npcMmsRepository.Find(npcMmsId);
            model.SendTitle = model.NpcMms.Title;
            return model;
        }

        public void Send(EditNpcMmsSendModel model)
        {
            if (!model.Receivers.Any())
            {
                throw new ArgumentException("接收人未指定");
            }
            var trans = TransactionManager.BeginTransaction();
            try
            {

                var newNpcMmsSend = new NpcMmsSend();
                newNpcMmsSend.NpcMms = _npcMmsRepository.Find(model.NpcMmsId);
                foreach (var receiver in model.Receivers)
                {
                    newNpcMmsSend.NpcMmsReceivers.Add(new NpcMmsReceiver()
                                                          {
                                                              TelNum = receiver
                                                          });

                }
                newNpcMmsSend.TimeOfExceptSend = model.TimeOfExpectSend;
                newNpcMmsSend.Title = model.SendTitle;
                trans.Begin();
                _npcMmsSendRepository.Save(newNpcMmsSend);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
    }
}
