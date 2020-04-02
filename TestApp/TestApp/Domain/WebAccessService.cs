using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestApp.Helpers;
using TestApp.Models;

namespace TestApp.Domain
{
    public class WebAccessService
    {
        public delegate void OnErrorDelegate(string statusCode, string msg);
        public event OnErrorDelegate OnError;

        private HttpClient _client;
        private const string _baseHost = "http://map.aviasales.ru";

        public WebAccessService()
        {
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => { return true; };
            _client = new HttpClient()
            {
                BaseAddress = new Uri(_baseHost),
                MaxResponseContentBufferSize = 1024 * 1024 * 16,
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<SupportedDirectionsResponse> GetSupportedDirections(string iataSource)
        {
            var uri = new Uri(String.Format("/supported_directions.json?origin_iata={0}&one_way=true&locale=ru", iataSource));
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            string answer = await SendRequest(httpRequestMessage);
            try
            {
                return JsonConvert.DeserializeObject<SupportedDirectionsResponse>(answer, MyExtensions.JsonSettings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JSON Exception:\n" + ex);
                return null;
            }
        }

        public async Task<AirPriceResponse[]> GetAirPrices(string iataSource)
        {
            var uri = new Uri(String.Format("/prices.json?origin_iata={0}&period=2020-05-05:season&direct=true&one_way=true&price=50000&no_visa=true&schengen=true&need_visa=false&locale=ru", iataSource));
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            string answer = await SendRequest(httpRequestMessage);
            try
            {
                return JsonConvert.DeserializeObject<AirPriceResponse[]>(answer, MyExtensions.JsonSettings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JSON Exception:\n" + ex);
                return null;
            }
        }

        private async Task<string> SendRequest(HttpRequestMessage request)
        {
            if (!MyExtensions.IsHasInternet())
            {
                OnError?.Invoke("", "Нет подключения к интернету");
                return String.Empty;
            }

            try
            {
                var httpResponseMessage = await _client.SendAsync(request);
                var httpContent = httpResponseMessage.Content;
                var resp = await httpResponseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine(String.Format("Status: {0}\nresp: {1}", httpResponseMessage.StatusCode.ToString(), resp));
                if (!httpResponseMessage.IsSuccessStatusCode)
                    OnError?.Invoke(httpResponseMessage.StatusCode.ToString(), resp);
                return resp;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                OnError?.Invoke("Exception", ex.Message);
                return String.Empty;
            }
        }

    }
}
