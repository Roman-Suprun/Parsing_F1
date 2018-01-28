using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DB_F1")
        { }

        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<Team> Teams { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Result> Results { get; set; }
    }
}
