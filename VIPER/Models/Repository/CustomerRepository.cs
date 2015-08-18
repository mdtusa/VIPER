using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;

namespace VIPER.Models.Repository
{
    public class CustomerRepository : IDisposable
    {
        private VIPERDbContext context;
        bool disposed = false;

        public CustomerRepository()
        {
            context = new VIPERDbContext();
        }

        public IList<Customer> Customers
        {
            get
            {
                return context.Customers.ToList();
            }
        }

        public void Create(Customer c)
        {
            var entity = new Customer();
            entity.Name = c.Name;
            context.Customers.Add(entity);
            context.SaveChanges();
            c.CustomerID = entity.CustomerID;
        }

        public void Update(Customer c)
        {
            Customer entity = context.Customers.Find(c.CustomerID);
            if (entity != null)
                entity.Name = c.Name;
            context.SaveChanges();
        }

        public void Destroy(Customer c)
        {
            Customer entity = context.Customers.Find(c.CustomerID);
            if (entity != null)
                context.Customers.Remove(entity);
            context.SaveChanges();
        }

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
    }
}