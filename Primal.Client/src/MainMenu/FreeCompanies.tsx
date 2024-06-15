
import { ClassIcon } from "../assets/icons/ClassIcon";
import { FreeCompanyModel } from "../utils/apiModels";
import { observer } from "mobx-react";

export interface FreeCompaniesProps {
    freeCompanies: FreeCompanyModel[];
    startEncounter: (freeCompany: FreeCompanyModel) => void;
}

export const FreeCompanies = observer(({ freeCompanies, startEncounter }: FreeCompaniesProps) => {
    return (
        <div className="flex flex-col items-center">
            <div className="text-xl font-bold text-text1">
                Free Companies
            </div>
            <div className="mt-3 overflow-y-auto grid grid-cols-1 gap-4">
                {freeCompanies.map((freeCompany, i) =>
                    <div key={`freeCompany${i}`} className="flex flex-col items-center rounded-md border bg-bg3 border-bg4 hover:border-bg5 shadow-md overflow-x-auto">
                        <div className="p-3 flex justify-between items-center w-full">
                            <h4 className="text-xl font-bold justify-self-center">{freeCompany.name}</h4>
                            <div className="font-semibold justify-self-end pl-4">
                                {freeCompany.players.length === 4
                                    ? <button className="hover:text-text3" onClick={() => startEncounter(freeCompany)}>Start Encounter</button>
                                    : <h4 className="text-xl italic text-text3">{freeCompany.code}</h4>
                                }
                            </div>
                        </div>
                        <div className="grid xl:grid-cols-2 border-bg5 border-t xl:[&>*:nth-child(even)]:border-l xl:[&>*:nth-last-child(n+3)]:border-b py-1 px-1">
                            {freeCompany.players.map((player, i) =>
                                <div key={`freeCompanyPlayer${i}`} className="grid grid-cols-3 border-bg5">
                                    <div className="p-3 flex justify-center items-center">
                                        <ClassIcon className="w-14 h-14" classType={player.class} />
                                    </div>
                                    <div className="p-3 col-span-2 flex flex-col justify-center items-center">
                                        <h4 className="text-xl font-bold">{player.name}</h4>
                                        <p className="text-sm font-medium text-text3">{player.class}</p>
                                        <p className="text-sm font-medium text-text3">{player.userEmail}</p>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
});