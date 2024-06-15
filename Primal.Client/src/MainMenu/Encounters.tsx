
import { EncounterModel } from "../utils/apiModels";
import { observer } from "mobx-react";

export interface EncountersProps {
    encounters: EncounterModel[];
    continueEncounter: (encounterId: number) => void;
}

export const Encounters = observer(({ encounters, continueEncounter }: EncountersProps) => {
    return (
        <div className="flex flex-col items-center w-full">
            <div className="text-xl font-bold">
                Encounters in progress
            </div>
            <div className="mt-3 overflow-y-auto grid grid-cols-1 gap-4">
                {encounters.map(encounter =>
                    <div className="relative flex flex-col items-center rounded-md border bg-bg3 border-bg4 hover:border-bg5 shadow-md">
                        <div className="p-3 flex justify-between w-full items-center">
                            <h4 className="text-lg font-bold justify-self-center">{encounter.freeCompanyName}</h4>
                            <div className="font-semibold justify-self-end pl-4">
                                <button className="hover:text-text3" onClick={() => continueEncounter(encounter.encounterId)}>Continue</button>
                            </div>
                        </div>
                        <div className="p-3 flex justify-between w-full items-center border-t border-bg5">
                            <h4 className="text-xl font-bold justify-self-center">Encounter {encounter.encounterNumber}</h4>
                            <p className="text-sm font-medium text-text3 pl-4">{new Date(encounter.dateStarted).toLocaleString()}</p>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
});