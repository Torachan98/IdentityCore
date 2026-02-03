type InputProps = React.InputHTMLAttributes<HTMLInputElement> & {
  label?: string;
};

export function InputLabel({ label, className, ...props }: InputProps) {
  return (
    <div className="w-full flex flex-col gap-1">
      {label && (
        <label className="text-xs font-medium text-gray-500">{label}</label>
      )}
      <input
        {...props}
        className={`input w-full rounded-md border px-3 py-2 text-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500 ${className}`}
      />
    </div>
  );
}
