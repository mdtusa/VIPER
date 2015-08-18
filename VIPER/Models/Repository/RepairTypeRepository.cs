using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;
using VIPER.Models.ViewModel;

namespace VIPER.Models.Repository
{
    public class RepairTypeRepository : IDisposable
    {
        private VIPERDbContext context;

        public RepairTypeRepository()
        {
            this.context = new VIPERDbContext();
        }

        public IList<RepairType> RepairTypes
        {
            get
            {
                return context.RepairTypes.ToList();
            }
        }

        public void Create(RepairType rt)
        {
            var entity = new RepairType();
            entity.Name = rt.Name;
            context.RepairTypes.Add(entity);
            context.SaveChanges();
            rt.RepairTypeID = entity.RepairTypeID;
        }

        public void Update(RepairType rt)
        {

            RepairType entity = context.RepairTypes.Find(rt.RepairTypeID);
            if (entity != null)
                entity.Name = rt.Name;
            context.SaveChanges();

        }

        public void Destroy(RepairType rt)
        {

            RepairType entity = context.RepairTypes.Find(rt.RepairTypeID);
            if (entity != null)
                context.RepairTypes.Remove(entity);
            context.SaveChanges();

        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}