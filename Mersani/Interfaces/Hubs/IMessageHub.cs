using System;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Hubs
{
    public interface IMessageHub
    {
        Task OnConnectedAsync();
        Task OnDisconnectedAsync(Exception exception);
    }
}
