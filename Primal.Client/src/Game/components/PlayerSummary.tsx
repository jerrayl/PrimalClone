import { observer } from "mobx-react";
import { TokenIcon } from "../../assets/icons/TokenIcon";
import { PlayerModel, Token } from "../../utils/apiModels";
import { MightDisplay } from "./shared/MightDisplay";
import { HealthDice } from "./shared/HealthDice";
import { DefenceDisplay } from "./shared/DefenceDisplay";
import { ClassIcon } from "../../assets/icons/ClassIcon";

export interface PlayerSummaryProps {
  player: PlayerModel;
  isSelected: boolean;
}

export const PlayerSummary = observer(({ player, isSelected }: PlayerSummaryProps) => {
  return (
    <div className={`rounded-lg overflow-hidden shadow-xl text-center px-2 py-2 grid grid-cols-12 caret-transparent ${isSelected ? "bg-bg5" : "bg-bg4"} min-w-[16vw]`}>
      <div className="col-span-3 flex flex-col justify-center">
        <ClassIcon className="w-14 h-14" classType={player.class} />
      </div>
      <div className="col-span-9 flex flex-col gap-2">
        <div className="flex justify-evenly">
          <HealthDice health={player.currentHealth} className="w-12 h-12" />
          <DefenceDisplay defence={player.defence} className="w-12 h-12 pt-1" />
          <div className="animus rounded-lg shadow-md bg-amber-400 text-yellow-700 w-12 h-12 justify-center font-bold text-xl pt-2">
            {player.currentAnimus}
          </div>
        </div>
        <MightDisplay might={player.might} length={4} className="flex justify-center gap-2" />
        <div className="flex text-xl font-medium justify-evenly">
          {Object.entries(player.tokens).map(entry => <div key={`${player.id}-${entry[0]}`}><TokenIcon className="h-8 w-8" tokenType={entry[0] as unknown as Token} />{entry[1]}</div>)}
        </div>
      </div>
    </div >
  );
});