using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIPER.Models;
using System.Data.Entity;

namespace VIPER.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoutePermanent(new
            {
                controller = "KPI",
                action = "Workshop",
                id = DateTime.Now.Year.ToString()
            });
        }

       
    }
}
