import { observer } from "mobx-react";
import { AttackModal } from "./AttackModal";
import { MoveModal } from "./MoveModal";
import { BoardStore } from "../stores/BoardStore";
import { PlayerSummary } from "./PlayerSummary";
import { BossSummary } from "./BossSummary";
import { DisplayAttackModal } from "./DisplayAttackModal";
import { CharacterType, Class } from "../../utils/apiModels";
import { BOARD, BOARD_MAPPING } from "../utils/constants";
import { ClassIcon } from "../../assets/icons/ClassIcon";
import { Button } from "../../SharedComponents/Button";

export interface BoardProps {
  boardStore: BoardStore;
}

export const Board = observer(({ boardStore }: BoardProps) => {
  return (
    <div>
      {boardStore.attackStore && <AttackModal attackStore={boardStore.attackStore} bossPart={boardStore.targetedBossPart} closeModal={() => boardStore.attackStore = null} />}
      {boardStore.pendingMove && <MoveModal cost={boardStore.selectedPath.length} move={boardStore.move} closeModal={boardStore.cancelMove} />}
      {boardStore.getGameState().attack && <DisplayAttackModal displayAttackModel={boardStore.getGameState().attack} continueAction={boardStore.continueEnemyAction} />}
      <div className="flex caret-transparent content-center justify-center mt-16">
        <div className="flex flex-col justify-evenly">
          <div className="flex justify-around">
            {
              !boardStore.getGameState().characterPerformingAction ?
                <Button text="End Turn" onClick={() => boardStore.endTurn()} /> :
                boardStore.getGameState().characterPerformingAction != CharacterType.Player &&
                <Button text="Continue Enemy Action" onClick={() => boardStore.continueEnemyAction()} />
            }
          </div>
          <BossSummary boss={boardStore.getGameState().boss} selectedBossPart={boardStore.selectedBossPart ? boardStore.selectedBossPart.bossPart : null} />
        </div>
        <div className="main flex">
          <div className="container">
            {
              BOARD.map((_, i) => BOARD_MAPPING[i]).map((position, i) => {
                const occupant = boardStore.getTileOccupant(position);
                return <div
                  key={i}
                  className={boardStore.getTileColor(position)}
                  onClick={() => { boardStore.selectTile(position) }}
                >
                  {occupant && (occupant.type == CharacterType.Player 
                    ? <ClassIcon className="w-14 h-14 mt-2 ml-2" classType={occupant.content as Class} />
                    : <img className="w-14 h-14 mt-2 ml-2" src={occupant.content as string} alt={occupant.description} />)}
                </div>
              })
            }
          </div>
        </div>
        <div className="flex flex-col gap-4">
          {boardStore.getGameState().players.map(x => <PlayerSummary key={x.class} player={x} isSelected={!!boardStore.selectedPlayer && x.id === boardStore.selectedPlayer.id} />)}
        </div>
      </div>
    </div>
  );
});