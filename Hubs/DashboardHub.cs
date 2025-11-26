using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Tesko.Hubs
{
    public class DashboardHub : Hub
    {
        private const string ApproversGroup = "Approvers";

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var user = Context.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            }

            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Approver" || role == "Admin")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, ApproversGroup);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
                }

                var role = user.FindFirst(ClaimTypes.Role)?.Value;
                if (role == "Approver" || role == "Admin")
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, ApproversGroup);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendDashboardUpdate()
        {
            await Clients.All.SendAsync("ReceiveDashboardUpdate");
        }
    }
}
