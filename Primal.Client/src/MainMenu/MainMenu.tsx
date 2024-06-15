import { Observer } from "mobx-react";
import primallogo from "../assets/primallogo.png";
import { MainMenuStore } from "./MainMenuStore";
import { Players } from "./Players";
import { FreeCompanies } from "./FreeCompanies";
import { NewPlayerModal } from "./NewPlayerModal";
import { NewFreeCompanyModal } from "./NewFreeCompanyModal";
import { JoinFreeCompanyModal } from "./JoinFreeCompanyModal";
import { Encounters } from "./Encounters";
import { StartEncounterModal } from "./StartEncounterModal";

export const MainMenu = () => {
  const store = new MainMenuStore();

  return (
    <Observer>
      {() =>
        <div className="bg-bg1 text-text1 overflow-y-hidden">
          {store.newPlayerForm && <NewPlayerModal form={store.newPlayerForm} closeModal={() => store.newPlayerForm = null} submitForm={store.createPlayer}/>}
          {store.newFreeCompanyForm && <NewFreeCompanyModal form={store.newFreeCompanyForm} closeModal={() => store.newFreeCompanyForm = null} submitForm={store.createFreeCompany}/>}
          {store.joinFreeCompanyForm && <JoinFreeCompanyModal form={store.joinFreeCompanyForm} closeModal={() => store.joinFreeCompanyForm = null} submitForm={store.joinFreeCompany}/>}
          {store.startEncounterForm && <StartEncounterModal form={store.startEncounterForm} closeModal={() => store.startEncounterForm = null} submitForm={store.startEncounter}/>}

          <div className="flex flex-col caret-transparent">
            <div className="flex justify-center h-[20vh]">
              <img className="h-full" src={primallogo} alt="primal logo" />
            </div>
            <div className="grid lg:grid-cols-9 overflow-y-auto h-[80vh] caret-transparent">
              <div className="col-span-3 mt-2 mr-4 flex flex-col gap-4 mx-2">
                <Encounters encounters={store.encounters} continueEncounter={store.continueEncounter}/>
              </div>
              <div className="col-span-3 mt-4 lg:mt-0 mx-2">
                <FreeCompanies freeCompanies={store.freeCompanies} startEncounter={store.showStartEncounterModal} />
              </div>
              <div className="col-span-3 mt-4 lg:mt-0 mx-2 lg:mr-4 mb-4 lg:mb-0">
                <Players players={store.players} showNewFreeCompanyModal={store.showNewFreeCompanyModal} showJoinFreeCompanyModal={store.showJoinFreeCompanyModal} showNewPlayerModal={store.showNewPlayerModal} />
              </div>
            </div>
          </div>
        </div>}
    </Observer>
  );
};