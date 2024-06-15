import { IObservableArray, makeAutoObservable, observable } from "mobx";
import { ContainsPosition, Equal } from "../Game/utils/gridHelper";
import { Position } from "../utils/apiModels";

export class EncounterBuilderStore {
  selectedPositions: IObservableArray<Position> = observable.array();

  constructor() {
    makeAutoObservable(this);
  }

  selectTile = (position: Position) => {
    if (ContainsPosition(this.selectedPositions, position)) {
      return;
    }

    this.selectedPositions.push(position);
  }

  deselectTile = (position: Position) => {
    this.selectedPositions = observable.array(this.selectedPositions.filter(x => !Equal(x, position)));
  }


  getTileColor = (position: Position): string => {
    return ContainsPosition(this.selectedPositions, position) ? "bg-bg5"  : "bg-bg3";
  }
}