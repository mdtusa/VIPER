﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VIPER.Models.Entities;

namespace VIPER.Models
{
    public class VIPERDbContext : DbContext
    {
        public VIPERDbContext() 
        { 
            this.Configuration.ProxyCreationEnabled = false; 
        } 

        public DbSet<Job> Jobs { get; set; }

        public DbSet<AllJob> AllJobs { get; set; }

        public DbSet<Technician> Technicians { get; set; }

        public DbSet<MarketCategory> MarketCategories { get; set; }

        public DbSet<Market> Markets { get; set; }

        public DbSet<Motor> Motors { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Process> Processes { get; set; }

        public DbSet<RepairType> RepairTypes { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<JobProcess> JobProcesses { get; set; }

        public DbSet<ProcessTime> ProcessTimes { get; set; }

        public DbSet<EmployeeProcess> EmployeeProcesses { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<KPI> KPIs { get; set; }

        public DbSet<TechServicesKPI> TechServicesKPIs { get; set; }

        public DbSet<Month> Months { get; set; }

        public DbSet<TechnicianType> TechnicianTypes { get; set; }

        public DbSet<EngineerUtilization> EngineerUtilizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Job>().Property(j => j.ReceivedDate).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.ShipDate).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.StartDate).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.CompletionDate).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.PromiseDate).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.SchedulePriority).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.PackagingCost).IsOptional();
            modelBuilder.Entity<Job>().Property(j => j.PlannedPackaging).IsOptional();
            modelBuilder.Entity<JobProcess>().Property(j => j.ReworkTime).IsOptional();


        }
    }
}