using System;
using System.Linq;
using System.Collections.Generic;
using Primal.Entities;
using Primal.Repositories;
using Primal.Extensions;
using Primal.Business.Constants;

namespace Primal.Business.Services
{
    public interface IMightCardsService
    {
        List<MightCard> DrawCardsFromCritCards(int deckId, int attackId, CharacterType attacker, List<MightCard> cards);
        List<MightCard> DrawCardsFromCards(int deckId, int attackId, CharacterType attacker, List<MightCard> cards);
        List<MightCard> DrawCards(int deckId, int attackId, CharacterType attacker, Dictionary<Might, int> cardsToDraw);
    }

    public class MightCardsService : IMightCardsService
    {
        private readonly IDatabaseRepository<MightCard> _mightCards;

        public MightCardsService(
            IDatabaseRepository<MightCard> mightCards
        )
        {
            _mightCards = mightCards;
        }

        public List<MightCard> DrawCardsFromCritCards(int deckId, int attackId, CharacterType attacker, List<MightCard> cards)
        {
            var cardsDrawn = DrawCardsFromCards(
                deckId,
                attackId,
                attacker,
                cards.Where(x => x.IsCritical).ToList()
            );
            cardsDrawn.ForEach(x => x.IsDrawnFromCritical = true);
            _mightCards.UpdateBatch(cardsDrawn);
            return cardsDrawn;
        }

        public List<MightCard> DrawCardsFromCards(int deckId, int attackId, CharacterType attacker, List<MightCard> cards)
        {
            var cardsDrawn = DrawCards(
                deckId,
                attackId,
                attacker,
                cards
                    .GroupBy(x => x.Type)
                    .ToDictionary(x => x.Key, x => x.Count())
            );
            return cardsDrawn;
        }

        public List<MightCard> DrawCards(int deckId, int attackId, CharacterType attacker, Dictionary<Might, int> cardsToDraw)
        {
            var cardsDrawn = new List<MightCard>();
            var mightDeckCards = GetMightCards(deckId);
            foreach (var might in Enum.GetValues<Might>())
            {
                if (!cardsToDraw.ContainsKey(might))
                {
                    continue;
                }
                if (mightDeckCards[might].Count > cardsToDraw[might])
                {
                    cardsDrawn.AddRange(mightDeckCards[might].Take(cardsToDraw[might]));
                }
                else
                {
                    var extraCardsNeeded = cardsToDraw[might] - mightDeckCards[might].Count;
                    cardsDrawn.AddRange(mightDeckCards[might]);
                    RefreshMightDeck(might, deckId);
                    mightDeckCards = GetMightCards(deckId);
                    cardsDrawn.AddRange(mightDeckCards[might].Take(extraCardsNeeded));
                }
            }

            if (attacker == CharacterType.Player)
            {
                cardsDrawn.ForEach(x => x.AttackId = attackId);
            }
            else
            {
                cardsDrawn.ForEach(x => x.BossAttackId = attackId);
            }
            _mightCards.UpdateBatch(cardsDrawn);
            return cardsDrawn;
        }

        private Dictionary<Might, List<MightCard>> GetMightCards(int deckId)
        {
            return _mightCards
                .Read(x => x.DeckId == deckId && x.AttackId is null && x.BossAttackId is null)
                .OrderBy(x => x.Id)
                .GroupBy(x => x.Type)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        private void RefreshMightDeck(Might might, int deckId)
        {
            var mightCards = MightCardsDistribution.MIGHT_CARDS
            .Where(x => x.Type == might)
            .Select(x =>
                new MightCard
                {
                    Type = x.Type,
                    Value = x.Value,
                    IsCritical = x.IsCritical,
                    DeckId = deckId
                }
            ).ToList();
            mightCards.Shuffle();
            _mightCards.AddBatch(mightCards);
        }
    }
}
