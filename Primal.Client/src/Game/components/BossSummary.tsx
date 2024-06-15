import { observer } from "mobx-react";
import { BossModel } from "../../utils/apiModels";
import { MightDisplay } from "./shared/MightDisplay";
import { HealthDice } from "./shared/HealthDice";
import { DefenceDisplay } from "./shared/DefenceDisplay";
import { formatBossPart } from "../utils/constants";

export interface BossSummaryProps {
  boss: BossModel;
  selectedBossPart: string | null;
}

export const BossSummary = observer(({ boss, selectedBossPart }: BossSummaryProps) => {
  return (
    <div className="rounded-lg overflow-hidden shadow-xl text-center px-2 py-2 caret-transparent bg-bg4 min-w-[16vw]">
      <div className="justify-center text-xl font-bold mb-2">
        {boss.name}
      </div>
      <div className="flex flex-col gap-2 justify-center items-center">
        {Object.keys(boss.health).map(bossPart =>
          <div key={bossPart} className="flex items-center">
            <h2 className={`mr-2 ${bossPart === selectedBossPart ? "font-bold" : "font-semibold"}`}>{formatBossPart(bossPart)}</h2>
            <HealthDice health={boss.health[bossPart]} className="w-10 h-10" />
          </div>
        )}
      </div>
      <div className="flex items-center justify-evenly">
        <DefenceDisplay defence={boss.defence} className="w-10 h-10" />
        <MightDisplay might={boss.might} length={5} className="flex justify-center gap-2" />
      </div>
      <div className="justify-center text-xl font-semibold">
        Next Action
      </div>
      <div className="flex flex-col gap-2 justify-center items-center">
        {boss.nextAction.map((actionComponent, i) =>
          <div key={`boss-action-${i}`} className="flex items-center">
            <h2 className={boss.actionComponentIndex === i ? "font-bold" : "font-semibold"}>{actionComponent}</h2>
          </div>
        )}
      </div>
    </div >
  );
});