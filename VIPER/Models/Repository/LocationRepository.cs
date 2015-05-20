using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;

namespace VIPER.Models.Repository
{
    public class LocationRepository : IDisposable
    {
        private VIPERDbContext context;

        public LocationRepository()
        {
            context = new VIPERDbContext();
        }

        public List<Location> Locations
        {
            get
            {
                return context.Locations.ToList();
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}