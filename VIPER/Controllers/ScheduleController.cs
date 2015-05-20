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
        public ActionResult Schedule()
        {
            return View();
        }

        public virtual JsonResult ReadJobs([DataSourceRequest] DataSourceRequest request)
        {
            return Json(scheduleRepo.GetAllJobs().ToDataSourceResult(request));
        }

        public virtual JsonResult UpdateJobs([DataSourceRequest] DataSourceRequest request, ScheduleViewModel s)
        {
            if (ModelState.IsValid)
            {
                scheduleRepo.Update(s, ModelState);
            }

            return Json(new[] { s }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult ReadAssignments([DataSourceRequest] DataSourceRequest request)
        {
            return Json(empRepo.GetAssignment().ToDataSourceResult(request));
        }

        public virtual JsonResult DestroyAssignment([DataSourceRequest] DataSourceRequest request, EmployeeProcessViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                empRepo.Delete(assignment);
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult CreateAssignment([DataSourceRequest] DataSourceRequest request, EmployeeProcessViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                empRepo.Insert(assignment, ModelState);
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult UpdateAssignment([DataSourceRequest] DataSourceRequest request, EmployeeProcessViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                empRepo.Update(assignment);
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }

    }
}