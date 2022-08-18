using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
//using System.Web.Mvc;

namespace DentResistanceOilCanningUpgrade.Models
{   
    public partial class dr_Formulas
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int formula_key { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int grade_key { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int model { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string model_formula { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int process_order { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string variable_name { get; set; }

        [Key]
        [Column(Order = 6)]
        public double coefficient { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string formula { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "datetime2")]
        public DateTime date_created { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string created_by { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "datetime2")]
        public DateTime date_updated { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string updated_by { get; set; }
    }
}
