using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using SampleDb.Entities;

namespace ErgoFab.DataAccess.Model
{
    public class ErgoFabDbContext : DbContext
    {
        public DbSet<Country> Country { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Employee> Employee { get; set; }

        public ErgoFabDbContext()
        {
        }

        public ErgoFabDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    
    public class ErgoFabDbContextDesignTime : IDesignTimeDbContextFactory<ErgoFabDbContext>
    {
        public ErgoFabDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ErgoFabDbContext>();
            // pass your design time connection string here
            NpgsqlConnectionStringBuilder cs = new NpgsqlConnectionStringBuilder()
            {
                Host = "dummy"
            };
            optionsBuilder.UseNpgsql(cs.ConnectionString);
            return new ErgoFabDbContext(optionsBuilder.Options);
        }
    }
}
