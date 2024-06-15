import { observer } from "mobx-react";
import { StartEncounterForm } from "./MainMenuStore";
import { Modal } from "../SharedComponents/Modal";

export interface StartEncounterProps {
    form: StartEncounterForm;
    closeModal: () => void;
    submitForm: () => void;
}

export const StartEncounterModal = observer(({ form, closeModal, submitForm }: StartEncounterProps) => {
    return (
        <Modal title="Start Encounter" buttonText="Start" closeModal={closeModal} submitForm={submitForm}>
            <div className="grid grid-cols-6 gap-2 p-5">
                {[...Array(20).keys()].map(x => x+1).map(number =>
                    <div className={`border text-xl cursor-pointer font-bold border-transparent rounded h-12 w-12 flex items-center justify-center ${number === form.number ? "text-accent2 hover:border-accent2 " : "text-accent1 hover:border-accent1"}`} onClick={() => form.number = number}>
                        {number}
                    </div>
                )}
            </div>
        </Modal>
    );
});
