import { Corner, Direction } from "../../utils/apiModels";
import e_north from "./e_north.png";
import e_northeast from "./e_northeast.png";
import e_northwest from "./e_northwest.png";
import e_south from "./e_south.png";
import e_southeast from "./e_southeast.png";
import e_southwest from "./e_southwest.png";
import c_east from "./c_east.png";
import c_northeast from "./c_northeast.png";
import c_northwest from "./c_northwest.png";
import c_southeast from "./c_southeast.png";
import c_southwest from "./c_southwest.png";
import c_west from "./c_west.png";

export const borderIconMap : {[key in Direction | Corner]: string} = {
    [Direction.North] : e_north,
    [Direction.NorthEast]: e_northeast,
    [Direction.NorthWest]: e_northwest,    
    [Direction.South]: e_south,
    [Direction.SouthEast]: e_southeast,
    [Direction.SouthWest]: e_southwest,    
    [Corner.East]: c_east,
    [Corner.NorthEast]: c_northeast,
    [Corner.NorthWest]: c_northwest,    
    [Corner.SouthEast]: c_southeast,
    [Corner.SouthWest]: c_southwest,
    [Corner.West]: c_west
}

