interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {}

export default function Button({ children, ...props }: Props) {
  return (
    <button
      {...props}
      className={`rounded-lg bg-blue-600 py-2 text-sm font-semibold
        text-white hover:bg-blue-700 disabled:opacity-60 ${props.className ?? ""}`}
    >
      {children}
    </button>
  );
}
