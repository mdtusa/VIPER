using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public IList<SizeViewModel> Sizes
        {
            get { return context.Sizes.Select(s => new SizeViewModel() {
                SizeID = s.SizeID,
                Name = s.Name,
                RepairTypeID = s.RepairTypeID,
                RepairType = new RepairTypeViewModel()
                {
                    RepairTypeID = s.RepairType.RepairTypeID,
                    Name = s.RepairType.Name
                }
            }).ToList(); }
        }

        public void Create(SizeViewModel s)
        {
            var size = new Size();
            size.Name = s.Name;
            size.RepairTypeID = s.RepairType.RepairTypeID;
            context.Sizes.Add(size);
            context.SaveChanges();
            s.SizeID = size.SizeID;
        }

        public void Update(SizeViewModel s)
        {
            Size entity = context.Sizes.Single(size => size.SizeID == s.SizeID);
            if (entity != null)
            {
                entity.Name = s.Name;
                entity.RepairTypeID = s.RepairType.RepairTypeID;
            }
            context.SaveChanges();
        }

        public void Destroy(SizeViewModel s)
        {
            Size entity = context.Sizes.Single(size => size.SizeID == s.SizeID);
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