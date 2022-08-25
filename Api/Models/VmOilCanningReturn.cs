using DentResistanceOilCanningUpgrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentResistanceOilCanningUpgrade.Api.Models
{
    //view model used to return data from an oil canning calculation
    public class VmOilCanningReturn
    {
        public List<Chart> chartList { get; set; }
        public OilCanning oilcanning { get; set; }
}
}