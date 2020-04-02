using System;
using System.Linq;
using TestApp.Domain;
using TestApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp
{
    public partial class App : Application
    {
        public static WebAccessManager WebAccess { get; private set; }

        public App()
        {
            InitializeComponent();

            WebAccess = new WebAccessManager(ErrorViewMsg);
            MainPage = new NavigationPage(new MainPage());
        }

        public async void ErrorViewMsg(string statusCode, string msg)
        {
            await Application.Current.MainPage.Navigation.NavigationStack.Last().DisplayAlert(statusCode, msg, "Ok");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
