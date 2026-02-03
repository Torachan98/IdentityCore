import React from "react";
import Select from "@/components/atoms/Select";
import Pagination from "./Pagination";

export type Column<T> = {
  key: keyof T | string;
  header: string;
  width?: string;
  align?: "left" | "center" | "right";
  render?: (row: T) => React.ReactNode;
};

type DataTableProps<T extends Record<string, any>> = {
  data: T[];
  columns: Column<T>[];
  rowKey?: keyof T;
  actions?: (row: T) => React.ReactNode;

  pageNum: number;
  pageSize: number;
  totalPages: number;

  onChangePageNum: (pageNum: number) => void;
  onChangePageSize: (pageSize: number) => void;
};

function MobileRow<T extends Record<string, any>>({
  row,
  columns,
  index,
  actions,
}: {
  row: T;
  columns: Column<T>[];
  index: number;
  actions?: (row: T) => React.ReactNode;
}) {
  return (
    <div className="space-y-3 rounded-lg border bg-white p-4 shadow-sm">
      <div className="text-xs text-gray-500">#{index + 1}</div>

      {columns.map((col) => (
        <div
          key={col.key.toString()}
          className="flex items-start justify-between gap-4"
        >
          <span className="text-sm font-medium text-gray-500">
            {col.header}
          </span>

          <span className="text-sm text-gray-800 text-right">
            {col.render ? col.render(row) : row[col.key as keyof T]}
          </span>
        </div>
      ))}

      {actions && <div className="pt-2">{actions(row)}</div>}
    </div>
  );
}

export default function DataTable<T extends Record<string, any>>({
  data,
  columns,
  rowKey,
  actions,
  pageNum,
  pageSize,
  totalPages,
  onChangePageNum,
  onChangePageSize,
}: DataTableProps<T>) {
  return (
    <div className="space-y-3">
      <div className="hidden md:block overflow-x-auto rounded-lg border bg-white shadow-sm">
        <table className="w-full border-collapse table-fixed">
          <thead className="sticky top-0 bg-gray-50 text-sm font-semibold text-gray-600">
            <tr>
              <th className="w-[60px] px-4 py-3 text-center">#</th>

              {columns.map((col) => (
                <th
                  key={col.key.toString()}
                  className={`px-4 py-3 ${
                    col.align === "center"
                      ? "text-center"
                      : col.align === "right"
                        ? "text-right"
                        : "text-left"
                  }`}
                  style={{ width: col.width }}
                >
                  {col.header}
                </th>
              ))}

              {actions && (
                <th className={`px-4 py-3 text-center w-[15%]`}>Actions</th>
              )}
            </tr>
          </thead>

          <tbody className="text-sm text-gray-700">
            {data.length === 0 && (
              <tr>
                <td
                  colSpan={columns.length + (actions ? 2 : 1)}
                  className="py-10 text-center text-gray-400"
                >
                  No data available
                </td>
              </tr>
            )}

            {data.map((row, index) => (
              <tr
                key={rowKey ? String(row[rowKey]) : index}
                className="border-t transition hover:bg-gray-50"
              >
                <td className="px-4 py-3 text-center">{index + 1}</td>

                {columns.map((col) => (
                  <td
                    key={col.key.toString()}
                    className={`px-4 py-3 truncate ${
                      col.align === "center"
                        ? "text-center"
                        : col.align === "right"
                          ? "text-right"
                          : "text-left"
                    }`}
                  >
                    {col.render ? col.render(row) : row[col.key as keyof T]}
                  </td>
                ))}

                {actions && <td className="px-4 py-3">{actions(row)}</td>}
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* =========================
          MOBILE CARDS
      ========================= */}
      <div className="md:hidden space-y-4">
        {data.length === 0 && (
          <div className="py-10 text-center text-gray-400">
            No data available
          </div>
        )}

        {data.map((row, index) => (
          <MobileRow
            key={rowKey ? String(row[rowKey]) : index}
            row={row}
            columns={columns}
            index={index}
            actions={actions}
          />
        ))}
      </div>

      <div className="flex flex-col gap-3 pt-2 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex items-center gap-2">
          <Select
            value={pageSize}
            onChange={(e) => onChangePageSize(Number(e.target.value))}
            options={[
              { value: 10, label: "10 / page" },
              { value: 25, label: "25 / page" },
              { value: 50, label: "50 / page" },
              { value: 100, label: "100 / page" },
            ]}
          />

          <span className="text-sm text-gray-500">
            Page {pageNum} of {totalPages}
          </span>
        </div>

        <Pagination
          pageNum={pageNum}
          totalPages={totalPages}
          onChange={onChangePageNum}
        />
      </div>
    </div>
  );
}
