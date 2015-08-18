using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;

namespace VIPER.Models.Repository
{
    public class HourRepository : IDisposable
    {
        private VIPERDbContext context;

        public HourRepository()
        {
            this.context = new VIPERDbContext();
        }

        public IList<HourViewModel> Hour
        {
            get
            {
                return context.Hours.Select(h => new HourViewModel() { 
                    HourID = h.HourID,
                    Name = h.Name
                }).ToList();
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}