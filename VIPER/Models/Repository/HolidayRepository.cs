using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;

namespace VIPER.Models.Repository
{
    public class HolidayRepository : IDisposable
    {
        private VIPERDbContext context;

        public HolidayRepository()
        {
            this.context = new VIPERDbContext();
        }

        public IList<HolidayViewModel> Holidays
        {
            get
            {
                return context.Holidays.Select(h => new HolidayViewModel()
                {
                    HolidayID = h.HolidayID,
                    Name = h.Name,
                    Date = h.Date
                }).ToList();
            }
        }

        public void Create(HolidayViewModel h)
        {
            var entity = new Holiday();
            entity.Name = h.Name;
            entity.Date = h.Date;
            context.Holidays.Add(entity);
            context.SaveChanges();
            h.HolidayID = entity.HolidayID;
        }

        public void Update(HolidayViewModel h)
        {
            Holiday entity = context.Holidays.Find(h.HolidayID);
            if (entity != null)
            {
                entity.Name = h.Name;
                entity.Date = h.Date;
            }
            context.SaveChanges();
        }

        public void Destroy(HolidayViewModel h)
        {
            Holiday entity = context.Holidays.Find(h.HolidayID);
            if (entity != null)
                context.Holidays.Remove(entity);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}