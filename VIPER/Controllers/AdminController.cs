using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIPER.Models.Repository;
using VIPER.Models.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using VIPER.Models.ViewModel;
using VIPER.Tools;


namespace VIPER.Controllers
{
    
    public partial class AdminController : Controller
    {
        private RepairTypeRepository rtRepo;
        private ProcessTimeRepository ptRepo;
        private SizeRepository sizeRepo;
        private JobRepository jobRepo;
        private ScheduleRepository scheduleRepo;
        private EmployeeRepository empRepo;
        public HolidayRepository holidayRepo;

        public AdminController()
        {
            rtRepo = new RepairTypeRepository();
            ptRepo = new ProcessTimeRepository();
            sizeRepo = new SizeRepository();
            jobRepo = new JobRepository();
            scheduleRepo = new ScheduleRepository();
            empRepo = new EmployeeRepository();
            holidayRepo = new HolidayRepository();
        }

        protected override void Dispose(bool disposing)
        {
            rtRepo.Dispose();
            sizeRepo.Dispose();
            ptRepo.Dispose();
            jobRepo.Dispose();
            scheduleRepo.Dispose();
            empRepo.Dispose();
            holidayRepo.Dispose();

            base.Dispose(disposing);
        }        
    }
}