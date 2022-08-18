using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentResistanceOilCanningUpgrade.Models
{
    public class ReturnObject<T>
    {
        //This is to indicate if the operation was successful. ie, normally some type of CRUD operation
        public bool success { get; set; }
        //This is to indicate if the Session was properly validated
        public bool validated { get; set; }
        //This is the redirect url if the validation was not successful
        public string url { get; set; }
        //This is the data from the CRUD operation
        public T data { get; set; }
        //public Node nodes { get; set; }
    }
}