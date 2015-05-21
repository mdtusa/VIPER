using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.ViewModel;
using System.Data.Entity;
using VIPER.Models.Entities;
using System.Web.Mvc;

namespace VIPER.Models.Repository
{
    public class ScheduleRepository
    {
        private VIPERDbContext context;

        public ScheduleRepository()
        {
            context = new VIPERDbContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IQueryable<ScheduleViewModel> GetAllJobs()
        {
            ChangeJobScheduleStatus();
            IList<Job> jobs = context.Jobs.Include(j => j.JobProcesses).Include("JobProcesses.Process").Where(j => j.JobSchedule == true).ToList();
            int parentID = 0,currentID,previousID = 0 ;

            foreach (Job j in jobs)
            {
                foreach (JobProcess jp in j.JobProcesses)
                {
                    currentID = jp.JobProcessID;
                    if (currentID > previousID)
                        parentID = currentID;

                    previousID = currentID;
                }
            }

            ScheduleViewModel scheduleParent,schedule;
            List<ScheduleViewModel> scheduleViewModel = new List<ScheduleViewModel>();
                        
            foreach(Job j in jobs)
            {
                scheduleParent = new ScheduleViewModel();
                scheduleParent.TaskID = parentID++;
                
                scheduleParent.JobID = j.JobID;
                scheduleParent.Title = j.VesselName;
                scheduleParent.Summary = true;
                scheduleParent.Expanded = false;
                scheduleParent.OrderId = 0;
                scheduleParent.ParentID = null;
                scheduleParent.Start = j.StartDate.GetValueOrDefault();
                scheduleParent.End = j.CompletionDate.GetValueOrDefault();
                scheduleParent.SchedulePriority = 0;
                scheduleViewModel.Add(scheduleParent);
                      
                foreach(JobProcess jp in j.JobProcesses)
                {
                    schedule = new ScheduleViewModel();
                    schedule.TaskID = jp.JobProcessID;
                    
                    schedule.ParentID = parentID - 1;
                    schedule.JobID = j.JobID;
                    schedule.Title = jp.Process.Name;
                    schedule.Summary = true;
                    schedule.Expanded = false;
                    schedule.OrderId = jp.Process.Step;
                    schedule.Start = jp.Start;
                    schedule.End = jp.End;
                    
                    scheduleViewModel.Add(schedule);
                }

            }
            return scheduleViewModel.AsQueryable();
        }


        public void ChangeJobScheduleStatus()
        {
            var jobs = context.Jobs.Include(j => j.JobProcesses).Include("JobProcesses.Process").Where(j => j.JobSchedule == true);
            foreach(var job in jobs)
            {
                if (job.PromiseDate < DateTime.Today)
                {
                    job.JobSchedule = false;
                }
            }
            context.SaveChanges();
        }


        public virtual void Update(ScheduleViewModel task, ModelStateDictionary modelState)
        {
            if (task.ParentID != null && ValidateModel(task, modelState))
            {
                var jobs = context.Jobs.Include(j => j.JobProcesses).Include("JobProcesses.Process").Where(j => j.JobID == task.JobID).ToList();
                var job = jobs[0];
                var processesToBeEdited = job.JobProcesses.Where(jp => jp.Process.Step >= task.OrderId).OrderBy(jp => jp.Process.Step);
                DateTime nextStart = DateTime.Now;

                foreach (JobProcess jp in processesToBeEdited)
                {
                    if(jp.Process.Step == task.OrderId)
                    {
                        jp.Start = task.Start;
                        jp.End = nextStart = task.End;
                        if (jp.Process.Step == 1)
                            job.StartDate = jp.Start;
                    }
                    else
                    {
                        jp.Start = nextStart;
                        jp.End = nextStart = GetNextProcessStartTime(jp.Start, (Double)jp.PlannedTime);
                        if (jp.Process.Step == 7)
                            job.CompletionDate = jp.End;
                    }
                    
                }
                context.SaveChanges();
            }
        }
        
        private bool ValidateModel(ScheduleViewModel task, ModelStateDictionary modelState)
        {
            if (task.Start > task.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
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

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(9, 0, 0)) <= 0)
            {
                nextStart = nextStart.AddMinutes(15);
            }

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(12, 0, 0)) <= 0)
            {
                nextStart = nextStart.AddMinutes(30);
            }

            if (nextStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) >= 0 && previousStart.TimeOfDay.CompareTo(new TimeSpan(14, 0, 0)) <= 0)
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

        public Double GetTotalProcessHours(DateTime start, DateTime end)
        {
            Double totalHours= 0.00;
           
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1) )
            {
                if (i == start.Date)
                {
                    totalHours = new TimeSpan(15, 30, 0).Subtract(start.TimeOfDay).TotalHours;

                    if (totalHours > 6.50)
                        totalHours = totalHours - 1.00;
                    else if (totalHours > 3.50)
                        totalHours = totalHours - 0.75;
                    else if (totalHours > 1.50)
                        totalHours = totalHours - 0.25;
                }
                else if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                    totalHours = totalHours + 7.5;
                else if(i == end.Date)
                {
                    Double remainingHours = end.TimeOfDay.Subtract(new TimeSpan(7, 0, 0)).TotalHours;

                    if (remainingHours > 7.00)
                        remainingHours = remainingHours - 1.00;
                    else if(remainingHours > 5.00)
                        remainingHours = remainingHours - 0.75;
                    else if(remainingHours > 2.00)
                        remainingHours = remainingHours - 0.25;

                    totalHours = totalHours + remainingHours;
                }
                
            }

            return totalHours;
        }
    }
}