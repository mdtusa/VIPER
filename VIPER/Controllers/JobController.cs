using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;
using System.Web.Script.Serialization;
using VIPER.Tools;

namespace VIPER.Controllers
{
    public partial class AdminController : Controller
    {
        [AuthorizeAD(Groups = "FUN_DTUSHOU_VIPERAdmin_GSU")]
        public ActionResult Job()
        {
            return View();
        }

        public ActionResult Job_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(jobRepo.Jobs.ToDataSourceResult(request));
        }

       
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Job_Create([DataSourceRequest] DataSourceRequest request, JobViewModel j)
        {
            if (j != null && ModelState.IsValid)
            {
                jobRepo.Create(j);
            }

            return Json(new[] { j }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Job_Update([DataSourceRequest] DataSourceRequest request, JobViewModel j)
        {
            if (j != null && ModelState.IsValid)
            {
                jobRepo.Update(j);
            }

            return Json(new[] { j }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Job_Destroy([DataSourceRequest] DataSourceRequest request, JobViewModel j)
        {
            if (j != null)
            {
                jobRepo.Destroy(j);
            }

            return Json(new[] { j }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult JobProcess_Read([DataSourceRequest] DataSourceRequest request, int jobID)
        {
            return Json(jobRepo.GetJobProcesses(jobID).ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult JobProcess_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<JobProcessViewModel> jobProcesses)
        {
            if (jobProcesses != null && ModelState.IsValid)
            {
                foreach (var jobProcess in jobProcesses)
                {
                    jobRepo.UpdateJobProcesses(jobProcess);
                }
            }

            return Json(jobProcesses.ToDataSourceResult(request, ModelState));
        }

        public ActionResult JobCost_Read([DataSourceRequest] DataSourceRequest request, int jobID)
        {
            return Json(jobRepo.GetJobCost(jobID).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult JobCost_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IList<JobCostViewModel> jobCost)
        {
            if (jobCost != null && ModelState.IsValid)
            {
                jobRepo.UpdateJobCost(jobCost);
            }
            return Json(jobCost.ToDataSourceResult(request, ModelState));
        }

        public ActionResult JobSchedule_Read([DataSourceRequest] DataSourceRequest request, int jobID)
        {
            return Json(jobRepo.GetJobSchedule(jobID).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult JobSchedule_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<JobScheduleViewModel> jobSchedules)
        {
            if (jobSchedules != null && ModelState.IsValid)
            {
                foreach (var jobSchedule in jobSchedules)
                {
                    jobRepo.UpdateJobSchedule(jobSchedule);
                }
            }
            return Json(jobSchedules.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void UpdateJobStatus(int status, int jobID)
        {
            if(ModelState.IsValid)
            {
                jobRepo.UpdateJobStatus(status, jobID);
            }
        }

      
        [AcceptVerbs(HttpVerbs.Post)]
        public void UpdateJobProcessStatus(int status, int jobProcessID)
        {
            if(ModelState.IsValid)
            {
                jobRepo.UpdateJobProcessStatus(status, jobProcessID);
            }
        }
    }
}