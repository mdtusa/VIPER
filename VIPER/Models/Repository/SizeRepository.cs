using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;

namespace VIPER.Models.Repository
{
    public class SizeRepository : IDisposable
    {
        private VIPERDbContext context;

        public SizeRepository()
        {
            context = new VIPERDbContext();
        }

        public IList<Size> Sizes
        {
            get
            {
                return context.Sizes.Include(s => s.RepairType).ToList();
            }
        }

        public void Create(Size s)
        {
            var size = new Size();
            size.Name = s.Name;
            size.RepairTypeID = s.RepairType.RepairTypeID;
            context.Sizes.Add(size);
            context.SaveChanges();
            s.SizeID = size.SizeID;
        }

        public void Update(Size s)
        {
            Size entity = context.Sizes.Find(s.SizeID);
            if (entity != null)
            {
                entity.Name = s.Name;
                entity.RepairTypeID = s.RepairType.RepairTypeID;
            }
            context.SaveChanges();
        }

        public void Destroy(Size s)
        {
            Size entity = context.Sizes.Find(s.SizeID);
            if (entity != null)
                context.Sizes.Remove(entity);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}