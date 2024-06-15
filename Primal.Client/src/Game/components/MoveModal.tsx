import { observer } from "mobx-react";
import { Modal } from "../../SharedComponents/Modal";

export interface MoveModalProps {
    cost: number;
    move: () => Promise<void>;
    closeModal: () => void;
}

export const MoveModal = observer(({ cost, move, closeModal }: MoveModalProps) => {
    return (
        <Modal
            title="Move"
            buttonText="Confirm"
            closeModal={closeModal}
            submitForm={move}
        >
            <h2 className="text-lg font-medium">
                Cost: {cost} Animus
            </h2>
        </Modal>
    );
});
