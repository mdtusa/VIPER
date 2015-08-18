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

        bool disposed = false;

        public JobRepository()
        {
            context = new VIPERDbContext();
        }

        public List<JobViewModel> Jobs
        {
            get
            {
                var jobs = context.Jobs.Include(j => j.RepairType).Include(j => j.Size).Include(j => j.JobProcesses).OrderByDescending(j => j.JobID).ToList();
                 
                JobViewModel job; 
                List<JobViewModel> jobsViewModel = new List<JobViewModel>();
                Decimal totalPlannedHours, totalActualHours, completedProcessHours;

                foreach(Job j in jobs)
                {
                    job = new JobViewModel();
                    job.JobID = j.JobID;
                    job.VesselName = j.VesselName;
                    job.JobNumber = j.JobNumber;
                    job.Quantity = j.Quantity;
                    job.OpenDate = j.OpenDate;
                    totalPlannedHours = j.JobProcesses.Sum(jp => jp.PlannedTime);
                    totalActualHours = j.JobProcesses.Sum(jp => jp.ActualTime);
                    completedProcessHours = j.JobProcesses.Where(jp => jp.Status == (int)JobStatus.Completed).Sum(jp => jp.PlannedTime);
                    if (totalPlannedHours == 0)
                        job.PercentComplete = 0;
                    else
                        job.PercentComplete = Convert.ToInt32(Math.Round((completedProcessHours / totalPlannedHours) * 100));

                    if (j.Status.HasValue)
                        job.Status = j.Status.Value;
                    else
                        job.Status = (int)JobStatus.NoStatus;

                    if (j.InvoicedTotal.HasValue && j.TotalJobCost.HasValue && j.TotalJobCost != 0)
                    {
                        job.ActualProfit = j.InvoicedTotal.Value - j.TotalJobCost.Value;
                        job.Margin = (job.ActualProfit / j.TotalJobCost.Value) * 100;
                    }
                    else
                    {
                        job.ActualProfit = 0;
                        job.Margin = 0;
                    }

                    if (totalActualHours == 0)
                        job.Efficiency = 0;
                    else
                        job.Efficiency = (totalPlannedHours / totalActualHours) * 100;

                    job.RepairTypeID = j.RepairTypeID.GetValueOrDefault();

                    if (j.RepairType != null)
                    {
                        job.RepairType = new RepairType
                        {
                            RepairTypeID = j.RepairType.RepairTypeID,
                            Name = j.RepairType.Name
                        };

                    }
                    else
                    {
                        job.RepairType = new RepairType
                        {
                            RepairTypeID = 0,
                            Name = ""
                        };
                    }
                    
                    job.SizeID = j.SizeID.GetValueOrDefault();

                    if (j.Size != null)
                    {
                        job.Size = new Size
                        {
                            SizeID = j.Size.SizeID,
                            Name = j.Size.Name
                        };
                    }
                    else
                    {
                        job.Size = new Size
                        {
                            SizeID = 0,
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
            entity.Status = (int)JobStatus.NoStatus;
            entity.RepairTypeID = j.RepairType.RepairTypeID;
            
            if (j.RepairType.RepairTypeID == 0)
                entity.RepairTypeID = null;

            entity.SizeID = j.Size.SizeID;

            if (j.Size.SizeID == 0)
                entity.SizeID = null;

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
            Job entity = context.Jobs.Find(j.JobID);
            if (entity != null)
            {
                entity.VesselName = j.VesselName;
                entity.JobNumber = j.JobNumber;
                entity.OpenDate = j.OpenDate.Date;
                entity.Quantity = j.Quantity;
                entity.RepairTypeID = j.RepairType.RepairTypeID;
                entity.SizeID = j.Size.SizeID;
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
            var job = context.Jobs.Include(j => j.JobProcesses).First(j => j.JobID == jobID);

            List<JobCostViewModel> pivotJobs = new List<JobCostViewModel>();

            var pj = new JobCostViewModel();
            pj.PivotJobID = 1;
            pj.JobID = jobID;
            pj.CostType = "Spare Parts";
            pj.Planned = job.PlannedSparePartsCost;
            pj.Actual = job.SparePartsCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 2;
            pj.JobID = jobID;
            pj.CostType = "Third Party";
            pj.Planned = job.PlannedThirdPartyCost;
            pj.Actual = job.ThirdPartyCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 3;
            pj.JobID = jobID;
            pj.CostType = "Shipping";
            pj.Planned = job.PlannedShippingCost;
            pj.Actual = job.ShippingCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 4;
            pj.JobID = jobID;
            pj.CostType = "Duties";
            pj.Planned = job.PlannedDutiesCost;
            pj.Actual = job.DutiesCost;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            
            pj = new JobCostViewModel();
            pj.PivotJobID = 5;
            pj.JobID = jobID;
            pj.CostType = "Labor";
            pj.Planned = job.JobProcesses.Sum(jp => jp.PlannedTime) * 93;
            pj.Actual =  job.JobProcesses.Sum(jp => jp.ActualTime) * 93;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 6;
            pj.JobID = jobID;
            pj.CostType = "Packaging";
            if (job.PlannedPackaging == 0 || job.PlannedPackaging == null)
                pj.Planned = (106 * job.Quantity);
            else
                pj.Planned = job.PlannedPackaging.Value;

            if (job.PackagingCost == 0 || job.PackagingCost == null)
                pj.Actual = (106 * job.Quantity);
            else
                pj.Actual = job.PackagingCost.Value;
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 7;
            pj.JobID = jobID;
            pj.CostType = "Consumable";
            pj.Planned = ((pivotJobs[4].Planned + job.PlannedDutiesCost + job.PlannedShippingCost + job.PlannedThirdPartyCost + job.PlannedSparePartsCost)) * ((Decimal)(0.0075));
            pj.Actual = ((pivotJobs[4].Actual + job.DutiesCost + job.ShippingCost + job.ThirdPartyCost + job.SparePartsCost)) * ((Decimal)(0.0075));
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 8;
            pj.JobID = jobID;
            pj.CostType = "Total Cost";
            pj.Planned = pivotJobs[0].Planned + pivotJobs[1].Planned + pivotJobs[2].Planned + pivotJobs[3].Planned + pivotJobs[4].Planned + pivotJobs[5].Planned + pivotJobs[6].Planned;
            pj.Actual = job.TotalJobCost.GetValueOrDefault();
            pj.Difference = pj.Planned - pj.Actual;
            pivotJobs.Add(pj);

            pj = new JobCostViewModel();
            pj.PivotJobID = 9;
            pj.JobID = jobID;
            pj.CostType = "Invoiced Total";
            pj.Planned = 0;
            pj.Actual = job.InvoicedTotal.GetValueOrDefault();
            pj.Difference = 0;
            pivotJobs.Add(pj);

            return pivotJobs;
        }

        public void UpdateJobCost(IList<JobCostViewModel> jobCost)
        {
            int jobID = jobCost[0].JobID;
            var job = context.Jobs.Include(j => j.JobProcesses).FirstOrDefault(j => j.JobID == jobID );

            if (job != null)
            {
                foreach (JobCostViewModel jc in jobCost)
                {
                    if (jc.PivotJobID == 1)
                    {
                        job.PlannedSparePartsCost = jc.Planned;
                        job.SparePartsCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;

                        var labor = job.JobProcesses.Sum(jp => jp.ActualTime) * 93;
                        //var plannedLabor = job.JobProcesses.Sum(jp => jp.PlannedTime) * 93;

                        var subTotal = (job.SparePartsCost + job.ThirdPartyCost + job.ShippingCost + job.DutiesCost  + labor);
                        //var plannedSubTotal = (job.PlannedSparePartsCost + job.PlannedThirdPartyCost + job.PlannedShippingCost + job.PlannedDutiesCost + job.PlannedPackaging + plannedLabor);
                       
                        var consumable = subTotal * (Decimal)0.0075;
                        //var plannedConsumable = plannedSubTotal * (Decimal)0.0075;
                        if (job.PackagingCost == 0 || job.PackagingCost == null)
                            job.PackagingCost = job.Quantity * 106;

                        job.TotalJobCost = subTotal + consumable + job.PackagingCost;
                    }
                    else if (jc.PivotJobID == 2)
                    {
                        job.PlannedThirdPartyCost = jc.Planned;
                        job.ThirdPartyCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;

                        var labor = job.JobProcesses.Sum(jp => jp.ActualTime) * 93;
                        //var plannedLabor = job.JobProcesses.Sum(jp => jp.PlannedTime) * 93;

                        var subTotal = (job.SparePartsCost + job.ThirdPartyCost + job.ShippingCost + job.DutiesCost + labor);
                        //var plannedSubTotal = (job.PlannedSparePartsCost + job.PlannedThirdPartyCost + job.PlannedShippingCost + job.PlannedDutiesCost + job.PlannedPackaging + plannedLabor);

                        var consumable = subTotal * (Decimal)0.0075;
                        //var plannedConsumable = plannedSubTotal * (Decimal)0.0075;

                        if (job.PackagingCost == 0 || job.PackagingCost == null)
                            job.PackagingCost = job.Quantity * 106;

                        job.TotalJobCost = subTotal + consumable + job.PackagingCost;
                    }
                    else if (jc.PivotJobID == 3)
                    {
                        job.PlannedShippingCost = jc.Planned;
                        job.ShippingCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;

                        var labor = job.JobProcesses.Sum(jp => jp.ActualTime) * 93;
                        //var plannedLabor = job.JobProcesses.Sum(jp => jp.PlannedTime) * 93;

                        var subTotal = (job.SparePartsCost + job.ThirdPartyCost + job.ShippingCost + job.DutiesCost  + labor);
                        //var plannedSubTotal = (job.PlannedSparePartsCost + job.PlannedThirdPartyCost + job.PlannedShippingCost + job.PlannedDutiesCost + job.PlannedPackaging + plannedLabor);
                        
                        var consumable = subTotal * (Decimal)0.0075;
                        //var plannedConsumable = plannedSubTotal * (Decimal)0.0075;

                        if (job.PackagingCost == 0 || job.PackagingCost == null)
                            job.PackagingCost = job.Quantity * 106;

                        job.TotalJobCost = subTotal + consumable + job.PackagingCost;
                    }
                    else if (jc.PivotJobID == 4)
                    {
                        job.PlannedDutiesCost = jc.Planned;
                        job.DutiesCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;

                        var labor = job.JobProcesses.Sum(jp => jp.ActualTime) * 93;
                        //var plannedLabor = job.JobProcesses.Sum(jp => jp.PlannedTime) * 93;

                        var subTotal = (job.SparePartsCost + job.ThirdPartyCost + job.ShippingCost + job.DutiesCost + labor);
                        //var plannedSubTotal = (job.PlannedSparePartsCost + job.PlannedThirdPartyCost + job.PlannedShippingCost + job.PlannedDutiesCost + job.PlannedPackaging + plannedLabor);

                        var consumable = subTotal * (Decimal)0.0075;
                        //var plannedConsumable = plannedSubTotal * (Decimal)0.0075;

                        if (job.PackagingCost == 0 || job.PackagingCost == null)
                            job.PackagingCost = job.Quantity * 106;

                        job.TotalJobCost = subTotal + consumable + job.PackagingCost;
                    }
                    else if (jc.PivotJobID == 6)
                    {
                        job.PlannedPackaging = jc.Planned;
                        job.PackagingCost = jc.Actual;
                        jc.Difference = jc.Planned - jc.Actual;

                        var labor = job.JobProcesses.Sum(jp => jp.ActualTime) * 93;
                        //var plannedLabor = job.JobProcesses.Sum(jp => jp.PlannedTime) * 93;

                        var subTotal = (job.SparePartsCost + job.ThirdPartyCost + job.ShippingCost + job.DutiesCost + labor);
                        //var plannedSubTotal = (job.PlannedSparePartsCost + job.PlannedThirdPartyCost + job.PlannedShippingCost + job.PlannedDutiesCost + job.PlannedPackaging + plannedLabor);

                        var consumable = subTotal * (Decimal)0.0075;
                        //var plannedConsumable = plannedSubTotal * (Decimal)0.0075;

                        job.TotalJobCost = subTotal + consumable + jc.Actual;
                    }
                    else if (jc.PivotJobID == 9)
                    {
                        jc.Planned = 0;
                        job.InvoicedTotal = jc.Actual;
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
           
            if (job.StartDate.HasValue)
                jobSchedule.StartDate = job.StartDate.GetValueOrDefault();
           
            if (job.PromiseDate.HasValue)
                jobSchedule.PromiseDate = job.PromiseDate.GetValueOrDefault();
            
            if (job.ShipDate.HasValue)
                jobSchedule.ShipDate = job.ShipDate.GetValueOrDefault();
           
            if (job.CompletionDate.HasValue)
                jobSchedule.CompletionDate = job.CompletionDate.GetValueOrDefault();
           
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
            var job = context.Jobs.Include(x => x.JobProcesses).Include("JobProcesses.Process").FirstOrDefault(x => x.JobID == j.JobID);

            if (job != null)
            {
                if(j.ReceivedDate.HasValue)
                    job.ReceivedDate = j.ReceivedDate.Value;

                if (j.StartDate.HasValue && !job.StartDate.HasValue)
                {
                    job.StartDate = j.StartDate.Value;
                    job.PromiseDate = job.StartDate.Value.AddDays(45.00);
                }
                else if (j.StartDate.HasValue)
                    job.StartDate = j.StartDate.Value;

                if(j.ShipDate.HasValue)
                    job.ShipDate = j.ShipDate.Value;

                if (j.CompletionDate.HasValue)
                    job.CompletionDate = j.CompletionDate.Value;

                if (j.PromiseDate.HasValue)
                    job.PromiseDate = j.PromiseDate.Value;
                     
                if(j.ShipDate.HasValue && j.ReceivedDate.HasValue)
                    j.TurnTime = (j.ShipDate.Value - j.ReceivedDate.Value).Days;
               
                context.SaveChanges();
            }
        }

        public List<JobProcessViewModel> GetJobProcesses(int jobID)
        {
            var entities = context.JobProcesses.Where(jp => jp.JobID == jobID).Include(jp => jp.Process).ToList();

            List<JobProcessViewModel> jobProcesses = new List<JobProcessViewModel>();
            JobProcessViewModel jobProcess;

            foreach(var jp in entities)
            {
                jobProcess = new JobProcessViewModel();
                jobProcess.JobProcessID = jp.JobProcessID;
                jobProcess.JobID = jp.JobID;
                jobProcess.Status = jp.Status;
                jobProcess.ProcessID = jp.ProcessID;
                jobProcess.ProcessName = jp.Process.Name;
                jobProcess.PlannedTime = jp.PlannedTime;
                jobProcess.ActualTime = jp.ActualTime;
                jobProcess.Note = jp.Note;
                jobProcess.Difference = jp.PlannedTime - jp.ActualTime;
                jobProcess.ReworkTime = jp.ReworkTime;
                jobProcess.ScheduleWeek = jp.ScheduleWeek;

                jobProcesses.Add(jobProcess);
            }
            return jobProcesses;
        }

        public void UpdateJobProcesses(JobProcessViewModel jobProcess)
        {
            var entity = context.JobProcesses.Find(jobProcess.JobProcessID);
            if (entity != null)
            {
                entity.PlannedTime = jobProcess.PlannedTime;
                entity.ActualTime = jobProcess.ActualTime;
                entity.ReworkTime = jobProcess.ReworkTime;
                entity.Note = jobProcess.Note;
                entity.ScheduleWeek = jobProcess.ScheduleWeek;
                jobProcess.Difference = jobProcess.PlannedTime - jobProcess.ActualTime;
                context.SaveChanges();
            }
        }

        //public DateTime GetNextProcessStartTime(DateTime nextStart, Double totalProcessTime)
        //{
        //    IList<Holiday> holidays = context.Holidays.Where(h => h.Date.Year == DateTime.Now.Year).ToList();

        //    Double totalProcessDays = Math.Floor((totalProcessTime / 7.5));

        //    Double remainingHours = (totalProcessTime % 7.5);

        //    for (int i = 0; i < totalProcessDays; i++)
        //    {
        //        nextStart = nextStart.AddDays(1);
        //        if (nextStart.DayOfWeek == DayOfWeek.Saturday)
        //            nextStart = nextStart.AddDays(2);
        //        foreach (Holiday h in holidays)
        //        {
        //            if (nextStart.Date == h.Date.Date)
        //                nextStart = nextStart.AddDays(1);
        //        }
        //    }

        //    DateTime previousStart = nextStart;

        //    nextStart = nextStart.AddHours(remainingHours);

        //    if (nextStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) < 0)
        //    {
        //        nextStart = nextStart.AddMinutes(15);
        //    }

        //    if (nextStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) < 0)
        //    {
        //        nextStart = nextStart.AddMinutes(30);
        //    }

        //    if (nextStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) < 0)
        //    {
        //        nextStart = nextStart.AddMinutes(15);
        //    }

        //    if (nextStart.TimeOfDay.CompareTo(new TimeSpan(15, 30, 0)) > 0)
        //    {
        //        var remainingProcessTime = nextStart.TimeOfDay.Subtract(new TimeSpan(15, 30, 0));
        //        nextStart = nextStart.Date.AddHours(31);

        //        previousStart = nextStart;

        //        nextStart = nextStart.AddHours(remainingProcessTime.TotalHours);

        //        if (nextStart.DayOfWeek == DayOfWeek.Saturday)
        //            nextStart = nextStart.AddDays(2);

        //        foreach (Holiday h in holidays)
        //        {
        //            if (nextStart.Date == h.Date.Date)
        //                nextStart = nextStart.AddDays(1);
        //        }

        //        if (nextStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) < 0)
        //        {
        //            nextStart = nextStart.AddMinutes(15);
        //        }

        //        if (nextStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) < 0)
        //        {
        //            nextStart = nextStart.AddMinutes(30);
        //        }

        //        if (nextStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) < 0)
        //        {
        //            nextStart = nextStart.AddMinutes(15);
        //        }
        //    }

        //    return nextStart;
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                context.Dispose();
                // Free any other managed objects here. 
            }

            // Free any unmanaged objects here. 
            disposed = true;
        }

        public void UpdateJobStatus(int status, int jobID)
        {
            var job = context.Jobs.Find(jobID);

            if(job != null)
            {
                job.Status = status;
                context.SaveChanges();
            }
        }

        public void UpdateJobProcessStatus(int status, int jobProcessID)
        {
            var jobProcess = context.JobProcesses.Find(jobProcessID);

            if (jobProcess != null)
            {
                jobProcess.Status = status;
                context.SaveChanges();
            }
        }
    }
}