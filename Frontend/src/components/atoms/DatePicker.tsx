import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { use, useEffect, useRef, useState } from "react";

type DatePickerProps = {
  label?: string;
  value?: string;
  onChange: (value: string) => void;
  disabled?: boolean;
};

const days = ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"];

export default function DatePicker({
  label,
  value,
  onChange,
  disabled = false,
}: DatePickerProps) {
  const [open, setOpen] = useState(false);
  const [current, setCurrent] = useState<Date>(
    value ? new Date(value) : new Date(),
  );
  const [view, setView] = useState<"month" | "year">("month");
  const [position, setPosition] = useState({ top: 0, left: 0 });
  const [placement, setPlacement] = useState<"top" | "bottom">("bottom");
  const [yearBase, setYearBase] = useState<number>(
    (value ? new Date(value).getFullYear() : new Date().getFullYear()) - 6,
  );

  const containerRef = useRef<HTMLDivElement>(null);
  const buttonRef = useRef<HTMLButtonElement>(null);
  const popupRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open) return;

    const handleClickOutside = (e: MouseEvent) => {
      if (
        containerRef.current &&
        !containerRef.current.contains(e.target as Node)
      ) {
        setOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [open]);

  useEffect(() => {
    if (!open || !buttonRef.current) return;

    const buttonRect = buttonRef.current.getBoundingClientRect();
    const popupHeight = popupRef.current?.offsetHeight ?? 300;
    const viewportHeight = window.innerHeight;

    const spaceBelow = viewportHeight - buttonRect.bottom;
    const spaceAbove = buttonRect.top;

    if (spaceBelow < popupHeight && spaceAbove > popupHeight) {
      setPlacement("top");
      setPosition({
        top: buttonRect.top - popupHeight - 8,
        left: buttonRect.left,
      });
    } else {
      setPlacement("bottom");
      setPosition({
        top: buttonRect.bottom + 8,
        left: buttonRect.left,
      });
    }
  }, [open, view]);

  const year = current.getFullYear();
  const month = current.getMonth();
  const firstDay = new Date(year, month, 1).getDay();
  const daysInMonth = new Date(year, month + 1, 0).getDate();

  const selected =
    value && !isNaN(new Date(value).getTime())
      ? new Date(value).toDateString()
      : null;

  const years = Array.from({ length: 12 }, (_, i) => yearBase + i);

  return (
    <div ref={containerRef} className="w-full">
      {label && (
        <label className="mb-1 block text-sm font-medium text-gray-600">
          {label}
        </label>
      )}

      <button
        ref={buttonRef}
        type="button"
        disabled={disabled}
        onClick={() => setOpen((v) => !v)}
        className={`flex w-full items-center justify-between rounded-lg border px-3 py-2 text-sm
          ${
            disabled
              ? "cursor-not-allowed bg-gray-100 text-gray-400"
              : "bg-white text-gray-700 hover:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
          }`}
      >
        <span>
          {value ? new Date(value).toLocaleDateString() : "Select date"}
        </span>
        <FontAwesomeIcon icon={"calendar-days"} className="text-gray-400" />
      </button>

      {open && !disabled && (
        <div
          ref={popupRef}
          style={{ top: position.top, left: position.left }}
          className="fixed z-[9999] w-72 rounded-xl border bg-white p-4 shadow-xl"
        >
          <div className="mb-3 flex items-center justify-between">
            {view === "month" && (
              <>
                <button
                  onClick={() => setCurrent(new Date(year, month - 1, 1))}
                  className="rounded px-2 py-1 text-gray-500 hover:bg-gray-100"
                >
                  <FontAwesomeIcon icon={"angle-left"} />
                </button>

                <button
                  onClick={() => setView("year")}
                  className="text-sm font-semibold text-gray-700 hover:underline"
                >
                  {current.toLocaleString("default", {
                    month: "long",
                    year: "numeric",
                  })}
                </button>

                <button
                  onClick={() => setCurrent(new Date(year, month + 1, 1))}
                  className="rounded px-2 py-1 text-gray-500 hover:bg-gray-100"
                >
                  <FontAwesomeIcon icon={"angle-right"} />
                </button>
              </>
            )}
          </div>

          {view === "month" && (
            <>
              <div className="grid grid-cols-7 text-center text-xs text-gray-500">
                {days.map((d) => (
                  <div key={d}>{d}</div>
                ))}
              </div>

              <div className="mt-2 grid grid-cols-7 gap-1">
                {Array.from({ length: firstDay }).map((_, i) => (
                  <div key={i} />
                ))}

                {Array.from({ length: daysInMonth }).map((_, i) => {
                  const date = new Date(year, month, i + 1);
                  const isSelected = selected === date.toDateString();

                  return (
                    <button
                      key={i}
                      onClick={() => {
                        onChange(date.toUTCString());
                        setOpen(false);
                      }}
                      className={`rounded-md py-1 text-sm
                        ${
                          isSelected
                            ? "bg-blue-600 text-white"
                            : "hover:bg-blue-50"
                        }`}
                    >
                      {i + 1}
                    </button>
                  );
                })}
              </div>
            </>
          )}

          {view === "year" && (
            <div className="grid max-h-64 grid-cols-4 gap-2 overflow-auto">
              {years.map((y) => (
                <button
                  key={y}
                  onClick={() => {
                    setCurrent(new Date(y, month, 1));
                    setView("month");
                  }}
                  className={`rounded-md py-2 text-sm
                    ${
                      y === year ? "bg-blue-600 text-white" : "hover:bg-blue-50"
                    }`}
                >
                  {y}
                </button>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
}
