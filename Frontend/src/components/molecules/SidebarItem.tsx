type Props = {
  icon: React.ReactNode;
  label: string;
  collapsed: boolean;
  active: boolean;
  onClick?: () => void;
};

export function SidebarItem({
  icon,
  label,
  collapsed,
  active,
  onClick,
}: Props) {
  return (
    <button
      onClick={onClick}
      className={`flex w-full items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium  ${active ? 'bg-blue-200 text-white-700 hover:bg-white-100' : 'text-gray-700 hover:bg-gray-100'}`}
    >
      {icon}
      {!collapsed && (
        <span className={`truncate ${active ? 'text-white-700' : ''}`}>
          {label}
        </span>
      )}
    </button>
  );
}
