using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUELampen.Domain.ClientInterface
{
    public interface IHTTPSClient
    {
        Task ConnectAsync();
        Task<string> ReceiveDataAsync();
        Task SendAsync(string data);
        Task CloseAsync();
    }
}
