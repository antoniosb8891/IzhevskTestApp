using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TestApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace TestApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _iataSource = "";
        private List<IATADirectItemModel> _iATADirectItems = new List<IATADirectItemModel>();
        private List<IATADirectItemModel> _iATADirectItemsList = null, _iATADirectMapPinsList = null;
        private bool _iATAListRefreshing = false;
        private bool _iataLoading = false;
        private string _filterText = "";
        public Action MapUpdate = null;
        private bool _filterProcessing = false;


        public bool IATAListRefreshing 
        {
            get => _iATAListRefreshing;
            set
            {
                if (value == _iATAListRefreshing) return;
                _iATAListRefreshing = value;
                OnPropertyChanged("IATAListRefreshing");
            }
        }
        
        public bool IataLoading
        {
            get => _iataLoading;
            set
            {
                if (value == _iataLoading) return;
                _iataLoading = value;
                OnPropertyChanged("IataLoading");
            }
        }

        public bool FilterProcessing
        {
            get => _filterProcessing;
            set
            {
                if (value == _filterProcessing) return;
                _filterProcessing = value;
                OnPropertyChanged("FilterProcessing");
            }
        }

        public List<IATADirectItemModel> IATADirectItemsList
        {
            get => _iATADirectItemsList;
            set
            {
                if (value == _iATADirectItemsList) return;
                _iATADirectItemsList = value;
                OnPropertyChanged("IATADirectItemsList");
            }
        }
        
        public List<IATADirectItemModel> IATADirectMapPinsList
        {
            get => _iATADirectMapPinsList;
            set
            {
                if (value == _iATADirectMapPinsList) return;
                _iATADirectMapPinsList = value;
                OnPropertyChanged("IATADirectMapPinsList");
            }
        }

        public string IATASource
        {
            get => _iataSource;
            set
            {
                if (value == _iataSource) return;
                if (value.Length < _iataSource.Length || Regex.IsMatch(value, "^[A-Za-z]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
                    _iataSource = value.ToUpper();
                OnPropertyChanged("IATASource");
            }
        }

        public ICommand FindIATACommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (String.IsNullOrEmpty(IATASource) || IataLoading)
                    {
                        if (!IataLoading)
                            IATAListRefreshing = false;
                        return;
                    }

                    IATAListRefreshing = true;
                    IataLoading = true;
                    var o = await App.WebAccess.GetSupportedDirections(IATASource);
                    IATAListRefreshing = false;
                    if (o.IsLoadOk)
                    {
                        _iATADirectItems.Clear();
                        foreach (var it in o.Directions)
                            _iATADirectItems.Add(new IATADirectItemModel(it));
                        FilterApply();
                    }
                    IataLoading = false;
                });
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                if (value == _filterText) return;
                _filterText = value;
                FilterApply();
                OnPropertyChanged("FilterText");
            }
        }
        private void FilterApply()
        {
            FilterProcessing = true;
            Task.Factory.StartNew(() =>
            {
                var list = _iATADirectItems;
                string findText = _filterText;
                if (!String.IsNullOrEmpty(findText))
                {
                    findText = findText.ToLower();
                    list = _iATADirectItems.Where(x => x.IATA.ToLower().Contains(findText) || x.Name.ToLower().Contains(findText)).ToList();
                }
                IATADirectItemsList = list;
                IATADirectMapPinsList = list.Where(x => x.Pin != null).ToList();
            }).ContinueWith(t => 
            {
                MapUpdate?.Invoke();
                FilterProcessing = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IATADirectItemModel
    {
        private SupportedDirectionsResponse.Direction _data;
        private Pin _pin;

        public IATADirectItemModel(SupportedDirectionsResponse.Direction data)
        {
            _data = data;
            if (IsVaildCoord)
            {
                _pin = new Pin();
                _pin.Label = String.Format("{0} | {1}", IATA, Name);
                _pin.Position = new Position(Lat, Lng);
                _pin.Address = CountryName;
                _pin.Tag = this;
            }
            else
                _pin = null;
        }

        public string IATA => _data.iata;
        public string Name => _data.name;
        public string CountryName => _data.country_name;
        public bool IsVaildCoord => _data.IsVaildCoords;
        public double Lat => IsVaildCoord ? _data.Coords[1] : 0;
        public double Lng => IsVaildCoord ? _data.Coords[0] : 0;
        public Pin Pin => _pin;
    }
}
