using System;
using System.Linq;
using System.Collections.Generic;
using Primal.Entities;
using Primal.Repositories;
using Primal.Models;
using Primal.Extensions;
using AutoMapper;
using Primal.Business.Bosses;
using Primal.Business.Constants;
using Primal.Business.Helpers;
using Primal.Business.Services;
using System.Threading.Tasks;
using Primal.Infrastructure;

namespace Primal.Business
{
    public interface IGame
    {
        int StartEncounter(StartEncounterModel model);
        AttackResponseModel StartAttack(AttackModel attackModel);
        AttackResponseModel RerollAttack(RerollModel rerollModel);
        Task CompleteAttack(int attackId);
        Task Move(MoveModel moveModel);
        Task SpendToken(SpendTokenModel spendTokenModel);
        Task EndTurn();
        Task ContinueEnemyAction();
    }

    public class Game : IGame
    {
        private readonly IDatabaseRepository<Encounter> _encounters;
        private readonly IDatabaseRepository<EncounterMightDeck> _encounterMightDecks;
        private readonly IDatabaseRepository<MightCard> _mightCards;
        private readonly IDatabaseRepository<FreeCompany> _freeCompanies;
        private readonly IDatabaseRepository<Player> _players;
        private readonly IDatabaseRepository<EncounterPlayer> _encounterPlayers;
        private readonly IDatabaseRepository<Boss> _bosses;
        private readonly IDatabaseRepository<Attack> _attacks;
        private readonly IMapper _mapper;
        private readonly IMightCardsService _mightCardsService;
        private readonly INotificationService _notificationService;
        private readonly IBossFactory _bossFactory;
        private readonly UserContext _userContext;


        public Game(
            IDatabaseRepository<Encounter> encounters,
            IDatabaseRepository<EncounterMightDeck> encounterMightDecks,
            IDatabaseRepository<MightCard> mightCards,
            IDatabaseRepository<FreeCompany> freeCompanies,
            IDatabaseRepository<Player> players,
            IDatabaseRepository<EncounterPlayer> encounterPlayers,
            IDatabaseRepository<Boss> bosses,
            IDatabaseRepository<Attack> attacks,
            IMapper mapper,
            IMightCardsService mightCardsService,
            INotificationService notificationService,
            IBossFactory bossFactory,
            UserContext userContext
        )
        {
            _encounters = encounters;
            _encounterMightDecks = encounterMightDecks;
            _mightCards = mightCards;
            _freeCompanies = freeCompanies;
            _players = players;
            _encounterPlayers = encounterPlayers;
            _bosses = bosses;
            _attacks = attacks;
            _mapper = mapper;
            _mightCardsService = mightCardsService;
            _notificationService = notificationService;
            _bossFactory = bossFactory;
            _userContext = userContext;
        }

        public int StartEncounter(StartEncounterModel model)
        {
            var freeCompany = _freeCompanies.ReadOne(x => x.Code == model.FreeCompanyCode, x => x.Players);

            if (freeCompany.Players.Count != 4)
            {
                throw new ErrorMessageException("Free company does not have enough players");
            }

            var encounter = new Encounter();
            _encounters.Add(encounter);

            foreach (var index in Enumerable.Range(0, 4))
            {
                var encounterPlayer = DefaultPlayers.EncounterPlayers[index];
                encounterPlayer.EncounterId = encounter.Id;
                encounterPlayer.PlayerId = freeCompany.Players[index].Id;
                _encounterPlayers.Add(encounterPlayer);
            }

            var playerDeck = new EncounterMightDeck()
            {
                EncounterId = encounter.Id,
                IsFreeCompanyDeck = true
            };
            _encounterMightDecks.Add(playerDeck);

            var enemyDeck = new EncounterMightDeck()
            {
                EncounterId = encounter.Id,
                IsFreeCompanyDeck = false
            };
            _encounterMightDecks.Add(enemyDeck);

            var playerMightCards = MightCardsDistribution.MIGHT_CARDS
                .Select(x => new MightCard
                {
                    Type = x.Type,
                    Value = x.Value,
                    IsCritical = x.IsCritical,
                    DeckId = playerDeck.Id
                })
                .ToList();
            playerMightCards.Shuffle();
            _mightCards.AddBatch(playerMightCards);

            var enemyMightCards = MightCardsDistribution.MIGHT_CARDS
                .Select(x => new MightCard
                {
                    Type = x.Type,
                    Value = x.Value,
                    IsCritical = x.IsCritical,
                    DeckId = enemyDeck.Id
                })
                .ToList();
            enemyMightCards.Shuffle();
            _mightCards.AddBatch(enemyMightCards);

            _bossFactory.CreateBossByNumber(model.EncounterNumber, encounter.Id);

            return encounter.Id;
        }

