import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export function Input({
  icon,
  ...props
}: React.InputHTMLAttributes<HTMLInputElement> & {
  icon: IconProp;
}) {
  return (
    <div className="relative">
      <FontAwesomeIcon
        icon={icon}
        className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
      />
      <input {...props} className={`input w-full pl-10 ${props.className}`} />
    </div>
  );
}
