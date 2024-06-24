using Primal.Common;

namespace Primal.Business.Players
{
    public interface IPlayerDefinition
    {
        ActionCard[] ActionCards { get; }
        MasteryCard[] MasteryCards { get; }
    }
}