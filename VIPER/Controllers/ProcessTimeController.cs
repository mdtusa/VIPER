using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using VIPER.Models.ViewModel;
using VIPER.Tools;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        [AuthorizeAD(Groups = "FUN_DTUSHOU_VIPERAdmin_GSU")]
        public ActionResult ProcessTime()
        {
            return View();
        }

        public ActionResult Process_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(ptRepo.ProcessTimeFromSQL.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Process_Create([DataSourceRequest] DataSourceRequest request, ProcessTimeViewModel p)
        {
            if (p != null && ModelState.IsValid)
            {
                ptRepo.Create(p);
            }

            return Json(new[] { p }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Process_Update([DataSourceRequest] DataSourceRequest request, ProcessTimeViewModel p)
        {
            if (p != null && ModelState.IsValid)
            {
                ptRepo.Update(p);
            }

            return Json(new[] { p }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Process_Destroy([DataSourceRequest] DataSourceRequest request, ProcessTimeViewModel p)
        {
            if (p != null)
            {
                ptRepo.Destroy(p);
            }

            return Json(new[] { p }.ToDataSourceResult(request, ModelState));
        }
       
    }
}