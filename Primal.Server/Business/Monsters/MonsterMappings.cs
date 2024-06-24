using System;
using Primal.Common;

namespace Primal.Business.Monsters
{
    public static class MonsterMappings
    {
        public static IMonsterDefinition MonsterMap(MonsterType monsterType) => monsterType switch
        {
            MonsterType.Vyraxen => new Vyraxen(),
            _ => throw new NotImplementedException()
        };

        public static int[] AggressionIconMap(int aggressionLevel) => aggressionLevel switch {
            0 => [0,1,2],
            1 => [1,2,3],
            2 => [2,3,4],
            3 => [3,4,5],
            _ => []
        };
    }
}