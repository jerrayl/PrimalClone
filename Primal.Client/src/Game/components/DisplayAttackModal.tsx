import { observer } from "mobx-react";
import { CharacterType, DisplayAttackModel } from "../../utils/apiModels";
import { MightCardDisplay } from "./shared/MightCardDisplay";
import { Button } from "../../SharedComponents/Button";

export interface DisplayAttackModalProps {
    displayAttackModel: DisplayAttackModel;
    continueAction: () => Promise<void>;
}

export const DisplayAttackModal = observer(({ displayAttackModel, continueAction }: DisplayAttackModalProps) => {
    return (
        <div className="font-serif fixed z-10 inset-0 flex items-center justify-center min-h-full caret-transparent bg-bg2 bg-opacity-60">
            <div className="rounded-lg overflow-hidden shadow-xl max-w-lg min-w-[25%] bg-bg2 text-text2">
                <div className="px-6 py-6">
                    <div className="text-center">
                        <div className="flex justify-between">
                            <h3 className="text-xl font-medium">
                                {displayAttackModel.characterType} Attack
                            </h3>
                        </div>

                        <div className="flex flex-col px-4 py-8">
                            <div className="mt-2 font-lg text-lg">
                                {displayAttackModel.characterType === CharacterType.Player && displayAttackModel.cardsDrawn.filter(card => !card.isDrawnFromCritical && card.value === 0).length >= 2 ? "Miss" : `${displayAttackModel.cardsDrawn.reduce((partialSum, card) => partialSum + card.value, 0)} damage`}
                            </div>
                            <div className="grid grid-cols-6 gap-4 mt-3">
                                {displayAttackModel.cardsDrawn.filter(x => !x.isDrawnFromCritical).map((card, i) =>
                                    <MightCardDisplay
                                        key={i}
                                        mightCard={card}
                                    />)}
                            </div>
                            {displayAttackModel.characterType == CharacterType.Player &&
                                <>
                                    <hr className="h-2px my-6 bg-gray-700 border-0" />
                                    <div className="grid grid-cols-6 gap-4">
                                        {displayAttackModel.cardsDrawn.filter(x => x.isDrawnFromCritical).map((card, i) =>
                                            <MightCardDisplay key={i} mightCard={card} />)}
                                    </div>
                                </>
                            }
                        </div>
                        <div>
                            <div className="flex justify-center">
                                <Button text="Continue" onClick={async () => await continueAction()} isPrimary/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
});
