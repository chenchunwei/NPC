using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;

namespace NPC.Application.ManageModels.ArticleCategories
{
    public class EditArticleCategoryModel
    {
        public Guid? Id { get; set; }
        public EditArticleCategoryModelFormData FormData { get; set; }
        public Unit Unit { get; set; }
    }

    public class EditArticleCategoryModelFormData
    {
        public string Name { get; set; }
    }
}
