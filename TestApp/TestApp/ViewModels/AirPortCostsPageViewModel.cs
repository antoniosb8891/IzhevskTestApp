using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using TestApp.Models;
using Xamarin.Forms;

namespace TestApp.ViewModels
{
    public class AirPortCostsPageViewModel : INotifyPropertyChanged
    {
        public Action MapUpdate = null;
        private SupportedDirectionsResponse.Origin _srcAirPort;
        private ObservableCollection<PriceItemModel> _priceItemsListItemsList = new ObservableCollection<PriceItemModel>();
        private IATADirectItemModel _destAirport;
        private bool _priceListRefreshing = false;
        private bool _dataLoading = false;

        public AirPortCostsPageViewModel(SupportedDirectionsResponse.Origin srcAirPort, IATADirectItemModel destAirport)
        {
            _srcAirPort = srcAirPort;
            _destAirport = destAirport;
        }

        public List<IATADirectItemModel> MapPinsList => new List<IATADirectItemModel>() { _destAirport };

        public bool PriceListRefreshing
        {
            get => _priceListRefreshing;
            set
            {
                if (value == _priceListRefreshing) return;
                _priceListRefreshing = value;
                OnPropertyChanged("PriceListRefreshing");
            }
        }

        public bool DataLoading
        {
            get => _dataLoading;
            set
            {
                if (value == _dataLoading) return;
                _dataLoading = value;
                OnPropertyChanged("DataLoading");
            }
        }

        public ObservableCollection<PriceItemModel> PriceItemsList
        {
            get => _priceItemsListItemsList;
            set
            {
                if (value == _priceItemsListItemsList) return;
                _priceItemsListItemsList = value;
                OnPropertyChanged("PriceItemsList");
            }
        }

        public ICommand LoadPricesCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (DataLoading)
                        return;

                    PriceListRefreshing = true;
                    DataLoading = true;
                    var o = await App.WebAccess.GetAirPrices(_srcAirPort.iata);
                    PriceListRefreshing = false;
                    DataLoading = false;
                    PriceItemsList.Clear();
                    string iata = _destAirport.IATA;
                    foreach (var p in o.AirPriceResponse)
                        if (p.actual && iata.Equals(p.destination))
                            PriceItemsList.Add(new PriceItemModel(p));
                });
            }
        }

        public string Title => _srcAirPort.name;
        public string DestIATA => _destAirport.IATA;
        public string DestName => _destAirport.Name;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PriceItemModel
    {
        private AirPriceResponse _airPrice;
        public PriceItemModel(AirPriceResponse airPrice)
        {
            _airPrice = airPrice;
        }

        public string DepartDate => _airPrice.depart_date;
        public string ReturnDate => _airPrice.return_date;
        public int Cost => _airPrice.value;
    }
}
