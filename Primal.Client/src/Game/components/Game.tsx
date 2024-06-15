import { Observer } from "mobx-react";
import { Board } from "./Board";
import { GameStore } from "../stores/GameStore";

export const Game = () => {
    const gameStore = new GameStore();

    return (
        <Observer>
            {() => gameStore.gameState ? <Board boardStore={gameStore.boardStore} /> : <div className="w-screen h-screen flex items-center justify-center bg-bg1"><h2>Loading...</h2></div>}
        </Observer>
    )
}