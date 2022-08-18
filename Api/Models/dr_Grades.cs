using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
//using System.Web.Mvc;

namespace DentResistanceOilCanningUpgrade.Models
{
    public partial class dr_Grades
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int grade_key { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int model { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string grade_name { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int publish { get; set; }

        [Key]
        [Column(Order = 4)]
        public double normal_anisotropy { get; set; }

        [Key]
        [Column(Order = 5)]
        public double constants { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string constants_1 { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "datetime2")]
        public DateTime date_created { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string created_by { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "datetime2")]
        public DateTime date_updated { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string updated_by { get; set; }
    }
}
