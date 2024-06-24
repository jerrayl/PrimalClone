using System;
using Primal.Common;

namespace Primal.Business.Players
{
    public static class PlayerMappings
    {
        public static IPlayerDefinition PlayerMap(ClassType classType) => classType switch
        {
            ClassType.GreatBow => new GreatBow(),
            _ => throw new NotImplementedException()
        };
    }
}