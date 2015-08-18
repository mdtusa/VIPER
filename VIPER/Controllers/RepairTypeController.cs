using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;
using VIPER.Tools;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        [AuthorizeAD(Groups = "FUN_DTUSHOU_VIPERAdmin_GSU")]
        public ActionResult RepairType()
        {
            return View();
        }

        public JsonResult JSONRepairType()
        {
            //return Json(rtRepo.RepairTypes.Select(rt => new { RepairTypeID = rt.RepairTypeID, Name = rt.Name }), JsonRequestBehavior.AllowGet);
            return Json(rtRepo.RepairTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RepairType_Read([DataSourceRequest] DataSourceRequest request)
        {

            return Json(rtRepo.RepairTypes.ToDataSourceResult(request));
        }

        public ActionResult FilterMenuCustomization_RepairTypes()
        {
            return Json(rtRepo.RepairTypes.Select(rt => rt.Name).Distinct(), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RepairType_Create([DataSourceRequest] DataSourceRequest request, RepairType rt)
        {
            if (rt != null && ModelState.IsValid)
            {
                rtRepo.Create(rt);
            }

            return Json(new[] { rt }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RepairType_Update([DataSourceRequest] DataSourceRequest request, RepairType rt)
        {
            if (rt != null && ModelState.IsValid)
            {
                rtRepo.Update(rt);
            }

            return Json(new[] { rt }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RepairType_Destroy([DataSourceRequest] DataSourceRequest request, RepairType rt)
        {
            if (rt != null)
            {
                rtRepo.Destroy(rt);
            }

            return Json(new[] { rt }.ToDataSourceResult(request, ModelState));
        }
    }
}