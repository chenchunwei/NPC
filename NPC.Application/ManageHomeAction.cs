using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class ManageHomeAction : BaseAction
    {
        private readonly UnitRepository _unitRepository;
        public ManageHomeAction()
        {
            _unitRepository = new UnitRepository();
        }

        public LoginModel InitializeLoginModel()
        {
            var model = new LoginModel();
            _unitRepository.GetAllUnits().ToList().ForEach(unit => model.UnitOptions.Add(unit.Id.ToString(), unit.Name));
            return model;
        }
    }
}
