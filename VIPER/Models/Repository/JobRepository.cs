using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;
using System.Data.Entity;

namespace VIPER.Models.Repository
{
    public class JobRepository : IDisposable
    {
        private VIPERDbContext context;

        public JobRepository()
        {
            context = new VIPERDbContext();
        }

        public List<JobViewModel> Jobs
        {
            get
            {
                var jobs = context.Jobs.Include(j => j.RepairType).Include(j => j.Hour).Include(j => j.Size).Include(j => j.JobProcesses);
                 
                JobViewModel job; 
                List<JobViewModel> jobsViewModel = new List<JobViewModel>();

                foreach(Job j in jobs)
                {
                    job = new JobViewModel();
                    job.JobID = j.JobID;
                    job.VesselName = j.VesselName;
                    job.JobNumber = j.JobNumber;
                    job.Quantity = j.Quantity;
                    job.OpenDate = j.OpenDate;
                    if (j.InvoicedTotal != null && j.TotalJobCost.GetValueOrDefault() != null)
                        job.ActualProfit = j.InvoicedTotal - j.TotalJobCost.GetValueOrDefault();
                    else
                        job.ActualProfit = 0;
                    
                   
                    job.RepairTypeID = j.RepairTypeID.GetValueOrDefault();
                    if (j.RepairType != null)
                    {
                        job.RepairType = new RepairTypeViewModel
                        {
                            RepairTypeID = j.RepairType.RepairTypeID,
                            Name = j.RepairType.Name
                        };

                    }
                    else
                    {
                        job.RepairType = new RepairTypeViewModel
                        {
                            RepairTypeID = 0,
                            Name = ""
                        };
                    }
                    
                    job.SizeID = j.SizeID.GetValueOrDefault();
                    if (j.Size != null)
                    {
                        job.Size = new SizeViewModel
                        {
                            SizeID = j.Size.SizeID,
                            Name = j.Size.Name
                        };
                    }
                    else
                    {
                        job.Size = new SizeViewModel
                        {
                            SizeID = 0,
                            Name = ""
                        };
                    }
                    
                    job.HourID = j.HourID.GetValueOrDefault();
                    if (j.Hour != null)
                    {
                        job.Hour = new HourViewModel
                        {
                            HourID = j.Hour.HourID,
                            Name = j.Hour.Name
                        };
                    }
                    else
                    {
                        job.Hour = new HourViewModel
                        {
                            HourID = 0,
                            Name = ""
                        };
                    }
                    
                    jobsViewModel.Add(job);
                }

                return jobsViewModel;
            }
        }
       
        public void Create(JobViewModel j)
        {
            Job entity = new Job();
            entity.VesselName = j.VesselName;
            entity.JobNumber = j.JobNumber;
            entity.OpenDate = j.OpenDate.Date;
            
            entity.Quantity = j.Quantity;
            
            entity.RepairTypeID = j.RepairType.RepairTypeID;
            if (j.RepairType.RepairTypeID == 0)
                entity.RepairTypeID = null;
            entity.SizeID = j.Size.SizeID;
            if (j.Size.SizeID == 0)
                entity.SizeID = null;
            entity.HourID = j.Hour.HourID;
            if (j.Hour.HourID == 0)
                entity.HourID = null;
            entity.ActualProfit = 0;
            
            context.Jobs.Add(entity);
            
            List<Process> processes = context.Processes.OrderBy(p => p.ProcessID).ToList();

            List<ProcessTime> processTimes = context.ProcessTimes.Where(pt => pt.RepairTypeID == j.RepairType.RepairTypeID).Where(pt => pt.SizeID == j.Size.SizeID).OrderBy(pt => pt.ProcessID).ToList();

            JobProcess jp; 

            for (int i=0;i<processes.Count;i++)
            {
                jp = new JobProcess();
                jp.JobID = j.JobID;
                jp.ProcessID = processes[i].ProcessID;
                if (processTimes.Count == 0)
                    jp.PlannedTime = 0;
                else
                jp.PlannedTime = processTimes[i].PlannedTime * entity.Quantity;
                context.JobProcesses.Add(jp);
            }
            context.SaveChanges();
            j.JobID = entity.JobID;
        }

        public void Update(JobViewModel j)
        {
            Job entity = context.Jobs.FirstOrDefault(job => job.JobID == j.JobID);
            if (entity != null)
            {
                entity.VesselName = j.VesselName;
                entity.JobNumber = j.JobNumber;
                entity.OpenDate = j.OpenDate.Date;
                entity.Quantity = j.Quantity;
                entity.RepairTypeID = j.RepairType.RepairTypeID;
                entity.SizeID = j.Size.SizeID;
                entity.HourID = j.Hour.HourID;
                entity.ActualProfit = j.InvoicedTotal - j.TotalJobCost;
                
                context.SaveChanges();
            }
        }

