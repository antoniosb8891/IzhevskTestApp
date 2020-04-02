using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using TestApp.Droid;
using TestApp;

[assembly: Xamarin.Forms.Dependency(typeof(AppChangePermisSysSettings))]
namespace TestApp.Droid
{
    public class AppChangePermisSysSettings : IAppChangePermisSysSettings
    {
        public void OpenSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings, Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }
    }
}