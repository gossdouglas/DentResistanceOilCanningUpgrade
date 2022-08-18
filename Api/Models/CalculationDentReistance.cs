using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentResistanceOilCanningUpgrade.Models
{
    public class CalculationDentReistance
    {
        public string GradeName { get; set; }
        public short GradeKey { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }        
        public double MajorStrain { get; set; }
        public double MinorStrain { get; set; }
        public double Thickness { get; set; }

        public string ModelFormula { get; set; }
        public double FootPounds { get; set; }
        public double RunningTotal { get; set; }

        //public int? id { get; set; }
        public int PoundsForce { get; set; }
        public double Result { get; set; }
    }
}