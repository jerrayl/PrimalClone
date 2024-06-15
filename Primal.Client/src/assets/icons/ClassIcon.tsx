import { Class } from "../../utils/apiModels";
import Blade from "./Blade.svg?react";
import Cur from "./Cur.svg?react";
import Exile from "./Exile.svg?react";
import Grovemaiden from "./GroveMaiden.svg?react";
import Harbinger from "./Harbinger.svg?react";
import Huntress from "./Huntress.svg?react";
import Penitent from "./Penitent.svg?react";
import Priest from "./Priest.svg?react";
import Ranger from "./Ranger.svg?react";
import Warbear from "./Warbear.svg?react";
import Warden from "./Warden.svg?react";
import Witch from "./Witch.svg?react";

export interface ClassIconProps {
    classType: Class;
    className: string;
}

export const ClassIcon = ({ classType, className }: ClassIconProps) => {
    return (classIconMap[classType]({ title: classType.toString(), className: className }))
}

const classIconMap = {
    [Class.Blade]: Blade,
    [Class.Cur]: Cur,
    [Class.Exile]: Exile,
    [Class.GroveMaiden]: Grovemaiden,
    [Class.Harbinger]: Harbinger,
    [Class.Huntress]: Huntress,
    [Class.Penitent]: Penitent,
    [Class.Priest]: Priest,
    [Class.Ranger]: Ranger,
    [Class.Warbear]: Warbear,
    [Class.Warden]: Warden,
    [Class.Witch]: Witch
}

