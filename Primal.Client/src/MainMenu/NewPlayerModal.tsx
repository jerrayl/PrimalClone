import { observer } from "mobx-react";
import { NewPlayerForm } from "./MainMenuStore";
import { Modal } from "../SharedComponents/Modal";
import { ClassIcon } from "../assets/icons/ClassIcon";
import { Class } from "../utils/apiModels";
import { TextInput } from "../SharedComponents/TextInput";

export interface NewPlayerProps {
    form: NewPlayerForm;
    closeModal: () => void;
    submitForm: () => void;
}

export const NewPlayerModal = observer(({ form, closeModal, submitForm }: NewPlayerProps) => {
    return (
        <Modal title="New Player" buttonText="Create Player" closeModal={closeModal} submitForm={submitForm}>
            <div className="grid grid-cols-6 gap-2 p-5">
                {Object.keys(Class).map(x => x as any as Class).map(cls =>
                    <div key={cls.toString()} className={`border border-transparent rounded h-12 w-12 ${cls === form.class ? "hover:border-accent2" : "hover:border-accent1"}`} onClick={() => form.class = cls}>
                        <ClassIcon className={`w-12 h-12 ${cls === form.class ? "fill-accent2" : "fill-accent1"}`} classType={cls} />
                    </div>
                )}
            </div>
            <TextInput
                id="name"
                placeholder="Name"
                value={form.name}
                onChange={(newValue: string) => form.name = newValue}
            />
        </Modal>
    );
});
