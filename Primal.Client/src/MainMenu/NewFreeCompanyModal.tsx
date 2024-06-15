import { observer } from "mobx-react";
import { NewFreeCompanyForm } from "./MainMenuStore";
import { Modal } from "../SharedComponents/Modal";
import { TextInput } from "../SharedComponents/TextInput";

export interface NewFreeCompanyProps {
    form: NewFreeCompanyForm;
    closeModal: () => void;
    submitForm: () => void;
}

export const NewFreeCompanyModal = observer(({ form, closeModal, submitForm }: NewFreeCompanyProps) => {
    return (
        <Modal title="New Free Company" buttonText="Create Free Company" closeModal={closeModal} submitForm={submitForm}>
            <TextInput
                id="name"
                placeholder="Name"
                value={form.name}
                onChange={(newValue: string) => form.name = newValue}
            />
        </Modal>
    );
});
