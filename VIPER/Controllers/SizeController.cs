using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;
using VIPER.Tools;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        [AuthorizeAD(Groups = "FUN_DTUSHOU_VIPERAdmin_GSU")]
        public ActionResult Size()
        {
            return View();
        }

        public JsonResult JSONSize(int? RepairTypeID)
        {
            var sizes = sizeRepo.Sizes;

            if (RepairTypeID != null)
            {
                sizes = sizes.Where(s => s.RepairTypeID == RepairTypeID.GetValueOrDefault()).ToList();
            }
            return Json(sizes.Select(s => new { SizeID = s.SizeID, Name = s.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Size_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(sizeRepo.Sizes.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Size_Create([DataSourceRequest] DataSourceRequest request, SizeViewModel s)
        {
            if (s != null && ModelState.IsValid)
            {
                sizeRepo.Create(s);
            }

            return Json(new[] { s }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Size_Update([DataSourceRequest] DataSourceRequest request, SizeViewModel s)
        {
            if (s != null && ModelState.IsValid)
            {
                sizeRepo.Update(s);
            }

            return Json(new[] { s }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Size_Destroy([DataSourceRequest] DataSourceRequest request, SizeViewModel s)
        {
            if (s != null)
            {
                sizeRepo.Destroy(s);
            }

            return Json(new[] { s }.ToDataSourceResult(request, ModelState));
        }
    }
}