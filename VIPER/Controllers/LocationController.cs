using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        public JsonResult JSONLocation()
        {
            return Json(locationRepo.Locations.Select(l => new { LocationID = l.LocationID, Name = l.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}