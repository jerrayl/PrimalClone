using Primal.Common;

namespace Primal.Business
{
    public interface IPlayerActions
    {
        void Move(BoardSector sector, int? discardedCardId, bool useStaminaToken);
        void PlayCard(int cardId, int[] discardedCardIds, bool useStaminaToken);
        void EndPhase();
        void UseEquipmentAction(EquipmentType equipmentType);
        void UsePotion(int index);
    }
}
