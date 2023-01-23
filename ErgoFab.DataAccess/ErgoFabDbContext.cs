using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
    }
}
