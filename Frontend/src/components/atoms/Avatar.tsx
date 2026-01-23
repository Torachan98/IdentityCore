type Props = {
  firstName: string;
  lastname: string;
};

export function Avatar({ firstName, lastname }: Props) {
  return (
    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-blue-600 text-white font-semibold">
      {firstName.charAt(0).toUpperCase()}
      {lastname.charAt(0).toUpperCase()}
    </div>
  );
}
