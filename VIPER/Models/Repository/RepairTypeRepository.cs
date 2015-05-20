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

        public IList<RepairTypeViewModel> RepairTypes
        {
            get
            {
                return context.RepairTypes.Select(rt => new RepairTypeViewModel() { 
                    RepairTypeID = rt.RepairTypeID,
                    Name = rt.Name
                }).ToList();
            }
        }

        public void Create(RepairTypeViewModel rt)
        {
            var entity = new RepairType();
            entity.Name = rt.Name;
            context.RepairTypes.Add(entity);
            context.SaveChanges();
            rt.RepairTypeID = entity.RepairTypeID;
        }

        public void Update(RepairTypeViewModel rt)
        {

            RepairType entity = context.RepairTypes.Single(r => r.RepairTypeID == rt.RepairTypeID);
            if (entity != null)
                entity.Name = rt.Name;
            context.SaveChanges();

        }

        public void Destroy(RepairTypeViewModel rt)
        {

            RepairType entity = context.RepairTypes.Single(r => r.RepairTypeID == rt.RepairTypeID);
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