using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSerchService.Models
{
    public class propertymodel
    {
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string unit { get; set; }
        public string type { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string countryCode { get; set; }
        public string postCode { get; set; }
        public string street { get; set; }
        public int beds { get; set; }
        public int baths { get; set; }
       
    }
}