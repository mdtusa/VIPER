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
        public ActionResult Holiday()
        {
            return View();
        }

        public ActionResult Holiday_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(holidayRepo.Holidays.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Holiday_Create([DataSourceRequest] DataSourceRequest request, HolidayViewModel h)
        {
            if (h != null && ModelState.IsValid)
            {
                holidayRepo.Create(h);
            }

            return Json(new[] { h }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Holiday_Update([DataSourceRequest] DataSourceRequest request, HolidayViewModel h)
        {
            if (h != null && ModelState.IsValid)
            {
                holidayRepo.Update(h);
            }

            return Json(new[] { h }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Holiday_Destroy([DataSourceRequest] DataSourceRequest request, HolidayViewModel h)
        {
            if (h != null)
            {
                holidayRepo.Destroy(h);
            }

            return Json(new[] { h }.ToDataSourceResult(request, ModelState));
        }
    }
}