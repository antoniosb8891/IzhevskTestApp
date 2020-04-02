using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestApp.Helpers;
using TestApp.Models;
using TestApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace TestApp.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private bool _firstStart = true;
        private MainPageViewModel _context;

        public MainPage()
        {
            InitializeComponent();

            _context = new MainPageViewModel();
            _context.MapUpdate = MapUpdate;
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
                if (await MyExtensions.CheckGPSPermissions(this))
                {
                    // иницилизация карты
                    AirsMap.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(), 10d);
                }
            }
        }

        private async void IATAListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var ctrl = e.SelectedItem as IATADirectItemModel;
            await Navigation.PushAsync(new AirPortCostsPage(_context.SrcAirPort, ctrl));

            ListView lst = (ListView)sender;
            lst.SelectedItem = null;
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

        private async void AirsMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            var ctrl = _context.IATADirectMapPinsList.Where(x => x.IATA.Equals(e.Pin.Label)).FirstOrDefault();
            if (ctrl != null)
                await Navigation.PushAsync(new AirPortCostsPage(_context.SrcAirPort, ctrl));
        }
    }
}
