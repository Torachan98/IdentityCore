import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useEffect, useMemo, useRef, useState } from "react";

type Option = {
  label: string;
  value: string;
};

type Props = {
  options: Option[];
  value: string[];
  onChange: (values: string[]) => void;
  placeholder?: string;
  disabled?: boolean;
};

export function MultiSelect({
  options,
  value,
  onChange,
  placeholder = "Select...",
  disabled = false,
}: Props) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState("");
  const ref = useRef<HTMLDivElement>(null);

  // Close when clicking outside
  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (!ref.current?.contains(e.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, []);

  const optionMap = useMemo(() => {
    return new Map(options.map((o) => [o.value, o]));
  }, [options]);

  const filteredOptions = useMemo(() => {
    const keyword = search.toLowerCase();
    return options.filter((o) => o.label.toLowerCase().includes(keyword));
  }, [options, search]);

  const toggleValue = (val: string) => {
    onChange(
      value.includes(val) ? value.filter((v) => v !== val) : [...value, val],
    );
  };

  return (
    <div ref={ref} className="relative w-full">
      <div
        onClick={() => {
          if (!disabled) setOpen((v) => !v);
        }}
        className={`min-h-[42px] rounded border px-3 py-2 flex flex-wrap gap-2 items-center
                    ${
                      disabled
                        ? "bg-gray-100 text-gray-400 cursor-not-allowed"
                        : "cursor-pointer"
                    }
                  `}
      >
        {value.length === 0 && (
          <span className="text-gray-400">{placeholder}</span>
        )}

        {value.map((v) => {
          const opt = optionMap.get(v);
          return (
            <span
              key={v}
              className="bg-blue-100 text-blue-700 text-sm px-2 py-1 rounded flex items-center gap-1"
            >
              {opt?.label}
              {!disabled && (
                <button
                  onClick={(e) => {
                    e.stopPropagation();

                    if (!disabled) toggleValue(v);
                  }}
                  className="text-blue-500 hover:text-blue-700"
                >
                  <FontAwesomeIcon icon={"circle-xmark"} />
                </button>
              )}
            </span>
          );
        })}
      </div>

      {open && (
        <div className="absolute z-10 mt-1 w-full rounded border bg-white shadow">
          <input
            className="w-full border-b px-3 py-2 outline-none"
            placeholder="Search..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />

          <ul className="max-h-48 overflow-y-auto">
            {filteredOptions.length === 0 && (
              <li className="px-3 py-2 text-gray-400">No results</li>
            )}

            {filteredOptions.map((o) => (
              <li
                key={o.value}
                onClick={() => {
                  if (!disabled) toggleValue(o.value);
                }}
                className={`px-3 py-2 cursor-pointer hover:bg-gray-100 flex justify-between ${
                  value.includes(o.value) ? "bg-blue-100 font-medium" : ""
                }`}
              >
                {o.label}
                {value.includes(o.value) && <FontAwesomeIcon icon={"check"} />}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}
