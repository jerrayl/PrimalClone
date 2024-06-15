export interface HealthDiceProps {
    health: number;
    className: string;
}

export const HealthDice = ({ health, className }: HealthDiceProps) => {
    return (
        <div className={"rounded-lg shadow-md bg-rose-800 text-amber-300 justify-center font-medium text-xl flex items-center " + className}>
            {health}
        </div>
    )
}