        public async Task Move(MoveModel moveModel)
        {
            var player = _encounterPlayers.ReadOne(x => x.EncounterId == _userContext.EncounterId && x.Id == moveModel.PlayerId);

            if (player is null)
            {
                throw new ErrorMessageException("Player not found");
            }

            if (GridHelper.IsValidPath(new List<IPosition>() { player }.Concat(moveModel.Positions).ToList()) && player.CurrentAnimus >= moveModel.Positions.Count)
            {
                player.CurrentAnimus -= moveModel.Positions.Count;
                var lastPosition = moveModel.Positions.Last();
                player.XPosition = lastPosition.XPosition;
                player.YPosition = lastPosition.YPosition;
                _encounterPlayers.Update(player);
            }

            await _notificationService.UpdateGameState(_userContext.EncounterId.Value);
        }

        public async Task SpendToken(SpendTokenModel spendTokenModel)
        {
            var player = _encounterPlayers.ReadOne(x => x.EncounterId == _userContext.EncounterId && x.PlayerId == spendTokenModel.PlayerId);

            if (player is null)
            {
                throw new ErrorMessageException("Player not found");
            }

            if (spendTokenModel.Token == Token.Animus && player.Tokens[Token.Animus] > 0)
            {
                player.Tokens[Token.Animus] -= 1;
                player.CurrentAnimus += GlobalConstants.ANIMUS_TOKEN_VALUE;
                _encounterPlayers.Update(player);
            }

            if (spendTokenModel.Token == Token.Battleflow && player.Tokens[Token.Battleflow] > 0)
            {
                //Perform logic
            }

            await _notificationService.UpdateGameState(_userContext.EncounterId.Value);
        }

        public async Task CompleteAttack(int attackId)
        {
            var attack = _attacks.ReadOne(x => x.Id == attackId, x => x.MightCards, x => x.Player, x => x.Player.EncounterPlayer, x => x.Boss);

            if (attack is null)
            {
                throw new ErrorMessageException("Attack not found");
            }

            if (attack.MightCards.Count(x => !x.IsDrawnFromCritical && x.Value == 0) > GlobalConstants.NUM_ZEROES_TO_MISS)
            {
                await _notificationService.UpdateGameState(_userContext.EncounterId.Value);
            }

            attack.Player.EncounterPlayer.Tokens[Token.Empower] -= attack.EmpowerTokensUsed;
            attack.Player.EncounterPlayer.Tokens[Token.Redraw] -= attack.RerollTokensUsed;
            _encounterPlayers.Update(attack.Player.EncounterPlayer);

            // Assume attack targets boss
            attack.Boss.Health[attack.BossPart] -= (attack.MightCards.Sum(x => x.Value) + attack.BonusDamage) / attack.Boss.Defence;
            if (attack.Boss.Health[attack.BossPart] <= 0)
            {
                attack.Boss.Health[attack.BossPart] = 0;

                // If this pushes boss to next stage, discard action cards
                // Set target to be player who triggered the break
            }
            _bosses.Update(attack.Boss);

            _attacks.Delete(attack);

            await _notificationService.UpdateGameState(_userContext.EncounterId.Value);
        }

        public AttackResponseModel RerollAttack(RerollModel rerollModel)
        {
            var attack = _attacks.ReadOne(x => x.Id == rerollModel.AttackId, x => x.MightCards, x => x.Player, x => x.Player.EncounterPlayer);

            if (attack is null)
            {
                throw new ErrorMessageException("Attack not found");
            }

            var mightCardIds = attack.MightCards.Select(x => x.Id);
            if (rerollModel.MightCards.Any(x => !mightCardIds.Contains(x)))
            {
                throw new ErrorMessageException("Card ids not found");
            }

            if (rerollModel.RerollTokensUsed < 1 || attack.Player.EncounterPlayer.Tokens[Token.Redraw] < rerollModel.RerollTokensUsed || rerollModel.MightCards.Count != rerollModel.RerollTokensUsed)
            {
                throw new ErrorMessageException("Invalid number of reroll tokens");
            }

            attack.RerollTokensUsed += rerollModel.RerollTokensUsed;
            _attacks.Update(attack);

            var cardResult = attack.MightCards
                // intialize to cards not redrawn
                .Where(x => !rerollModel.MightCards.Contains(x.Id))
                .ToList();

            var cardsToRedraw = attack.MightCards
                .Where(x => rerollModel.MightCards.Contains(x.Id))
                .ToList();

            var critCards = _mightCardsService.DrawCardsFromCritCards(cardsToRedraw.First().DeckId, attack.Id, CharacterType.Player, cardsToRedraw);
            while (critCards.Any(x => x.IsCritical))
            {
                cardsToRedraw.AddRange(critCards);
                critCards = _mightCardsService.DrawCardsFromCritCards(cardsToRedraw.First().DeckId, attack.Id, CharacterType.Player, critCards);
            }
            cardResult.AddRange(critCards);

            var nonCritCards = _mightCardsService.DrawCardsFromCards(cardsToRedraw.First().DeckId, attack.Id, CharacterType.Player, cardsToRedraw.Where(x => !x.IsCritical).ToList());
            while (nonCritCards.Any(x => x.IsCritical))
            {
                cardsToRedraw.AddRange(nonCritCards);
                nonCritCards = _mightCardsService.DrawCardsFromCritCards(cardsToRedraw.First().DeckId, attack.Id, CharacterType.Player, nonCritCards);
            }
            cardResult.AddRange(nonCritCards);

            _mightCards.DeleteBatch(cardsToRedraw);

            var cardModels = cardResult
                .Select(x => _mapper.Map<MightCard, MightCardModel>(x))
                .ToList();

            return new AttackResponseModel()
            {
                AttackId = attack.Id,
                CardsDrawn = cardModels
            };
        }

