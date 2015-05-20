using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VIPER.Models.ViewModel;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        public virtual JsonResult ReadResources([DataSourceRequest] DataSourceRequest request)
        {
            return Json(empRepo.GetAll().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //public virtual JsonResult ReadPerson([DataSourceRequest] DataSourceRequest request)
        //{
        //    return Json(empRepo.GetPerson().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Employee()
        {
            ViewBag.Employee = empRepo.GetEmployee().ToList();
            return View();
        }

        public virtual JsonResult ReadEmpSchedule([DataSourceRequest] DataSourceRequest request)
        {
            return Json(empRepo.GetEmployeeCalendar().ToDataSourceResult(request));
        }

        public ActionResult Area()
        {
            ViewBag.Area = empRepo.GetArea().ToList();
            return View();
        }

        public virtual JsonResult ReadAreaSchedule([DataSourceRequest] DataSourceRequest request)
        {
            return Json(empRepo.GetAreaCalendar().ToDataSourceResult(request));
        }
    }
}