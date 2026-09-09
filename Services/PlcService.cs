using MESDemo.Models;
using RadialGraderMES.Web.Services;

namespace MESDemo.Services
{
    public class PlcService : IPlcService
    {
        private readonly HttpClient _httpClient;
        public PlcService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<PlcDataDto?> ReadDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PlcDataDto>(
                    "api/Plc/data",
                    cancellationToken);
            }
            catch(HttpRequestException)
            {
                return null;
            }
        }
    }
}
