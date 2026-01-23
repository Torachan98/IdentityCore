type Props = {
  children: React.ReactNode;
};

export function Icon({ children }: Props) {
  return <span className="text-lg">{children}</span>;
}
