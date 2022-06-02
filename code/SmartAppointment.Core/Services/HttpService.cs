using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Services
{
    public class HttpService : IHttpService
    {
        private IAuthenticationService _authService;
        HttpClient _httpClient;
        public HttpService(IAuthenticationService authService)
        {
            _authService = authService;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAsync(Uri baseAddress, string api)
        {
            try
            {
                var apiUri = new Uri(baseAddress, api);
                var request = new HttpRequestMessage(HttpMethod.Get, apiUri);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetAccessToken());
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
