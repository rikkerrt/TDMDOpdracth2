using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUELampen.Domain.ClientInterface
{
    public interface IHTTPClient
    {
        Task<Light?> GetAllLightsAsync();
    }
}
