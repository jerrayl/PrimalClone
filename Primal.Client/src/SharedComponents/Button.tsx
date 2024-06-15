export interface ButtonProps {
    text: string;
    onClick: () => void;
    isPrimary?: boolean;
}

export const Button = ({ text, onClick, isPrimary }: ButtonProps) => {
    return (
        <button
            type="button"
            className={`inline-flex justify-center rounded-md shadow-sm px-6 py-2 text-lg font-semibold  ml-3 w-auto text-sm caret-transparent ${isPrimary ? "bg-bg4 text-text1 hover:text-accent2" : "bg-bg2 text-text2 hover:text-accent1"}`}
            onClick={onClick}
        >
            {text}
        </button>
    )
}