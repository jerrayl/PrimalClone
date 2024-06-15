import { observer } from "mobx-react";
import { Button } from "./Button";
import { PropsWithChildren } from "react";

export interface ModalProps {
    title: string;
    buttonText: string;
    closeModal: () => void;
    submitForm: () => void;
}

export const Modal = observer(({ title, buttonText, closeModal, submitForm, children }: PropsWithChildren<ModalProps>) => {
    return (
        <div className="font-serif fixed z-10 inset-0 flex items-center justify-center min-h-full bg-bg2 bg-opacity-60">
            <div className="rounded-lg overflow-hidden shadow-xl max-w-lg min-w-[25%] bg-bg2 text-text2 px-6 py-6 text-center">
                <div className="flex justify-between mb-4">
                    <h3 className="text-xl font-medium">
                        {title}
                    </h3>
                    <h3
                        className="text-xl font-sans cursor-pointer font-medium -mt-1"
                        onClick={closeModal}>
                        x
                    </h3>
                </div>
                {children}
                <div>
                    <div className="flex justify-center mt-4">
                        <Button text={buttonText} onClick={submitForm} isPrimary/>
                    </div>
                </div>
            </div>
        </div>
    );
});
