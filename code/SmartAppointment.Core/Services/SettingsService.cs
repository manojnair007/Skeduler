using SmartAppointment.Core.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAppointment.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private IHttpService _httpService;
        private ICacheService _cacheService;
        private readonly string GetCosmosDBKey = "api/GetCosmosDBKey";
        public string EndpointUri { get; private set; }
        public string PrimaryKey { get; private set; }
        public SettingsService(IHttpService httpService)
        {
            _httpService = httpService;
            EndpointUri = GlobalConfig.CosmosDBEndpointUri;
        }

        public async Task SetCosmosDBPrimaryKey()
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                PrimaryKey = await SecureStorage.GetAsync(GetCosmosDBKey);
            }
            else
            {
                PrimaryKey = await _httpService.GetAsync(new Uri(GlobalConfig.DataAccessFunctioAppEndpointUri), GetCosmosDBKey);
                await SecureStorage.SetAsync(GetCosmosDBKey, PrimaryKey);
            }
               
        }
    }
}
