using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data.EF
{
    public class GContext : DbContext
    {
        public event Action<DbModelBuilder> EventRegistModel;

        static GContext()
        {
            //todo 温柔的解决迁移问题
            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GContext>());

            //修复EF6从Nuget中添加的Bug
            var _ = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB");

#if DEBUG
            if (AppDomain.CurrentDomain.BaseDirectory.Contains("Framework.Data"))
            {
                dbPath = dbPath.Replace(@"Infrastructure\Framework.Data\bin\Debug", @"UI\PaPaPa.Web");
            }
#endif

            //重定向数据目录，方便测试
            AppDomain.CurrentDomain.SetData("DataDirectory", dbPath);
        }

        public GContext()
            : base("name=connectionString")
        {
            base.Configuration.AutoDetectChangesEnabled = false;
            //关闭EF6.x 默认自动生成null判断语句
            base.Configuration.UseDatabaseNullSemantics = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (EventRegistModel != null)
            {
                this.EventRegistModel(modelBuilder);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(string.Join("\r\n", dbEx.EntityValidationErrors.Select(x =>
                {
                    return string.Join("    \r\n", x.ValidationErrors.Select(e => e.ErrorMessage).ToList());
                }).ToList()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(string.Join("\r\n", dbEx.EntityValidationErrors.Select(x =>
                {
                    return string.Join("    \r\n", x.ValidationErrors.Select(e => e.ErrorMessage).ToList());
                }).ToList()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
