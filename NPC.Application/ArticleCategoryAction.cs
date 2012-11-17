using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.ArticleCategories;
using NPC.Domain.Models.ArticleCategories;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class ArticleCategoryAction : BaseAction
    {
        private readonly ArticleCategoryRepository _articleCategoryRepository;
        public ArticleCategoryAction()
        {
            _articleCategoryRepository = new ArticleCategoryRepository();
        }
        #region 初始化树模型
        public ArticleCategoryTreeModel InitializeArticleCategoryTreeModel(Guid? id)
        {
            var model = new ArticleCategoryTreeModel();
            var subs = id.HasValue ? _articleCategoryRepository.GetSubs(NpcContext.CurrentUser.Unit.Id, id.Value) : _articleCategoryRepository.GetRoot(NpcContext.CurrentUser.Unit.Id);

            subs.ToList().ForEach(o => model.Components.Add(ConvertArticleCategoryToModel(o, true)));

            return model;

        }
        #endregion
        #region 转换ArticleCategory到Model
        private ArticleCategoryTreeModelComponent ConvertArticleCategoryToModel(ArticleCategory articleCategory, bool isNeedSub)
        {
            var model = new ArticleCategoryTreeModelComponent()
            {
                Id = articleCategory.Id,
                Name = articleCategory.CategoryName,
                IconCls = ApplicationConst.TreeLeafCls,
            };
            var childrens = _articleCategoryRepository.GetSubs(NpcContext.CurrentUser.Unit.Id, articleCategory.Id).ToList();
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

        #region 添加新的分类
        public void CreateNewCategory(EditArticleCategoryModel model)
        {
            var articleCategory = new ArticleCategory();
            articleCategory.CategoryName = model.FormData.Name;
            articleCategory.Unit = NpcContext.CurrentUser.Unit;
            if (model.Id.HasValue)
            {
                articleCategory.ParentArticleCategory = _articleCategoryRepository.Find(model.Id.Value);
            }
            _articleCategoryRepository.Save(articleCategory);
        }
        #endregion
    }
}
