using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestApp.Helpers
{
    public static class MyExtensions
    {
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public static bool IsHasInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public static async Task<bool> CheckGPSPermissions(Page page)
        {
            bool isGpsOk = false;

            try
            {
                // Проверка разрешения получения геопозиционирования
                PermissionStatus geoStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (geoStatus != PermissionStatus.Granted)
                    geoStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (geoStatus == PermissionStatus.Granted)
                    isGpsOk = true;
                else if ((geoStatus != PermissionStatus.Unknown) && await page.DisplayAlert("Разрешите доступ", "Пожалуйста, разрешите доступ приложению к Геолокациив настройках устройства.\nДля работы карты, требуется включенный GPS.", "Настройки", "Не сейчас"))
                {
                    if (Device.RuntimePlatform.Equals(Device.iOS))
                        await Launcher.CanOpenAsync("app-settings:");
                    else
                        DependencyService.Get<IAppChangePermisSysSettings>().OpenSettings();

                    geoStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                    if (geoStatus != PermissionStatus.Granted)
                        geoStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (geoStatus == PermissionStatus.Granted)
                        isGpsOk = true;
                }

                if (isGpsOk)
                    return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

    }
}
