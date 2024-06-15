using System.Linq;
using Primal.Entities;
using Primal.Repositories;
using Primal.Models;
using AutoMapper;
using Primal.Business.Bosses;
using Microsoft.AspNetCore.SignalR;
using Primal.SignalR;
using System.Threading.Tasks;

namespace Primal.Business.Services
{
    public interface INotificationService
    {
        Task UpdateGameState(int encounterId);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IDatabaseRepository<EncounterPlayer> _encounterPlayers;
        private readonly IDatabaseRepository<Boss> _bosses;
        private readonly IDatabaseRepository<BossAttack> _bossAttacks;
        private readonly IMapper _mapper;
        private readonly IBossFactory _bossFactory;

        public NotificationService(
            IHubContext<SignalRHub> hubContext,
            IDatabaseRepository<EncounterPlayer> encounterPlayers,
            IDatabaseRepository<Boss> bosses,
            IDatabaseRepository<BossAttack> bossAttacks,
            IMapper mapper,
            IBossFactory bossFactory
        )
        {
            _hubContext = hubContext;
            _encounterPlayers = encounterPlayers;
            _bosses = bosses;
            _bossAttacks = bossAttacks;
            _mapper = mapper;
            _bossFactory = bossFactory;
        }

        public async Task UpdateGameState(int encounterId)
        {
            var players = _encounterPlayers.Read(x => x.EncounterId == encounterId, x => x.Player);
            var bossEntity = _bosses.ReadOne(x => x.EncounterId == encounterId, x => x.Encounter);
            var boss = _bossFactory.GetBossInstance(bossEntity);
            var bossModel = _mapper.Map<Boss, BossModel>(bossEntity);
            bossModel.Name = boss.Name;
            bossModel.NextAction = boss.GetNextActionText();
            bossModel.Positions = boss.GetBossPositions();

            var characterPerformingAction = bossEntity.Encounter.CharacterPerformingAction;

            var model = new GameStateModel()
            {
                Players = players.Select(x => _mapper.Map<EncounterPlayer, PlayerModel>(x)).ToList(),
                Boss = bossModel,
                CharacterPerformingAction = characterPerformingAction
            };

            if (characterPerformingAction == CharacterType.Boss)
            {
                var attack = _bossAttacks.ReadOne(x => x.BossId == bossEntity.Id, x => x.MightCards);
                if (attack is not null)
                {
                    model.Attack = new DisplayAttackModel()
                    {
                        AttackId = attack.Id,
                        CardsDrawn = attack.MightCards.Select(x => _mapper.Map<MightCard, MightCardModel>(x)).ToList(),
                        AttackerId = bossEntity.Id,
                        CharacterType = CharacterType.Boss
                    };
                }
            }

            await _hubContext.Clients.Group(encounterId.ToString()).SendAsync("GameState", model);
        }
    }
}
