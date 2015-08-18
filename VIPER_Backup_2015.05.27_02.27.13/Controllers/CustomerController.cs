using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        public JsonResult JSONCustomer()
        {
            return Json(customerRepo.Customers.Select(c => new { CustomerID = c.CustomerID, Name = c.Name }), JsonRequestBehavior.AllowGet);
        }

    }
}