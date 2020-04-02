using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TestApp.Models;

namespace TestApp.ViewModels
{
    public class SupportedDirectionsViewModel : INotifyPropertyChanged
    {
        private SupportedDirectionsResponse _supportedDirectionsResponse;

        public SupportedDirectionsViewModel()
        {
            _supportedDirectionsResponse = new SupportedDirectionsResponse();
        }

        public SupportedDirectionsViewModel(SupportedDirectionsResponse supportedDirectionsResponse)
        {
            _supportedDirectionsResponse = supportedDirectionsResponse != null ? supportedDirectionsResponse : new SupportedDirectionsResponse();
        }

        public bool IsLoadOk
        {
            get => _supportedDirectionsResponse.directions != null;
        }

        public SupportedDirectionsResponse.Direction[] Directions => _supportedDirectionsResponse.directions != null ? _supportedDirectionsResponse.directions : new SupportedDirectionsResponse.Direction[0];

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
