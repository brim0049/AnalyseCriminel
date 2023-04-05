using System.Collections.Generic;
using System.Reflection;
using AnalyseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyseApi
{
    public class AnalyseDbContext : DbContext
    {
        public DbSet<Models.Person> Persons { get; set; }
        public DbSet<Models.Car> Cars { get; set; }
        public DbSet<Models.Event> Events { get; set; }
        public DbSet<Models.Address> Addresses { get; set; }
        public DbSet<Models.Call> Calls { get; set; }
        public DbSet<Models.Relationship> Relations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string connectionString = "server=localhost;port=3306;database=Analyse_db1;user=user;password=user";
            dbContextOptionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

      
    }
}

