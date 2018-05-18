using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlTypes;
using System.Reflection;
using System.Web;

namespace DataAccess.DC
{
    public class DC : DbContext
    {
        private readonly object _saveLock = new object();

        public DC()
            : base()
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 400;

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public DC(string connectionString)
            : base(connectionString)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 400;

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<CategoryHeader> CategoryHeaders { get; set; }
        public DbSet<UserGroupRelation> UserGroupRelation { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<SummaryTimeSheet> SummaryTimeSheets { get; set; }

        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<CategoryCustomer> CategoryCustomers { get; set; }

        public override int SaveChanges()
        {
            //UpdateDates();
            base.Configuration.AutoDetectChangesEnabled = true;
            base.Configuration.ValidateOnSaveEnabled = true;
            return base.SaveChanges();
        }

        public int SaveChangesWithNoDetect()
        {
            UpdateDates();
            base.Configuration.AutoDetectChangesEnabled = false;
            base.Configuration.ValidateOnSaveEnabled = false;

            return base.SaveChanges();
        }

        private void UpdateDates()
        {
            foreach (var change in ChangeTracker.Entries<ILoggedEntity>())
            {
                if (change.State != EntityState.Deleted)
                {
                    var values = change.CurrentValues;
                    foreach (var name in values.PropertyNames)
                    {
                        var value = values[name];
                        if (value is DateTime)
                        {
                            var date = (DateTime)value;
                            if (date < SqlDateTime.MinValue.Value)
                            {
                                values[name] = SqlDateTime.MinValue.Value;
                            }
                            else if (date > SqlDateTime.MaxValue.Value)
                            {
                                values[name] = SqlDateTime.MaxValue.Value;
                            }
                        }
                    }
                }
            }
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            try
            {
                DataTable dtReturn = new DataTable();

                //Add columns
                PropertyInfo[] oProps = typeof(T).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }

                //Add rows
                foreach (T rec in varlist)
                {
                    DataRow dr = dtReturn.NewRow();
                    foreach (PropertyInfo pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                    }
                    dtReturn.Rows.Add(dr);
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public static class DCLoader
    {
        public static DC GetNewDC()
        {
            return new DC();
        }

        public static DC GetMyDC()
        {
            var dc = HttpContext.Current == null
                        ? new DC()
                        : (HttpContext.Current.Items["DC"] == null ? new DC() : HttpContext.Current.Items["DC"] as DC);
            return dc;
        }
    }
}
