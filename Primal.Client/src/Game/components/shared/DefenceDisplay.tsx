export interface DefenceDisplayProps {
    defence: number;
    className: string;
}

export const DefenceDisplay = ({ defence, className }: DefenceDisplayProps) => {
    return (
        <div className={"defense rounded-lg shadow-md bg-sky-300 text-zinc-500 justify-center font-medium text-xl " + className}>
          {defence}
        </div>
    )
}