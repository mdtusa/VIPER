using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIPER.Tools;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        [AuthorizeAD(Groups = "FUN_DTUSHOU_VIPERAdmin_GSU")]
        public ActionResult Hour()
        {
            return View();
        }

        public JsonResult JSONHour()
        {
            return Json(hourRepo.Hour, JsonRequestBehavior.AllowGet);
        }
    }
}