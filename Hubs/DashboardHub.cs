using Microsoft.AspNetCore.SignalR;

namespace Tesko.Hubs
{
    public class DashboardHub : Hub
    {
        public async Task SendDashboardUpdate()
        {
            await Clients.All.SendAsync("ReceiveDashboardUpdate");
        }
    }
}
