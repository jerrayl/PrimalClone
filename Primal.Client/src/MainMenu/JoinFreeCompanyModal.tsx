import { observer } from "mobx-react";
import { JoinFreeCompanyForm } from "./MainMenuStore";
import { Modal } from "../SharedComponents/Modal";
import { TextInput } from "../SharedComponents/TextInput";

export interface NewFreeCompanyProps {
    form: JoinFreeCompanyForm;
    closeModal: () => void;
    submitForm: () => void;
}

export const JoinFreeCompanyModal = observer(({ form, closeModal, submitForm }: NewFreeCompanyProps) => {
    return (
        <Modal title="Join Free Company" buttonText="Join Free Company" closeModal={closeModal} submitForm={submitForm}>
            <TextInput
                id="code"
                placeholder="Code"
                value={form.code}
                onChange={(newValue: string) => form.code = newValue}
            />
        </Modal>
    );
});
