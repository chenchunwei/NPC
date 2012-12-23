﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application.Contexts;
using NPC.Domain.Repository;

namespace NPC.Website.Town.Main.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            var unitRepository = new UnitRepository();
            Guid unitId;
            var unitIdInParam = System.Web.HttpContext.Current.Request["unitId"];
            if (string.IsNullOrEmpty(unitIdInParam))
            {
                var unitIdStr = System.Configuration.ConfigurationManager.AppSettings["DefaultUnitId"];
                unitId = Guid.Parse(unitIdStr);
            }
            else
            {
                unitId = Guid.Parse(unitIdInParam);
            }
            var unit = unitRepository.Find(unitId);
            System.Web.HttpContext.Current.Items[NpcMainWebContext.KeyOfUnit] = unit;
        }
        protected int PageIndex
        {
            get
            {
                int pageIndex;
                if (!string.IsNullOrEmpty(Request["pageIndex"]) && int.TryParse(Request["pageIndex"], out pageIndex))
                    return pageIndex;
                return 1;
            }
        }
    }
}