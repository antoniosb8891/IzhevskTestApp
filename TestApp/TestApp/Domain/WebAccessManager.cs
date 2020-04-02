using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.ViewModels;

namespace TestApp.Domain
{
    public class WebAccessManager
    {
        private WebAccessService _webAccess;

        public WebAccessManager(WebAccessService.OnErrorDelegate ErrCallBack)
        {
            _webAccess = new WebAccessService();
            _webAccess.OnError += ErrCallBack;
        }
        
        public async Task<SupportedDirectionsViewModel> GetSupportedDirections(string iataSource)
        {
            var o = await _webAccess.GetSupportedDirections(iataSource);
            return new SupportedDirectionsViewModel(o);
        }
        
        public async Task<AirPricesViewModel> GetAirPrices(string iataSource)
        {
            var o = await _webAccess.GetAirPrices(iataSource);
            return new AirPricesViewModel(o);
        }
    }
}
