using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Primal.Business.Services;

namespace Primal.SignalR
{
    public class SignalRHub : Hub
    {
        private readonly INotificationService _notificationService;

        public SignalRHub(INotificationService notificationService){
            _notificationService = notificationService;
        }

        public async Task<string> Register(int encounterId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, encounterId.ToString());
            await _notificationService.UpdateGameState(encounterId);
            return GlobalConstants.SUCCESS;
        }
    }
}