        public void Destroy(JobViewModel j)
        {
            var entity = new Job();

            entity.JobID = j.JobID;

            context.Jobs.Attach(entity);

            context.Jobs.Remove(entity);

            var jobProcesses = context.JobProcesses.Where(jp => jp.JobID == entity.JobID);

            foreach(var jobProcess in jobProcesses)
            {
                context.JobProcesses.Remove(jobProcess);
            }

            context.SaveChanges();
        }

        public List<JobCostViewModel> GetJobCost(int jobID)
        {
            var job = context.Jobs.Where(j => j.JobID == jobID).Include(j => j.JobProcesses).ToList();

            List<JobCostViewModel> pivotJobs = new List<JobCostViewModel>();

            var pj = new JobCostViewModel();
            pj.PivotJobID = 1;
            pj.JobID = jobID;
            pj.CostType = "Spare Parts";
            pj.Planned = job[0].PlannedSparePartsCost;
            pj.Actual = job[0].SparePartsCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 2;
            pj.JobID = jobID;
            pj.CostType = "Third Party";
            pj.Planned = job[0].PlannedThirdPartyCost;
            pj.Actual = job[0].ThirdPartyCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 3;
            pj.JobID = jobID;
            pj.CostType = "Shipping";
            pj.Planned = job[0].PlannedShippingCost;
            pj.Actual = job[0].ShippingCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 4;
            pj.JobID = jobID;
            pj.CostType = "Duties";
            pj.Planned = job[0].PlannedDutiesCost;
            pj.Actual = job[0].DutiesCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            
            pj = new JobCostViewModel();
            pj.PivotJobID = 5;
            pj.JobID = jobID;
            pj.CostType = "Labor";
            pj.Planned = job[0].JobProcesses.Sum(jp => jp.PlannedTime) * 93;
            pj.Actual =  job[0].JobProcesses.Sum(jp => jp.ActualTime) * 93;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 6;
            pj.JobID = jobID;
            pj.CostType = "Packaging";
            pj.Planned = (106 * job[0].Quantity);
            pj.Actual = (106 * job[0].Quantity);
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 7;
            pj.JobID = jobID;
            pj.CostType = "Consumable";
            pj.Planned = ((pivotJobs[5].Planned + pivotJobs[4].Planned + job[0].PlannedDutiesCost + job[0].PlannedShippingCost + job[0].PlannedThirdPartyCost + job[0].PlannedSparePartsCost)) * ((Decimal)(0.0075));
            pj.Actual = ((pivotJobs[5].Actual + pivotJobs[4].Actual + job[0].DutiesCost + job[0].ShippingCost + job[0].ThirdPartyCost + job[0].SparePartsCost)) * ((Decimal)(0.0075));
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 8;
            pj.JobID = jobID;
            pj.CostType = "Total Cost";
            pj.Planned = pivotJobs[0].Planned + pivotJobs[1].Planned + pivotJobs[2].Planned + pivotJobs[3].Planned + pivotJobs[4].Planned + pivotJobs[5].Planned + pivotJobs[6].Planned;
            pj.Actual = job[0].TotalJobCost.GetValueOrDefault();//pivotJobs[0].Actual + pivotJobs[1].Actual + pivotJobs[2].Actual + pivotJobs[3].Actual + pivotJobs[4].Actual + pivotJobs[5].Actual + pivotJobs[6].Actual;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 9;
            pj.JobID = jobID;
            pj.CostType = "Invoiced Total";
            pj.Planned = 0;
            pj.Actual = job[0].InvoicedTotal;
            pj.Difference = 0;
            pivotJobs.Add(pj);

            //pj = new JobCostViewModel();
            //pj.PivotJobID = 9;
            //pj.JobID = jobID;
            //pj.CostType = "Profit";
            //pj.Planned = 0;
            //pj.Actual = pivotJobs[7].Actual - pivotJobs[6].Actual;
            //pj.Difference = 0;
            //pivotJobs.Add(pj);

            return pivotJobs;
        }

