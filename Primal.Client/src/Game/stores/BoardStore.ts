import { IObservableArray, makeAutoObservable, observable } from "mobx";
import { ContainsPosition, Equal, GetAllNearest, IsAdjacent } from "../utils/gridHelper";
import { CharacterType, GameStateModel, Position } from "../../utils/apiModels";
import { continueEnemyAction, endTurn, move } from "../../utils/api";
import { AttackStore } from "./AttackStore";
import { TileOccupant } from "../utils/types";
import { borderIconMap } from "../../assets/icons/BorderIcons";
import { bossIconMap } from "../../assets/icons/BossIcons";

export class BoardStore {
  getGameState: () => GameStateModel;
  selectedPosition: Position | null = null;
  selectedPath: IObservableArray<Position> = observable.array();
  attackStore: AttackStore | null = null;
  pendingMove: boolean = false;

  constructor(getGameState: () => GameStateModel) {
    makeAutoObservable(this);
    this.getGameState = getGameState;
  }

  get selectedBossPart() {
    return this.selectedCharacter && this.selectedCharacter.type == CharacterType.Boss
      ? this.getGameState().boss.positions.filter(p => p.xPosition == this.selectedPosition?.xPosition && p.yPosition == this.selectedPosition?.yPosition)[0]
      : null;
  }

  get selectedPlayer() {
    return this.selectedCharacter && this.selectedCharacter.type == CharacterType.Player
      ? this.getGameState().players.filter(x => x.id == this.selectedCharacter!.id)[0]
      : null;
  }

  get selectedCharacter() {
    return this.selectedPosition && this.getTileOccupant(this.selectedPosition);
  }

  get targetedBossPart() {
    return this.attackStore && this.getGameState().boss.positions.filter(x => Equal(x, this.attackStore!.target))[0].bossPart;
  }

  selectTile = (position: Position) => {
    if (this.selectedPath.length > 0 && ContainsPosition(this.selectedPath, position) && !Equal(this.selectedPath[this.selectedPath.length - 1], position)) {
      // Clicked on a tile what was not the last in the path
      return;
    }

    const occupant = this.getTileOccupant(position);
    console.log(this.selectedPath);

    if (occupant?.type == CharacterType.Boss &&
      (this.selectedPlayer && IsAdjacent(this.selectedPlayer, position) ||
        this.selectedPath.length > 0 && IsAdjacent(this.selectedPath[this.selectedPath.length-1], position)
      )
    ) {
      const boss = this.getGameState().boss;
      const validBossPositions = boss.positions.filter(x => boss.health[x.bossPart] > 0);
      const nearestBossPositions = GetAllNearest(this.selectedPlayer!, validBossPositions);
      if (nearestBossPositions && ContainsPosition(nearestBossPositions, position)) {
        this.attack(position);
        return;
      }

      this.selectedPosition = null;
      this.selectedPath = observable.array();
      return;
    }

    if (occupant) {
      this.selectedPosition = position;
      this.selectedPath = observable.array();
      return;
    }

    if (this.selectedPlayer && this.selectedPlayer.currentAnimus > this.selectedPath.length &&
      (this.selectedPath.length === 0 && IsAdjacent(this.selectedPosition!, position) ||
        this.selectedPath.length > 0 && IsAdjacent(this.selectedPath[this.selectedPath.length - 1], position))
    ) {
      // Valid continuation of path
      this.selectedPath.push(position);
      return;
    }

    if (this.selectedPath.length > 0 && Equal(this.selectedPath[this.selectedPath.length - 1], position)) {
      this.pendingMove = true;
      return;
    }

    this.selectedPosition = null;
    this.selectedPath = observable.array();
  }

  cancelMove = () => {
    this.pendingMove = false;
    this.selectedPosition = null;
    this.selectedPath = observable.array();
  }

  attack = (target: Position) => {
    if (this.selectedPlayer) {
      this.attackStore = new AttackStore(this.selectedPlayer, target);
    }
  }

  move = async () => {
    if (this.selectedPlayer && this.selectedPath.length > 0) {
      await move({ playerId: this.selectedPlayer.id, positions: this.selectedPath });
      this.selectedPosition = null;
      this.selectedPath = observable.array();
      this.pendingMove = false;
    }
  }

  endTurn = async () => {
    await endTurn();
  }

  continueEnemyAction = async () => {
    await continueEnemyAction();
  }

  tileIsHighlighted = (position: Position | null): boolean => {
    return ContainsPosition(this.selectedPath, position);
  }

  getTileColor = (position: Position): string => {
    return Equal(this.selectedPosition, position) ? "bg-bg5" : this.tileIsHighlighted(position) ? "bg-bg4" : "bg-bg3";
  }

  getTileOccupant = (position: Position): TileOccupant | undefined => {
    const gameState = this.getGameState();
    const player = gameState.players.filter(player => Equal(player, position)).find(x => x);
    const bossPosition = gameState.boss.positions.filter(bossPosition => Equal(bossPosition, position)).find(x => x);
    const bossBorder = bossPosition?.corner ?? bossPosition?.direction ?? "";
    return player ? { id: player.id, description: player.class.toString(), content: player.class, type: CharacterType.Player } :
      bossPosition ? { id: gameState.boss.id, description: "Boss " + bossBorder.toString(), content: bossBorder ? borderIconMap[bossBorder] : bossIconMap[gameState.boss.number], type: CharacterType.Boss } :
        undefined;
  }
}