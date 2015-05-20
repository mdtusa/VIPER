using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using VIPER.Models.ViewModel;
using VIPER.Models.Entities;

namespace VIPER.Models.Repository
{
    public class ProcessTimeRepository : IDisposable
    {
        private VIPERDbContext context;

        public ProcessTimeRepository()
        {
            context = new VIPERDbContext();
        }

        public List<ProcessTimeViewModel> ProcessTimeFromSQL
        {
            get
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT PT.RepairTypeID,PT.SizeID, RT.Name RepairTypeName, S.Name SizeName,");
                sql.Append("MAX(CASE WHEN P.Name = 'Dis-assemble' THEN PlannedTime END) DisassTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Dis-assemble' THEN PT.ProcessID END) DisassProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Dis-assemble' THEN ProcessTimeID END) DisassProcTimeID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Clean' THEN PlannedTime END) CleanTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Clean' THEN PT.ProcessID END) CleanProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Clean' THEN ProcessTimeID END) CleanProcTimeID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Inspect' THEN PlannedTime END) InspectTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Inspect' THEN PT.ProcessID END) InspectProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Inspect' THEN ProcessTimeID END) InspectProcTimeID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Assemble' THEN PlannedTime END) AssembleTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Assemble' THEN PT.ProcessID END) AssembleProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Assemble' THEN ProcessTimeID END) AssembleProcTimeID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Additional Works' THEN PlannedTime END) AddWorksTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Additional Works' THEN PT.ProcessID END) AddWorksProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Additional Works' THEN ProcessTimeID END) AddWorksProcTimeID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Paint' THEN PlannedTime END) PaintTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Paint' THEN PT.ProcessID END) PaintProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Paint' THEN ProcessTimeID END) PaintProcTimeID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Packaging' THEN PlannedTime END) PackagingTime,");
                sql.Append("MAX(CASE WHEN P.Name = 'Packaging' THEN PT.ProcessID END) PackagingProcID,");
                sql.Append("MAX(CASE WHEN P.Name = 'Packaging' THEN ProcessTimeID END) PackagingProcTimeID ");
                sql.Append("FROM ProcessTime PT ");
                sql.Append("INNER JOIN Process P ON PT.ProcessID = P.ProcessID ");
                sql.Append("INNER JOIN RepairType RT ON PT.RepairTypeID = RT.RepairTypeID ");
                sql.Append("INNER JOIN Size S ON PT.SizeID = S.SizeID ");
                sql.Append("GROUP BY PT.RepairTypeID,PT.SizeID, RT.Name, S.Name");

                List<ProcessTimeViewModel> entities = context.Database.SqlQuery<ProcessTimeViewModel>(sql.ToString()).ToList();

                if (entities != null)
                {
                    foreach (var item in entities)
                    {
                        item.CreateTypes();
                    }
                }

                return entities;

            }
        }

        public void Create(ProcessTimeViewModel p)
        {

            List<Process> proc = context.Processes.OrderBy(pr => pr.Step).ToList();
            ProcessTime entity = new ProcessTime();
            entity.ProcessID = proc[0].ProcessID;
            entity.PlannedTime = p.DisassTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.DisassProcTimeID = entity.ProcessTimeID;
            p.DisassProcID = entity.ProcessID;

            entity = new ProcessTime();
            entity.ProcessID = proc[1].ProcessID;
            entity.PlannedTime = p.CleanTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.CleanProcTimeID = entity.ProcessTimeID;
            p.CleanProcID = entity.ProcessID;

            entity = new ProcessTime();
            entity.ProcessID = proc[2].ProcessID;
            entity.PlannedTime = p.InspectTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.InspectProcTimeID = entity.ProcessTimeID;
            p.InspectProcID = entity.ProcessID;

            entity = new ProcessTime();
            entity.ProcessID = proc[3].ProcessID;
            entity.PlannedTime = p.AssembleTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.AssembleProcTimeID = entity.ProcessTimeID;
            p.AssembleProcID = entity.ProcessID;

            entity = new ProcessTime();
            entity.ProcessID = proc[4].ProcessID;
            entity.PlannedTime = p.AddWorksTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.AddWorksProcTimeID = entity.ProcessTimeID;
            p.AddWorksProcID = entity.ProcessID;

            entity = new ProcessTime();
            entity.ProcessID = proc[5].ProcessID;
            entity.PlannedTime = p.PaintTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.PaintProcTimeID = entity.ProcessTimeID;
            p.PaintProcID = entity.ProcessID;

            entity = new ProcessTime();
            entity.ProcessID = proc[6].ProcessID;
            entity.PlannedTime = p.PackagingTime.GetValueOrDefault();
            entity.RepairTypeID = p.RepairType.RepairTypeID;
            entity.SizeID = p.Size.SizeID;
            context.ProcessTimes.Add(entity);
            context.SaveChanges();
            p.PackagingProcTimeID = entity.ProcessTimeID;
            p.PackagingProcID = entity.ProcessID;

        }

        public void Update(ProcessTimeViewModel p)
        {
            ProcessTime entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.DisassProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.DisassProcID.GetValueOrDefault();
                entity.PlannedTime = p.DisassTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.CleanProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.CleanProcID.GetValueOrDefault();
                entity.PlannedTime = p.CleanTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.InspectProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.InspectProcID.GetValueOrDefault();
                entity.PlannedTime = p.InspectTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.AssembleProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.AssembleProcID.GetValueOrDefault();
                entity.PlannedTime = p.AssembleTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.AddWorksProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.AddWorksProcID.GetValueOrDefault();
                entity.PlannedTime = p.AddWorksTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.PaintProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.PaintProcID.GetValueOrDefault();
                entity.PlannedTime = p.PaintTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.PackagingProcTimeID);
            if (entity != null)
            {
                entity.ProcessID = p.PackagingProcID.GetValueOrDefault();
                entity.PlannedTime = p.PackagingTime.GetValueOrDefault();
                entity.RepairTypeID = p.RepairType.RepairTypeID;
                entity.SizeID = p.Size.SizeID;
            }
            context.SaveChanges();
        }

        public void Destroy(ProcessTimeViewModel p)
        {
            ProcessTime entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.DisassProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.CleanProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.InspectProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.AssembleProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.AddWorksProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.PaintProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();

            entity = context.ProcessTimes.Single(r => r.ProcessTimeID == p.PackagingProcTimeID);
            if (entity != null)
                context.ProcessTimes.Remove(entity);
            context.SaveChanges();
        }
        
        public void Dispose()
        {
            context.Dispose();
        }
    }
}