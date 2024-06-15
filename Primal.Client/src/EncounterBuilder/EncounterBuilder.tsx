import { Observer } from "mobx-react";
import { BOARD, BOARD_MAPPING } from "../Game/utils/constants";
import { EncounterBuilderStore } from "./EncounterBuilderStore";

export const EncounterBuilder = () => {

  const store = new EncounterBuilderStore();

  return (
    <Observer>
      {() =>
        <div>
          <div className="grid grid-cols-10 caret-transparent">
            <div className="flex flex-col col-span-2 ml-4 mt-3 items-center">
            <h2 className="text-sm">Left click to select tile, right click to deselect tile.</h2>

            </div>
            <div className="main flex mt-1 ml-4 col-span-6">
              <div className="container">
                {
                  BOARD.map((_, i) => BOARD_MAPPING[i]).map((position, i) => {
                    return <div
                      key={i}
                      className={store.getTileColor(position)}
                      onClick={() => store.selectTile(position)}
                      onContextMenu={(e) => {e.preventDefault();store.deselectTile(position)}}
                    >
                    </div>
                  })
                }
            </div>
          </div>
          <div className="col-span-2 mt-3 mr-4 flex flex-col gap-4 items-center">
            <h2 className="font-bold">Selected Tiles:</h2>
            {store.selectedPositions.map(p => <h3>{p.xPosition}, {p.yPosition}</h3>)}
          </div>
        </div>
        </div>
      }
    </Observer >
  );
};