        public void UpdateJobCost(IList<JobCostViewModel> jobCost)
        {
            int jobID = jobCost[0].JobID;
            var jobs = context.Jobs.Where(j => j.JobID == jobID ).Include(j => j.JobProcesses).ToList();
            Job entity = jobs[0];

            if (entity != null)
            {
                foreach (JobCostViewModel jc in jobCost)
                {
                    if (jc.PivotJobID == 1)
                    {
                        entity.PlannedSparePartsCost = jc.Planned;
                        entity.SparePartsCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;
                    }
                    if (jc.PivotJobID == 2)
                    {
                        entity.PlannedThirdPartyCost = jc.Planned;
                        entity.ThirdPartyCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;
                    }
                    if (jc.PivotJobID == 3)
                    {
                        entity.PlannedShippingCost = jc.Planned;
                        entity.ShippingCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;
                    }
                    if (jc.PivotJobID == 4)
                    {
                        entity.PlannedDutiesCost = jc.Planned;
                        entity.DutiesCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;
                    }

                    if (jc.PivotJobID == 5)
                    {
                        jc.Planned = entity.JobProcesses.Sum(jp => jp.PlannedTime) * 93;
                        jc.Actual = entity.JobProcesses.Sum(jp => jp.ActualTime) * 93;
                        jc.Difference = jc.Planned - jc.Actual;
                    }

                    if (jc.PivotJobID == 6)
                    {
                        jc.Planned = 106 * entity.Quantity;
                        jc.Actual = 106 * entity.Quantity;
                        jc.Difference = jc.Planned - jc.Actual;
                    }

                    if (jc.PivotJobID == 7)
                    {
                        jc.Planned = (jobCost[0].Planned + jobCost[1].Planned + jobCost[2].Planned + jobCost[3].Planned + jobCost[4].Planned + jobCost[5].Planned) * (Decimal)0.0075;
                        jc.Actual = (jobCost[0].Actual + jobCost[1].Actual + jobCost[2].Actual + jobCost[3].Actual + jobCost[4].Actual + jobCost[5].Actual) * (Decimal)0.0075;
                        jc.Difference = jc.Planned - jc.Actual;
                    }

                   
                    if (jc.PivotJobID == 8)
                    {
                        jc.Planned = jobCost[0].Planned + jobCost[1].Planned + jobCost[2].Planned + jobCost[3].Planned + jobCost[4].Planned + jobCost[5].Planned + jobCost[6].Planned;
                        entity.TotalJobCost = jc.Actual = jobCost[0].Actual + jobCost[1].Actual + jobCost[2].Actual + jobCost[3].Actual + jobCost[4].Actual + jobCost[5].Actual + jobCost[6].Actual;
                        jc.Difference = jc.Planned - jc.Actual;
                    }


                    if (jc.PivotJobID == 9)
                    {
                        jc.Planned = 0;
                        entity.InvoicedTotal = jc.Actual;
                        jc.Difference = 0;
                    }
                    
                }

                context.SaveChanges();
            }
        }

        public List<JobScheduleViewModel> GetJobSchedule(int jobID)
        {
            var job =  context.Jobs.Find(jobID);
            
            var  jobSchedule =  new JobScheduleViewModel();

            jobSchedule.JobID = job.JobID;
            if (job.ReceivedDate.HasValue)
                jobSchedule.ReceivedDate = job.ReceivedDate.GetValueOrDefault();
            else
                jobSchedule.ReceivedDate = null;
            if (job.StartDate.HasValue)
                jobSchedule.StartDate = job.StartDate.GetValueOrDefault();
            else
                jobSchedule.StartDate = null;
            if (job.PromiseDate.HasValue)
                jobSchedule.PromiseDate = job.PromiseDate.GetValueOrDefault();
            else
                jobSchedule.PromiseDate = null;
            if (job.ShipDate.HasValue)
                jobSchedule.ShipDate = job.ShipDate.GetValueOrDefault();
            else
                jobSchedule.ShipDate = null;
            if (job.CompletionDate.HasValue)
                jobSchedule.CompletionDate = job.CompletionDate.GetValueOrDefault();
            else
                jobSchedule.CompletionDate = null;
            if (job.ShipDate.HasValue && job.ReceivedDate.HasValue)
                jobSchedule.TurnTime = (job.ShipDate.GetValueOrDefault() - job.ReceivedDate.GetValueOrDefault()).Days;
            else
                jobSchedule.TurnTime = 0;

            List<JobScheduleViewModel> result = new List<JobScheduleViewModel>();
            result.Add(jobSchedule);

            return result;
            
        }

