

namespace HUELampen.Domain.ClientInterface
{
    public interface IHTTPClient
    {
        Task<Light> GetAllLightsAsync();
    }
}
