using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestApp.Helpers;
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
            await DisplayAlert("", ctrl.IATA, "Ok");

            ListView lst = (ListView)sender;
            lst.SelectedItem = null;
        }

        // обновление меток на карте
        private async void MapUpdate()
        {
            //AirsMap.Pins.Clear();
            /*foreach (var p in _context.IATADirectItemsList)
            {
                if (!p.IsVaildCoord)
                    continue;
                var pin = new Pin()
                {
                    Label = String.Format("{0} | {1}", p.IATA, p.Name),
                    Position = new Position(p.Lat, p.Lng),
                    Address = p.CountryName,
                    Tag = p
                };
                AirsMap.Pins.Add(pin);
            }*/

            //var pinList = _context.IATADirectItemsList.Where(x => x.Pin != null).ToList();
            //AirsMap.ItemsSource = pinList;
            //AirsMap.Pins = pinList;

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
            //var ctrl = e.Pin.Tag as IATADirectItemModel;
        }
    }
}
