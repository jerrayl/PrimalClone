import { makeAutoObservable } from "mobx";
import { GameStateModel } from "../../utils/apiModels";
import { BoardStore } from "./BoardStore";
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ENCOUNTER_ID, SIGNALR_CODES } from "../utils/constants";
import Cookies from 'js-cookie';

export class GameStore {
  connection: HubConnection;
  gameState?: GameStateModel;
  boardStore: BoardStore = new BoardStore(() => this.gameState!);

  constructor() {
    makeAutoObservable(this);
    const newConnection = new HubConnectionBuilder()
      .withUrl('signalr')
      .withAutomaticReconnect()
      .build();

    newConnection.on('GameState', message => {
      this.gameState = message;
    });
    this.connection = newConnection;
    this.startConnection();
  }

  startConnection = async () => {
    await this.connection.start();
    this.register();
  }

  register = async () => {
    const encounterId = Cookies.get(ENCOUNTER_ID);
    try {
      const response = await this.connection.invoke("register", Number(encounterId));
      if (response === SIGNALR_CODES.SUCCESS) {
        console.log("successfully connected to SignalR");
      }
    }
    catch (e) {
      console.log(e);
    }
  }
}