        public void UpdateJobSchedule(JobScheduleViewModel j)
        {
            List<Job> jobs = context.Jobs.Where(job => job.JobID == j.JobID).Include(job => job.JobProcesses).Include("JobProcesses.Process").ToList();
            Job entity = jobs[0];

            if (entity != null)
            {
                if(j.ReceivedDate.HasValue)
                    entity.ReceivedDate = j.ReceivedDate.GetValueOrDefault();

                if (j.StartDate.HasValue)
                {
                    entity.StartDate = j.StartDate.GetValueOrDefault();
                    entity.JobSchedule = true;
                    DateTime nextStart = DateTime.Now;

                    foreach (JobProcess jp in entity.JobProcesses)
                    {  
                        if (jp.Process.Step == 1)
                            jp.Start = j.StartDate.GetValueOrDefault();
                        else
                            jp.Start = nextStart;

                        jp.End = nextStart = GetNextProcessStartTime(jp.Start, (Double)jp.PlannedTime);
                        if (jp.Process.Step == 7)
                            entity.CompletionDate = j.CompletionDate =  jp.End;    
                    }
                }

                if(j.ShipDate.HasValue)
                    entity.ShipDate = j.ShipDate.GetValueOrDefault();

                if(j.PromiseDate.HasValue)
                    entity.PromiseDate = j.PromiseDate.GetValueOrDefault();

                if(j.ShipDate.HasValue && j.ReceivedDate.HasValue)
                    j.TurnTime = (j.ShipDate.GetValueOrDefault() - j.ReceivedDate.GetValueOrDefault()).Days;
               
                context.SaveChanges();
            }
        }

        public List<JobProcessViewModel> GetJobProcesses(int jobID)
        {
            return context.JobProcesses.Where(jp => jp.JobID == jobID).Include(jp => jp.Process).Select(jp => new JobProcessViewModel
                {
                    JobProcessID = jp.JobProcessID,
                    JobID = jp.JobID,
                    ProcessID = jp.ProcessID,
                    ProcessName = jp.Process.Name,
                    PlannedTime = jp.PlannedTime,
                    ActualTime = jp.ActualTime,
                    Difference = jp.PlannedTime - jp.ActualTime,
                    ImageURL = (jp.PlannedTime - jp.ActualTime) > 0 ? "check-icon-red.png" : "check-icon-green.png",
                    ScheduleWeek = jp.ScheduleWeek,
                    Status = jp.Status
                }).ToList();
        }

        public void UpdateJobProcesses(JobProcessViewModel jobProcess)
        {
            var entity = context.JobProcesses.FirstOrDefault(jp => jp.JobProcessID == jobProcess.JobProcessID);
            if (entity != null)
            {
                entity.PlannedTime = jobProcess.PlannedTime;
                entity.ActualTime = jobProcess.ActualTime;
                entity.ScheduleWeek = jobProcess.ScheduleWeek;
                jobProcess.Difference = jobProcess.PlannedTime - jobProcess.ActualTime;
                context.SaveChanges();
            }
        }

        public DateTime GetNextProcessStartTime(DateTime nextStart, Double totalProcessTime)
        {
            IList<Holiday> holidays = context.Holidays.Where(h => h.Date.Year == DateTime.Now.Year).ToList();

            Double totalProcessDays = Math.Floor((totalProcessTime / 7.5));

            Double remainingHours = (totalProcessTime % 7.5);

            for (int i = 0; i < totalProcessDays; i++)
            {
                nextStart = nextStart.AddDays(1);
                if (nextStart.DayOfWeek == DayOfWeek.Saturday)
                    nextStart = nextStart.AddDays(2);
                foreach (Holiday h in holidays)
                {
                    if (nextStart.Date == h.Date.Date)
                        nextStart = nextStart.AddDays(1);
                }
            }

            DateTime previousStart = nextStart;

            nextStart = nextStart.AddHours(remainingHours);

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) < 0)
            {
                nextStart = nextStart.AddMinutes(15);
            }

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) < 0)
            {
                nextStart = nextStart.AddMinutes(30);
            }

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) < 0)
            {
                nextStart = nextStart.AddMinutes(15);
            }

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(15, 30, 0)) > 0)
            {
                var remainingProcessTime = nextStart.TimeOfDay.Subtract(new TimeSpan(15, 30, 0));
                nextStart = nextStart.Date.AddHours(31);

                previousStart = nextStart;

                nextStart = nextStart.AddHours(remainingProcessTime.TotalHours);

                if (nextStart.DayOfWeek == DayOfWeek.Saturday)
                    nextStart = nextStart.AddDays(2);

                foreach (Holiday h in holidays)
                {
                    if (nextStart.Date == h.Date.Date)
                        nextStart = nextStart.AddDays(1);
                }

                if (nextStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) < 0)
                {
                    nextStart = nextStart.AddMinutes(15);
                }

                if (nextStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) < 0)
                {
                    nextStart = nextStart.AddMinutes(30);
                }

                if (nextStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) < 0)
                {
                    nextStart = nextStart.AddMinutes(15);
                }
            }

            return nextStart;
        }

        public void Dispose()
        {
            context.Dispose();
        }
       
    }
}