import { Token } from "../../utils/apiModels";
import Animus from "./Animus.svg?react";
import Battleflow from "./Battleflow.svg?react";
import Defence from "./Defence.svg?react";
import Empower from "./Empower.svg?react";
import Redraw from "./Redraw.svg?react";

export interface TokenIconProps {
    tokenType: Token;
    className: string;
}

export const TokenIcon = ({ tokenType, className }: TokenIconProps) => {
    return (tokenIconMap[tokenType]({ title: tokenType.toString(), className: className }))
}

const tokenIconMap = {
    [Token.Animus] : Animus,
    [Token.Battleflow]: Battleflow,
    [Token.Defence]: Defence,    
    [Token.Empower]: Empower,
    [Token.Redraw]: Redraw
}

