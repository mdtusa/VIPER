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
using VIPER.Models.Repository;

namespace VIPER.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepository custRepo;

        public CustomerController()
        {
            custRepo = new CustomerRepository();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public JsonResult JSONCustomer()
        {
            var json = Json(custRepo.Customers, JsonRequestBehavior.AllowGet);

            return json;
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(custRepo.Customers.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, Customer c)
        {
            if (c != null && ModelState.IsValid)
            {
                custRepo.Create(c);
            }

            return Json(new[] { c }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, Customer c)
        {
            if (c != null && ModelState.IsValid)
            {
                custRepo.Update(c);
            }

            return Json(new[] { c }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, Customer c)
        {
            if (c != null)
            {
                custRepo.Destroy(c);
            }

            return Json(new[] { c }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            custRepo.Dispose();
            base.Dispose(disposing);
        }

    }
}