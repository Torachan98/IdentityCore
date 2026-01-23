import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useEffect, useRef, useState } from "react";

type DateTimePickerProps = {
  label?: string;
  value?: string;
  onChange: (value: string) => void;
  disabled?: boolean;
};

const days = ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"];

export default function DateTimePicker({
  label,
  value,
  onChange,
  disabled = false,
}: DateTimePickerProps) {
  const initialDate = value ? new Date(value) : new Date();

  const [open, setOpen] = useState(false);
  const [current, setCurrent] = useState<Date>(initialDate);
  const [view, setView] = useState<"month" | "year">("month");
  const [yearBase, setYearBase] = useState(initialDate.getFullYear() - 6);
  const [position, setPosition] = useState({ top: 0, left: 0 });

  const containerRef = useRef<HTMLDivElement>(null);
  const buttonRef = useRef<HTMLButtonElement>(null);
  const popupRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open) return;

    const handler = (e: MouseEvent) => {
      if (
        containerRef.current &&
        !containerRef.current.contains(e.target as Node)
      ) {
        setOpen(false);
      }
    };

    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, [open]);

  useEffect(() => {
    if (!open || !buttonRef.current) return;

    const btn = buttonRef.current.getBoundingClientRect();
    const popupHeight = popupRef.current?.offsetHeight ?? 360;
    const viewportHeight = window.innerHeight;

    const spaceBelow = viewportHeight - btn.bottom;
    const spaceAbove = btn.top;

    if (spaceBelow < popupHeight && spaceAbove > popupHeight) {
      setPosition({
        top: btn.top - popupHeight - 8,
        left: btn.left,
      });
    } else {
      setPosition({
        top: btn.bottom + 8,
        left: btn.left,
      });
    }
  }, [open, view]);

  const year = current.getFullYear();
  const month = current.getMonth();
  const day = current.getDate();
  const hour = current.getHours();
  const minute = current.getMinutes();
  const second = current.getSeconds();

  const firstDay = new Date(year, month, 1).getDay();
  const daysInMonth = new Date(year, month + 1, 0).getDate();
  const years = Array.from({ length: 12 }, (_, i) => yearBase + i);

  const updateTime = (h: number, m: number, s: number) => {
    const d = new Date(current);
    d.setHours(h, m, s);
    setCurrent(d);
    onChange(d.toUTCString());
  };

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
              : "bg-white text-gray-700 hover:border-blue-500 focus:ring-2 focus:ring-blue-500"
          }`}
      >
        <span>
          {value ? new Date(value).toLocaleString() : "Select date & time"}
        </span>
        <FontAwesomeIcon icon={"clock"} className="text-gray-400" />
      </button>

      {open && !disabled && (
        <div
          ref={popupRef}
          style={{ top: position.top, left: position.left }}
          className="fixed z-[9999] w-80 rounded-xl border bg-white p-4 shadow-xl"
        >
          <div className="mb-3 flex items-center justify-between">
            {view === "month" && (
              <>
                <button
                  onClick={() => setCurrent(new Date(year, month - 1, 1))}
                  className="rounded px-2 py-1 text-gray-500 hover:bg-gray-100"
                >
                  ‹
                </button>

                <button
                  onClick={() => setView("year")}
                  className="text-sm font-semibold hover:underline"
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
                  ›
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
                  const d = i + 1;
                  const isSelected = d === day;

                  return (
                    <button
                      key={d}
                      onClick={() => {
                        const next = new Date(current);
                        next.setDate(d);
                        setCurrent(next);
                        onChange(next.toUTCString());
                      }}
                      className={`rounded-md py-1 text-sm
                        ${
                          isSelected
                            ? "bg-blue-600 text-white"
                            : "hover:bg-blue-50"
                        }`}
                    >
                      {d}
                    </button>
                  );
                })}
              </div>

              <div className="mt-4 grid grid-cols-3 gap-2">
                {[
                  { label: "Hours", value: hour, max: 24 },
                  { label: "Minutes", value: minute, max: 60 },
                  { label: "Seconds", value: second, max: 60 },
                ].map(({ label, value, max }) => (
                  <div key={label} className="flex flex-col">
                    <span className="text-center text-xs italic text-gray-400">
                      {label}
                    </span>
                    <select
                      value={value}
                      onChange={(e) => {
                        const v = Number(e.target.value);
                        if (label === "Hours") updateTime(v, minute, second);
                        if (label === "Minutes") updateTime(hour, v, second);
                        if (label === "Seconds") updateTime(hour, minute, v);
                      }}
                      className="rounded-md border px-2 py-1 text-sm"
                    >
                      {Array.from({ length: max }).map((_, i) => (
                        <option key={i} value={i}>
                          {String(i).padStart(2, "0")}
                        </option>
                      ))}
                    </select>
                  </div>
                ))}
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
