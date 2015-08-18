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

        public CustomerRepository()
        {
            context = new VIPERDbContext();
        }

        public List<Customer> Customers
        {
            get
            {
                return context.Customers.ToList();
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}