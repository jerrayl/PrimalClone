import { Position } from "../../utils/apiModels";

const COORDINATE_MAX_VALUE = 8;
const COORDINATE_MIN_VALUE = COORDINATE_MAX_VALUE * -1;
const MAX_DISTANCE = COORDINATE_MAX_VALUE * 2;

export const Range = (startRange: number, endRange: number) => {
    return [...Array(endRange + 1).keys()].map(i => i + startRange);
}

export const Add = (position: Position, addend: Position) => {
    return {
        xPosition: position.xPosition + addend.xPosition,
        yPosition: position.yPosition + addend.yPosition
    }
}

export const Equal = (position1: Position | null, position2: Position | null): boolean => {
    return !!position1 && !!position2 && position1.xPosition === position2.xPosition && position1.yPosition === position2.yPosition;
}

export const ContainsPosition = (positionArr: Position[], position: Position | null): boolean => {
    return positionArr.filter(x => Equal(x, position)).length > 0;
}

export const GetDistanceAlongAxis = (position1: Position, position2: Position): Number | null => {
    if (!IsOnSameAxis(position1, position2)) {
        return null;
    }

    return Math.max(Math.abs(position1.xPosition - position2.xPosition), Math.abs(position1.yPosition - position2.yPosition));
}

export const IsOnSameAxis = (position1: Position, position2: Position): boolean => {
    return (position1.xPosition === position2.xPosition) ||
        (position1.yPosition === position2.yPosition) ||
        (Math.abs(position1.xPosition - position2.xPosition) === Math.abs(position1.yPosition - position2.yPosition) && (position1.xPosition - position2.xPosition) * (position1.yPosition - position2.yPosition) < 0);
}

export const IsValidPosition = (position: Position) => {
    return position.xPosition <= COORDINATE_MAX_VALUE && position.xPosition >= COORDINATE_MIN_VALUE && position.yPosition <= COORDINATE_MAX_VALUE && position.yPosition >= COORDINATE_MIN_VALUE;
}

export const IsValidPath = (positions: Position[]): boolean => {
    return positions.length > 2 &&
        positions.reduce((prev, curr, i) => prev && (i === 0 || IsAdjacent(positions[i - 1], curr)), true);
}

export const IsAdjacent = (position1: Position, position2: Position): boolean => {
    return (position1.xPosition === position2.xPosition && Math.abs(position1.yPosition - position2.yPosition) === 1) ||
        (position1.yPosition === position2.yPosition && Math.abs(position1.xPosition - position2.xPosition) === 1) ||
        (position1.xPosition - position2.xPosition) * (position1.yPosition - position2.yPosition) === -1;
}

// Overaching game rule preferences north and west to break ties, hence this direction is hardcoded 
// Assume two or more positions and no duplicates are received
export const GetNorthWestiest = (positions: Position[]) => {
    const getNorthValue = (p: Position) => p.xPosition + 2 * p.yPosition;
    return positions
        .reduce((prev, curr) => getNorthValue(prev) < getNorthValue(curr) || prev.xPosition < curr.xPosition ? prev : curr);
}

export const GetRing = (position: Position, size: number = 1): Position[] => {

    if (size === 0) {
        return [{ xPosition: 0, yPosition: 0 }];
    }

    let positions: Position[] = [];

    Range(0, size).forEach(i => {
        // Northeast side
        positions.push({ xPosition: i, yPosition: size * -1 });
        // Southwest side
        positions.push({ xPosition: i * -1, yPosition: size });
    });

    if (size > 0) {
        Range(0, size - 1).forEach(i => {
            // East side
            positions.push({ xPosition: size, yPosition: i * -1 });
            // West side
            positions.push({ xPosition: size * -1, yPosition: i });
        });
    }

    if (size > 1) {
        Range(1, size - 1).forEach(i => {
            // Southeast side
            positions.push({ xPosition: size - i, yPosition: i });
            // Northwest side
            positions.push({ xPosition: i - size, yPosition: i * -1 });
        });
    }

    return positions.map(x => Add(x, position)).filter(IsValidPosition);
}

export const GetAllNearest = (position: Position, targets: Position[], startRange: number = 1, endRange: number = MAX_DISTANCE): Position[] | null => {
    for (var i = startRange; i <= endRange; i++) {
        const foundTargets = GetRing(position, i).filter(p => ContainsPosition(targets, p));
        if (foundTargets.length > 0) {
            return foundTargets;
        }
    }
    return null;
}

export const GetNearest = (position: Position, targets: Position[], startRange: number = 1, endRange: number = MAX_DISTANCE) => {
    const foundTargets = GetAllNearest(position, targets, startRange, endRange);
    if (foundTargets) {
        return GetNorthWestiest(targets);
    }
}