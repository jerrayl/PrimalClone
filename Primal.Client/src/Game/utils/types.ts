import { CharacterType, Class, Might } from "../../utils/apiModels";

export type TileOccupant = {
    id: number;
    type: CharacterType;
    content: string | Class;
    description: string;
}

export type MightCard = {
    id: number;
    value: number;
    type: Might;
    isCritical: boolean;
    isDrawnFromCritical: boolean;
}