using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Models
{
    public class AirPriceResponse
    {
        public bool show_to_affiliates { get; set; }
        public int trip_class { get; set; }
        public string destination { get; set; }
        public string depart_date { get; set; }
        public string return_date { get; set; }
        public int number_of_changes { get; set; }
        public int value { get; set; }
        public long created_at { get; set; }
        public long ttl { get; set; }
        public int distance { get; set; }
        public bool actual { get; set; }
    }
}
