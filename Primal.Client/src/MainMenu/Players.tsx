
import { Button } from "../SharedComponents/Button";
import { ClassIcon } from "../assets/icons/ClassIcon";
import { PlayerSummaryModel } from "../utils/apiModels";
import { observer } from "mobx-react";

export interface PlayersProps {
    players: PlayerSummaryModel[];
    showNewFreeCompanyModal: (player : PlayerSummaryModel) => void;
    showJoinFreeCompanyModal: (player : PlayerSummaryModel) => void;
    showNewPlayerModal: () => void;
}

export const Players = observer(({ players, showNewFreeCompanyModal, showJoinFreeCompanyModal, showNewPlayerModal }: PlayersProps) => {
    return (
        <div className="flex flex-col items-center w-full">
            <div className="grid grid-cols-3 w-full">
                <div className="text-xl font-bold col-start-2 flex justify-center">
                    Players
                </div>
                <div className="flex justify-end font-semibold">
                    <Button onClick={showNewPlayerModal} text="Create Player"/>
                </div>
            </div>
            <div className="mt-3 overflow-y-auto grid xl:grid-cols-2 grid-cols-1 gap-4">
                {players.map((player, i) =>
                    <div key={`player${i}`} className="relative flex flex-col items-center rounded-md border bg-bg3 border-bg4 hover:border-bg5 shadow-md">
                        <div className="grid grid-cols-6">
                            <div className="p-3 flex justify-center items-center col-span-2">
                                <ClassIcon className="w-14 h-14" classType={player.class} />
                            </div>
                            <div className="p-3 col-span-4 flex flex-col w-auto justify-center items-center">
                                <h4 className="text-xl font-bold">{player.name}</h4>
                                <p className="text-sm font-medium text-text3">{player.class}</p>
                            </div>
                        </div>
                        <div className="flex w-auto justify-center border-t divide-x py-1 px-2 border-bg5 divide-bg5">
                            <button className="font-semibold text-sm pr-2 py-1 hover:text-text3" onClick={() => showNewFreeCompanyModal(player)}>Create Free Company</button>
                            <button className="font-semibold text-sm pl-2 py-1 hover:text-text3" onClick={() => showJoinFreeCompanyModal(player)}>Join Free Company</button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
});