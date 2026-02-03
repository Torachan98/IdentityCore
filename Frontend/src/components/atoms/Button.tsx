interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  isTransparent?: boolean;
}

export default function Button({
  children,
  isTransparent = false,
  ...props
}: Props) {
  return (
    <button
      {...props}
      className={`rounded-lg ${isTransparent ? "bg-transparent" : "bg-blue-600"} py-2 text-sm font-semibold
        text-white hover: ${isTransparent ? "bg-transparent" : "bg-blue-700"} disabled:opacity-60 ${props.className ?? ""}`}
    >
      {children}
    </button>
  );
}
