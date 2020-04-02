using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TestApp.Models
{
    public class SupportedDirectionsResponse
    {
        public Origin origin { get; set; }
        public Direction[] directions { get; set; }

        public class Origin
        {
            public string iata { get; set; }
            public string name { get; set; }
            public string country { get; set; }
            public string[] coordinates { get; set; }
        }

        public class Direction
        {
            public bool direct { get; set; }
            public string iata { get; set; }
            public string name { get; set; }
            public string country { get; set; }
            public string country_name { get; set; }
            public string[] coordinates { get; set; }

            public bool IsVaildCoords => coordinates != null && coordinates.Length == 2 &&
                        !String.IsNullOrEmpty(coordinates[0]) && !String.IsNullOrEmpty(coordinates[1]);
            public double[] Coords
            {
                get
                {
                    if (IsVaildCoords)
                    {
                        bool b = false;
                        double _lat, _lng;
                        NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
                        CultureInfo culture = new CultureInfo("en-US"); ;
                        b = Double.TryParse(coordinates[0], style, culture, out _lng);
                        b = Double.TryParse(coordinates[1], style, culture, out _lat);
                        return new double[2] { _lng, _lat };
                    }
                    else
                        return new double[0];
                }
            }
        }
    }
}
