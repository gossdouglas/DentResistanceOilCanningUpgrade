using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace DentResistanceOilCanningUpgrade.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        //public virtual DbSet<dr_Formulas> dr_Formulas { get; set; }
        //public virtual DbSet<dr_Grades> dr_Grades { get; set; }
    }
}