        public AttackResponseModel StartAttack(AttackModel attackModel)
        {
            var player = _players.ReadOne(x => x.Id == attackModel.PlayerId, x => x.Attacks);

            if (player is null || player.Attacks.Any())
            {
                throw new ErrorMessageException("Invalid player id");
            }

            // Locate Player (in encounter)
            // Locate Enemy (in encounter)
            // Check that Player is able to attack enemy, record parts hit

            if (attackModel.Might.Values.Sum() > GlobalConstants.MAXIMUM_ATTACK_MIGHT_CARDS)
            {
                throw new ErrorMessageException("Too many might cards");
            }

            if (MightCardsHelper.GetEmpowerTokensNeeded(attackModel.Might, player.Might) != attackModel.EmpowerTokensUsed)
            {
                throw new ErrorMessageException("Invalid number of empower tokens");
            }

            // Assume that boss is the target
            var bossEntity = _bosses.ReadOne(x => x.EncounterId == _userContext.EncounterId);
            var boss = _bossFactory.GetBossInstance(bossEntity);
            if (!boss.GetBossPositions().Any(x => x.EqualTo(attackModel.Target)))
            {
                throw new ErrorMessageException("Boss is not targeted");
            }

            // Calculate bonus damage that would be done

            var attack = new Attack()
            {
                PlayerId = attackModel.PlayerId,
                BossId = bossEntity.Id,
                BossPart = boss.GetBossPartFromPosition(attackModel.Target).ConvertToString(),
                // Temporarily hardcode bonus damage as 0
                BonusDamage = 0,
                EmpowerTokensUsed = attackModel.EmpowerTokensUsed
            };
            _attacks.Add(attack);

            var playerMightDeck = _encounterMightDecks
                .ReadOne(x => x.EncounterId == _userContext.EncounterId && x.IsFreeCompanyDeck);

            var cardsDrawn = new List<MightCard>();
            var cardsDrawnFromCrit = new List<MightCard>();

            cardsDrawn.AddRange(_mightCardsService.DrawCards(playerMightDeck.Id, attack.Id, CharacterType.Player, attackModel.Might));

            if (cardsDrawn.Any(x => x.IsCritical))
            {
                cardsDrawnFromCrit.AddRange(_mightCardsService.DrawCardsFromCritCards(playerMightDeck.Id, attack.Id, CharacterType.Player, cardsDrawn));
            }

            while (cardsDrawnFromCrit.Any(x => x.IsCritical))
            {
                cardsDrawn.AddRange(cardsDrawnFromCrit);
                cardsDrawnFromCrit = _mightCardsService.DrawCardsFromCritCards(playerMightDeck.Id, attack.Id, CharacterType.Player, cardsDrawnFromCrit);
            }
            cardsDrawn.AddRange(cardsDrawnFromCrit);

            var cardModels = cardsDrawn.Select(x => _mapper.Map<MightCard, MightCardModel>(x)).ToList();

            return new AttackResponseModel
            {
                AttackId = attack.Id,
                CardsDrawn = cardModels
            };
        }

        public async Task EndTurn()
        {
            // check if all players have accepted end turn

            var encounter = _encounters.ReadOne(x => x.Id == _userContext.EncounterId, x => x.Boss);

            if (encounter is null || encounter.CharacterPerformingAction is not null)
            {
                throw new ErrorMessageException("Invalid encounter state");
            }

            _bossFactory.GetBossInstance(encounter.Boss).BeginAction();

            await _notificationService.UpdateGameState(_userContext.EncounterId.Value);
        }

        public async Task ContinueEnemyAction()
        {
            //ignore minions for now
            var encounter = _encounters.ReadOne(x => x.Id == _userContext.EncounterId, x => x.Boss);

            if (encounter is null || encounter.CharacterPerformingAction != CharacterType.Boss)
            {
                throw new ErrorMessageException("Invalid encounter state");
            }

            _bossFactory.GetBossInstance(encounter.Boss).PerformAction();

            await _notificationService.UpdateGameState(_userContext.EncounterId.Value);
        }
    }
}
