import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

interface Props extends React.SelectHTMLAttributes<HTMLSelectElement> {
  options: Option[];
}

interface Option {
  label: string;
  value: number | string;
}

export default function Select({ options, ...props }: Props) {
  return (
    <div className={`relative inline-block ${props.className}`}>
      <select
        {...props}
        className={`w-full appearance-none border p-2 pr-10 rounded focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500`}
      >
        {options.map((o) => (
          <option key={o.value} value={o.value}>
            {o.label}
          </option>
        ))}
      </select>

      <span className="pointer-events-none absolute inset-y-0 right-3 flex items-center text-gray-500">
        <FontAwesomeIcon icon={"caret-down"} />
      </span>
    </div>
  );
}
