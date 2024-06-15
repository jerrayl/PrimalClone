import { MightCard } from "../../utils/types";
import { COLORS, HIGHLIGHTS } from "../..//utils/constants";

export interface MightCardDisplayProps {
    mightCard: MightCard;
    toggleCard?: () => void;
    pendingRedraw?: boolean;
}

export const MightCardDisplay = ({ mightCard, toggleCard, pendingRedraw }: MightCardDisplayProps) => {
    return (
        <div className="flex flex-col items-center">
            <div
                className={`rounded-lg shadow-sm w-12 h-12 ${COLORS[mightCard.type]} font-bold text-2xl flex flex-col justify-center ${mightCard.isDrawnFromCritical ? "hover:cursor-default" : `hover:shadow-sm ${HIGHLIGHTS[mightCard.type]} hover:cursor-pointer`}`}
                onClick={toggleCard}
            >
                {pendingRedraw ? "X" : mightCard.isCritical ? `{${mightCard.value}}` : mightCard.value}
            </div>
        </div>
    )
}
