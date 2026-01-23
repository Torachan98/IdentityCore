import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useMemo } from "react";

type PaginationProps = {
  pageNum: number;
  totalPages: number;
  onChange: (page: number) => void;
};

export default function Pagination({
  pageNum,
  totalPages,
  onChange,
}: PaginationProps) {
  function getPaginationRange(
    current: number,
    total: number,
    delta = 1,
  ): (number | "...")[] {
    const range: (number | "...")[] = [];
    const left = Math.max(2, current - delta);
    const right = Math.min(total - 1, current + delta);

    range.push(1);

    if (left > 2) range.push("...");

    for (let i = left; i <= right; i++) {
      range.push(i);
    }

    if (right < total - 1) range.push("...");

    if (total > 1) range.push(total);

    return range;
  }

  const pages = useMemo(() => {
    return getPaginationRange(pageNum, totalPages);
  }, [pageNum, totalPages]);

  return (
    <div className="pt-2 flex items-center gap-2">
      <button
        disabled={pageNum === 1}
        onClick={() => onChange(pageNum - 1)}
        className="rounded-lg border px-3 py-1 text-sm disabled:opacity-40"
      >
        <FontAwesomeIcon icon={"angles-left"} />
      </button>

      {pages.map((p, i) => (
        <button
          key={i}
          onClick={() => {
            if (p !== "...") onChange(p);
          }}
          className={`${p !== "..." ? "rounded-lg border" : ""} px-3 py-1 text-sm 
              ${
                p === pageNum
                  ? "bg-blue-600 text-white border-blue-600"
                  : p !== "..."
                    ? '"hover:bg-gray-100"'
                    : ""
              }
            `}
          disabled={p === "..."}
        >
          {p}
        </button>
      ))}

      <button
        disabled={pageNum >= totalPages}
        onClick={() => onChange(pageNum + 1)}
        className="rounded-lg border px-3 py-1 text-sm disabled:opacity-40"
      >
        <FontAwesomeIcon icon={"angles-right"} />
      </button>
    </div>
  );
}
