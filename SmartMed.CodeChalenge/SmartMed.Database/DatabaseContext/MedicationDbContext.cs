using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartMed.CodeChalenge.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMed.Database
{
    public class MedicationDbContext : DbContext
    {


        public MedicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Medication> Medications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        

    }
}
