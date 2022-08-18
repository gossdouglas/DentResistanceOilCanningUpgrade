using DentResistanceOilCanningUpgrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentResistanceOilCanningUpgrade.Api.Models
{
    public class VmOilCanningReturn
    {
        public List<Chart> chartList { get; set; }
        public OilCanning oilcanning { get; set; }
}
}