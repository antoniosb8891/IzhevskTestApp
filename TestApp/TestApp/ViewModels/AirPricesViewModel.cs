using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TestApp.Models;

namespace TestApp.ViewModels
{
    public class AirPricesViewModel : INotifyPropertyChanged
    {
        private AirPriceResponse[] _airPriceResponse;

        public AirPricesViewModel()
        {
            _airPriceResponse = new AirPriceResponse[0];
        }

        public AirPricesViewModel(AirPriceResponse[] airPriceResponse)
        {
            if (airPriceResponse != null && airPriceResponse.Length > 0)
                _airPriceResponse = airPriceResponse;
            else
                _airPriceResponse = new AirPriceResponse[]
                {
                    new AirPriceResponse() { depart_date = "2020-05-06", return_date = "2020-05-20", value = 7423, destination = "MOW", actual = true },
                    new AirPriceResponse() { depart_date = "2020-05-10", return_date = "2020-06-10", value = 5435, destination = "PSA", actual = true },
                    new AirPriceResponse() { depart_date = "2020-05-15", return_date = "2020-06-15", value = 34653, destination = "PRG", actual = true },
                    new AirPriceResponse() { depart_date = "2020-05-20", return_date = "2020-06-20", value = 6556, destination = "MOW", actual = false },
                    new AirPriceResponse() { depart_date = "2020-05-25", return_date = "2020-06-25", value = 56787, destination = "MOW", actual = true },
                    new AirPriceResponse() { depart_date = "2020-06-10", return_date = "2020-07-10", value = 345, destination = "MOW", actual = false },
                    new AirPriceResponse() { depart_date = "2020-06-20", return_date = "2020-07-01", value = 9789, destination = "MOW", actual = true }
                };
        }

        public AirPriceResponse[] AirPriceResponse => _airPriceResponse;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
