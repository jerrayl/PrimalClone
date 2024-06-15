export interface TextInputProps {
    id: string;
    placeholder: string;
    value: string;
    onChange: (newValue: string) => void;
}

export const TextInput = ({ id, placeholder, value, onChange }: TextInputProps) => {
    return (
        <input
            type="text"
            id={id}
            className="bg-bg4 border border-bg5 text-text1 rounded-lg placeholder-gray-500 focus:border-accent2 w-full p-2.5 outline-none"
            placeholder={placeholder}
            value={value}
            onChange={e => onChange(e.target.value)}
            required
        />
    )
}