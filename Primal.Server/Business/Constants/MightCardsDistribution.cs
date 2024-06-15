using System.Collections.Generic;
using System.Linq;
using Primal.Models;

namespace Primal.Business.Constants
{
    public static class MightCardsDistribution
    {
        public static readonly List<MightCardModel> MIGHT_CARDS_THREES = new()
        {
            new MightCardModel(){
                Value = 0,
                Type = Might.White,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.White,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 1,
                Type = Might.White,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 1,
                Type = Might.White,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 2,
                Type = Might.White,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 2,
                Type = Might.White,
                IsCritical = true
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.Yellow,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.Yellow,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 1,
                Type = Might.Yellow,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 2,
                Type = Might.Yellow,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 3,
                Type = Might.Yellow,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 3,
                Type = Might.Yellow,
                IsCritical = true
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.Red,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.Red,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 2,
                Type = Might.Red,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 3,
                Type = Might.Red,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 3,
                Type = Might.Red,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 4,
                Type = Might.Red,
                IsCritical = true
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.Black,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 0,
                Type = Might.Black,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 3,
                Type = Might.Black,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 3,
                Type = Might.Black,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 4,
                Type = Might.Black,
                IsCritical = false
            },
            new MightCardModel(){
                Value = 5,
                Type = Might.Black,
                IsCritical = true
            }
        };
        public static readonly List<MightCardModel> MIGHT_CARDS = MIGHT_CARDS_THREES.Concat(MIGHT_CARDS_THREES).Concat(MIGHT_CARDS_THREES).ToList();
    }
}