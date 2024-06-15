using System.Linq;
using Primal.Entities;
using Primal.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Primal.SignalR;
using System.Threading.Tasks;

namespace Primal.Business.Services
{
    public interface INotificationService
    {
        Task UpdateGameState(int scenarioId);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IMapper _mapper;

        public NotificationService(
            IHubContext<SignalRHub> hubContext,
            IMapper mapper
        )
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task UpdateGameState(int scenarioId)
        {
            //await _hubContext.Clients.Group(encounterId.ToString()).SendAsync("GameState", model);
        }
    }
}
