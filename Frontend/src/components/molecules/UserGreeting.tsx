import { Avatar } from "@/components/atoms/Avatar";

type Props = {
  firstName: string;
  lastName: string;
  collapsed: boolean;
};

export function UserGreeting({ firstName, lastName, collapsed }: Props) {
  return (
    <div className="flex items-center gap-3 px-3 py-4">
      <Avatar firstName={firstName} lastname={lastName} />

      {!collapsed && (
        <div className="text-sm">
          <p className="text-gray-500">Hello</p>
          <p className="font-semibold truncate">{firstName + " " + lastName}</p>
        </div>
      )}
    </div>
  );
}
