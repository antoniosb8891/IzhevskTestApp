using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TestApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AirPortCostsPage : ContentPage
    {
        private AirPortCostsPageViewModel _context;
        private bool _firstStart = true;

        public AirPortCostsPage(SupportedDirectionsResponse.Origin srcAirPort, IATADirectItemModel destAirport)
        {
            InitializeComponent();

            _context = new AirPortCostsPageViewModel(srcAirPort, destAirport);
            BindingContext = _context;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_firstStart)
            {
                _firstStart = false;
                if (Device.RuntimePlatform.Equals(Device.Android))
                    await Task.Delay(500);  // Для Андроида нужно немного подождать для корректной иницилизации компонента карты
                MapUpdate();
                _context.LoadPricesCommand.Execute(null);
            }
        }

        private async void MapUpdate()
        {
            if (AirsMap.Pins.Count > 0)
                await AirsMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(
                        AirsMap.Pins[0].Position,
                        6d, // zoom
                        0d, // bearing(rotation)
                        0d // tilt
                        )));
        }
    }
}