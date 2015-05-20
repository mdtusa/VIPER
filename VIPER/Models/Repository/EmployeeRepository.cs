﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.ViewModel;
using System.Data.Entity;
using VIPER.Models.Entities;
using System.Web.Mvc;

namespace VIPER.Models.Repository
{
    public class EmployeeRepository : IDisposable
    {
         private VIPERDbContext context;

         public EmployeeRepository()
        {
            context = new VIPERDbContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        
        public virtual IQueryable<EmployeeViewModel> GetAll()
        {
            return context.Employees.Select(emp => new EmployeeViewModel
            {
                ID = emp.EmployeeID,
                Name = emp.Name,
                Color = emp.Type == "Person"? "Red" : "Green"
            });
        }

        public virtual IQueryable<EmployeeViewModel> GetEmployee()
        {
            return context.Employees.Where(e => e.Type == "Person").Select(emp => new EmployeeViewModel
            {
                ID = emp.EmployeeID,
                Name = emp.Name,
                Color = "#56ca85"
            });
        }

        public virtual IQueryable<EmployeeViewModel> GetArea()
        {
            return context.Employees.Where(e => e.Type == "Area").Select(emp => new EmployeeViewModel
            {
                ID = emp.EmployeeID,
                Name = emp.Name,
                Color = "#56ca85"
            });
        }

        internal IQueryable<EmployeeProcessViewModel> GetAssignment()
        {
            return context.EmployeeProcesses.Include(ep => ep.JobProcess).Include("JobProcess.Job").Where(ep => ep.JobProcess.Job.JobSchedule == true).Select( ep => new EmployeeProcessViewModel
            {
                EmployeeProcessID = ep.EmployeeProcessID,
                EmployeeID = ep.EmployeeID,
                JobProcessID = ep.JobProcessID,
                Units = (decimal)1.00
            });
        }

        internal void Delete(EmployeeProcessViewModel assignment)
        {  
            var entity = new EmployeeProcess();

            entity.EmployeeProcessID = assignment.EmployeeProcessID;

            context.EmployeeProcesses.Attach(entity);
            context.EmployeeProcesses.Remove(entity);
            context.SaveChanges();
        }

        internal void Insert(EmployeeProcessViewModel assignment, ModelStateDictionary modelState)
        {
            if (IsEmployeeAvailable(assignment, modelState))
            {
                var entity = new EmployeeProcess();

                entity.JobProcessID = assignment.JobProcessID;
                entity.EmployeeID = assignment.EmployeeID;

                context.EmployeeProcesses.Add(entity);
                context.SaveChanges();

                assignment.EmployeeProcessID = entity.EmployeeProcessID;
            }
        }

        internal void Update(EmployeeProcessViewModel assignment)
        {
            var entity = new EmployeeProcess();

            entity.EmployeeProcessID = assignment.EmployeeProcessID;
           
            context.EmployeeProcesses.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        private Boolean IsEmployeeAvailable(EmployeeProcessViewModel assignment, ModelStateDictionary modelState)
        {
            var employeeProcesses = context.EmployeeProcesses.Include(ep => ep.Employee).Include(ep => ep.JobProcess).Include("JobProcess.Job").Where(ep => ep.EmployeeID == assignment.EmployeeID).Where(ep => ep.JobProcess.Job.JobSchedule == true);

            var newJobProcess = context.JobProcesses.Find(assignment.JobProcessID);

            foreach( EmployeeProcess empProc in employeeProcesses)
            {
                if (((newJobProcess.Start > empProc.JobProcess.Start) && (newJobProcess.Start < empProc.JobProcess.End)) || ((newJobProcess.End > empProc.JobProcess.Start) && (newJobProcess.End < empProc.JobProcess.End)))
                {
                    modelState.AddModelError("errors", "Amin");
                    return false;
                }
            }

            return true;
        }

        internal IQueryable<CalendarViewModel> GetEmployeeCalendar()
        {
            var employeeProcesses = context.EmployeeProcesses.Include(ep => ep.Employee).Include(ep => ep.JobProcess).Include("JobProcess.Job").Include("JobProcess.Process").Where(ep => ep.JobProcess.Job.JobSchedule == true).Where(ep => ep.Employee.Type == "Person");

            List<CalendarViewModel> calendarViewModel = new List<CalendarViewModel>();
            CalendarViewModel entity;

            foreach(EmployeeProcess empProc in employeeProcesses)
            {
                entity = new CalendarViewModel();
                entity.TaskID = empProc.EmployeeProcessID;
                entity.Title = empProc.JobProcess.Job.VesselName + " - " + empProc.JobProcess.Process.Name;
                entity.Start = empProc.JobProcess.Start;
                entity.End = empProc.JobProcess.End;
                entity.OwnerID = empProc.EmployeeID;

                calendarViewModel.Add(entity);
            }

            return calendarViewModel.AsQueryable();
        }


        internal IQueryable<CalendarViewModel> GetAreaCalendar()
        {
            var employeeProcesses = context.EmployeeProcesses.Include(ep => ep.Employee).Include(ep => ep.JobProcess).Include("JobProcess.Job").Include("JobProcess.Process").Where(ep => ep.JobProcess.Job.JobSchedule == true).Where(ep => ep.Employee.Type == "Area");

            List<CalendarViewModel> calendarViewModel = new List<CalendarViewModel>();
            CalendarViewModel entity;

            foreach (EmployeeProcess empProc in employeeProcesses)
            {
                entity = new CalendarViewModel();
                entity.TaskID = empProc.EmployeeProcessID;
                entity.Title = empProc.JobProcess.Job.VesselName + " - " + empProc.JobProcess.Process.Name;
                entity.Start = empProc.JobProcess.Start;
                entity.End = empProc.JobProcess.End;
                entity.OwnerID = empProc.EmployeeID;

                calendarViewModel.Add(entity);
            }

            return calendarViewModel.AsQueryable();
        }
    }
}