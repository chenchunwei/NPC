﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application;
using NPC.Application.ManageModels.Nodes;
using NPC.Domain.Models.Nodes;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class NodeAction : BaseAction
    {
        private readonly NodeRepository _nodeRepository;
        private readonly ArticleCategoryRepository _articleCategoryRepository;
        public NodeAction()
        {
            _articleCategoryRepository = new ArticleCategoryRepository();
            _nodeRepository = new NodeRepository();
        }
        #region 初始化树模型
        public NodeTreeModel InitializeNodeTreeModel(Guid? id)
        {
            var model = new NodeTreeModel();
            var unitId = NpcContext.CurrentUser.Unit.Id;
            var subs = id.HasValue ? _nodeRepository.GetSubsInUnit(unitId, id.Value) : _nodeRepository.GetRootNodesInUnit(unitId);
            subs.ToList().ForEach(o => model.Components.Add(ConvertArticleCategoryToModel(o, true)));
            return model;

        }
        #endregion

        #region 转换ArticleCategory到Model
        private NodeTreeModelComponent ConvertArticleCategoryToModel(Node node, bool isNeedSub)
        {
            var model = new NodeTreeModelComponent()
            {
                Id = node.Id,
                Name = node.Name,
                CategoryId = node.OuterCategoryId,
                Code = node.Code,
                CategoryName = node.OuterCategoryId.HasValue ? _articleCategoryRepository.Find(node.OuterCategoryId.Value).CategoryName : "",
                IconCls = ApplicationConst.TreeLeafCls,
                NodeRecordMark = node.NodeRecordMark,
                OrderSort = node.OrderSort
            };
            var childrens = _nodeRepository.GetSubs(node.Id).ToList();
            if (childrens.Any())
            {
                if (isNeedSub)
                {
                    childrens.ForEach(o => model.Childrens.Add(ConvertArticleCategoryToModel(o, true)));
                }
                model.IconCls = ApplicationConst.TreeParentNode;
                model.State = isNeedSub ? "open" : "closed";
            }

            return model;
        }
        #endregion

        #region 添加新的节点
        public void CreateNewNode(EditNodeModel model)
        {
            var node = new Node();
            node.Name = model.FormData.Name;
            node.Code = model.FormData.Code;
            if (_nodeRepository.IsNodeCodeRepeatInUnit(NpcContext.CurrentUser.Unit.Id, node.Code, null))
            {
                throw new ApplicationException("节点编码不能重复，请重新设置！");
            }
            if (model.ParentId.HasValue)
            {
                node.ParentNode = _nodeRepository.Find(model.ParentId.Value);
            }
            node.OrderSort = model.FormData.OrderSort;
            node.Unit = NpcContext.CurrentUser.Unit;
            node.RecordDescription.CreateBy(NpcContext.CurrentUser);
            _nodeRepository.Save(node);
        }
        #endregion

        #region 编辑节点
        public void UpdateNode(EditNodeModel model)
        {
            if (model.Id == null)
                throw new ApplicationException("Id不能为null");
            var node = _nodeRepository.Find(model.Id.Value);
            node.Name = model.FormData.Name;
            node.Code = model.FormData.Code;
            node.OrderSort = model.FormData.OrderSort;
            if (_nodeRepository.IsNodeCodeRepeatInUnit(NpcContext.CurrentUser.Unit.Id, node.Code, node.Id))
            {
                throw new ApplicationException("节点编码不能重复，请重新设置！");
            }
            if (model.ParentId.HasValue)
            {
                node.ParentNode = _nodeRepository.Find(model.ParentId.Value);
            }
            node.Unit = NpcContext.CurrentUser.Unit;
            node.RecordDescription.UpdateBy(NpcContext.CurrentUser);
            _nodeRepository.Save(node);
        }
        #endregion

        #region 删除
        public void Delete(Guid id)
        {
            var target = _nodeRepository.Find(id);
            target.RecordDescription.Delete();
            _nodeRepository.Save(target);
        }
        #endregion

        public void RelateNode(RelateNodeModel model)
        {
            var node = _nodeRepository.Find(model.NodeId);
            node.OuterCategoryId = model.ArticleCategoryId;
            _nodeRepository.Save(node);
        }

        public void SettingNode(Guid nodeId, NodeRecordMark nodeRecordMark)
        {
            var node = _nodeRepository.Find(nodeId);
            node.NodeRecordMark = nodeRecordMark;
            _nodeRepository.Save(node);
        }

        public void RemoveRelateNode(Guid id)
        {
            var node = _nodeRepository.Find(id);
            node.OuterCategoryId = null;
            _nodeRepository.Save(node);
        }
    }
}
