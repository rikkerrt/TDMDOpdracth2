using HUELampen.Domain;
using HUELampen.Domain.ClientInterface;

namespace HUELampen.Infrastructure.BridgeConnection
{
    // All the code in this file is included in all platforms.
    public class BridgeConnection : IHTTPClient
    {
        private readonly HttpClient httpClient;

        public BridgeConnection(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Light> GetAllLightsAsync()
        {
            var response = await httpClient.GetAsync("");
            throw new NotImplementedException();
        }
    }
}
