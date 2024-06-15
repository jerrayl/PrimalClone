import { makeAutoObservable } from "mobx";
import { Class, EncounterModel, FreeCompanyModel, PlayerSummaryModel } from "../utils/apiModels";
import { createFreeCompany, createPlayer, getEncounters, getFreeCompanies, getPlayers, joinFreeCompany, startEncounter } from "../utils/api";
import Cookies from 'js-cookie';
import { ENCOUNTER_ID } from "../Game/utils/constants";

export class NewPlayerForm {
  class: Class | null = null;
  name: string = '';

  constructor() {
    makeAutoObservable(this);
  }
}

export class JoinFreeCompanyForm {
  code: string = '';

  constructor() {
    makeAutoObservable(this);
  }
}

export class NewFreeCompanyForm {
  name: string = '';

  constructor() {
    makeAutoObservable(this);
  }
}

export class StartEncounterForm {
  number: number = 0;

  constructor() {
    makeAutoObservable(this);
  }
}


export class MainMenuStore {
  encounters: EncounterModel[] = [];
  freeCompanies: FreeCompanyModel[] = [];
  players: PlayerSummaryModel[] = [];
  newPlayerForm: NewPlayerForm | null = null;
  newFreeCompanyForm: NewFreeCompanyForm | null = null;
  joinFreeCompanyForm: JoinFreeCompanyForm | null = null;
  startEncounterForm: StartEncounterForm | null = null;
  selectedPlayer: PlayerSummaryModel | null = null;
  selectedFreeCompany: FreeCompanyModel | null = null;

  constructor() {
    makeAutoObservable(this);
    this.loadData();
  }

  loadData = async () => {
    this.players = await getPlayers();
    this.freeCompanies = await getFreeCompanies();
    this.encounters = await getEncounters();
  }

  createPlayer = async () => {
    if (!this.newPlayerForm || !this.newPlayerForm.class) {
      return;
    }
    await createPlayer({ name: this.newPlayerForm.name, class: this.newPlayerForm.class })
    this.newPlayerForm = null;
    await this.loadData();
  }

  createFreeCompany = async () => {
    if (!this.newFreeCompanyForm || !this.newFreeCompanyForm.name || !this.selectedPlayer) {
      return;
    }
    await createFreeCompany({ name: this.newFreeCompanyForm.name, playerId: this.selectedPlayer.id });
    this.newFreeCompanyForm = null;
    await this.loadData();
  }

  joinFreeCompany = async () => {
    if (!this.joinFreeCompanyForm || !this.joinFreeCompanyForm.code || !this.selectedPlayer) {
      return;
    }
    await joinFreeCompany({ code: this.joinFreeCompanyForm.code, playerId: this.selectedPlayer.id });
    this.joinFreeCompanyForm = null;
    await this.loadData();
  }

  startEncounter = async () => {
    if (!this.startEncounterForm || !this.startEncounterForm.number || !this.selectedFreeCompany){
      return;
    }

    const encounterId = await startEncounter({freeCompanyCode: this.selectedFreeCompany.code, encounterNumber: this.startEncounterForm.number});
    Cookies.set(ENCOUNTER_ID, encounterId, { secure: true });
    window.location.href = '/game';
  }

  continueEncounter = async (encounterId: number) => {
    Cookies.set(ENCOUNTER_ID, encounterId.toString(), { secure: true });
    window.location.href = '/game';
  }

  showNewPlayerModal = () => {
    this.newPlayerForm = new NewPlayerForm();
  }

  showNewFreeCompanyModal = (player: PlayerSummaryModel) => {
    this.selectedPlayer = player;
    this.newFreeCompanyForm = new NewFreeCompanyForm();
  }

  showJoinFreeCompanyModal = (player: PlayerSummaryModel) => {
    this.selectedPlayer = player;
    this.joinFreeCompanyForm = new JoinFreeCompanyForm();
  }

  showStartEncounterModal = (freeCompany: FreeCompanyModel) => {
    this.selectedFreeCompany = freeCompany;
    this.startEncounterForm = new StartEncounterForm();
  }
}