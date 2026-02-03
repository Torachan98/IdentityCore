import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import Button from "./Button";
import { isInvalidDateRange } from "@/helpers";

export type ServiceOption = {
  id: string;
  name: string;
};

export type SelectedService = {
  id: string;
  name: string;
  activeDate?: string;
  expiredDate?: string;
};

export type ServiceSelectorProps = {
  /** paginated services from store */
  services: ServiceOption[];
  pageNum: number;
  pageSize: number;
  total: number;

  /** selected services */
  selected: SelectedService[];

  onSelect: (service: ServiceOption) => void;
  onRemove: (id: string) => void;

  onPageChange: (page: number) => void;
  onSearch: (keyword: string) => void;

  renderDatePicker: (
    value?: string,
    onChange?: (value: string) => void,
  ) => React.ReactNode;

  onUpdateDate: (
    id: string,
    field: "dateActive" | "dateExpired",
    value: string,
  ) => void;
};

export default function ServiceSelector({
  services,
  pageNum,
  pageSize,
  total,
  selected,
  onSelect,
  onRemove,
  onPageChange,
  onSearch,
  renderDatePicker,
  onUpdateDate,
}: ServiceSelectorProps) {
  const [keyword, setKeyword] = useState("");

  const totalPages = Math.ceil(total / pageSize);

  return (
    <div className="space-y-6">
      <div className="rounded-lg border bg-white p-4">
        <div className="flex flex-row justify-between items-center gap-3 mb-3">
          <input
            value={keyword}
            onChange={(e) => {
              setKeyword(e.target.value);
            }}
            placeholder="Search services..."
            className="w-full rounded border px-3 py-2 text-sm"
          />

          <Button className="px-3" onClick={() => onSearch(keyword)}>
            <FontAwesomeIcon icon={"search"} />
          </Button>
        </div>

        <div className="max-h-64 space-y-2 overflow-auto">
          {services.map((s) => {
            const exists = selected.some((x) => x.id === s.id);

            return (
              <div
                key={s.id}
                className="flex items-center justify-between rounded border p-2"
              >
                <span>{s.name}</span>

                {exists ? (
                  <button
                    className="text-sm text-red-600"
                    onClick={() => onRemove(s.id)}
                  >
                    Remove
                  </button>
                ) : (
                  <button
                    className="text-sm text-blue-600"
                    onClick={() => onSelect(s)}
                  >
                    Add
                  </button>
                )}
              </div>
            );
          })}
        </div>

        <div className="mt-3 flex items-center justify-end gap-3 text-sm">
          <Button
            className="px-2"
            disabled={pageNum === 1}
            onClick={() => onPageChange(pageNum - 1)}
          >
            <FontAwesomeIcon icon={"arrow-left"} />
          </Button>

          <span>
            {pageNum} / {totalPages}
          </span>

          <Button
            className="px-2"
            disabled={pageNum >= totalPages}
            onClick={() => onPageChange(pageNum + 1)}
          >
            <FontAwesomeIcon icon={"arrow-right"} />
          </Button>
        </div>

        {selected.length > 0 && (
          <div className="space-y-3 py-4">
            {selected.map((s) => {
              const hasDateError = isInvalidDateRange(
                s.activeDate,
                s.expiredDate,
              );

              return (
                <div key={s.id} className="rounded-lg border bg-white p-4">
                  <div className="mb-2 flex items-center justify-between">
                    <strong>{s.name}</strong>
                    <button
                      className="text-sm text-red-600"
                      onClick={() => onRemove(s.id)}
                    >
                      Remove
                    </button>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-600">
                        Date active
                      </label>
                      {renderDatePicker(s.activeDate, (v) =>
                        onUpdateDate(s.id, "dateActive", v),
                      )}
                    </div>

                    <div>
                      <label className="block text-sm font-medium text-gray-600">
                        Date expired
                      </label>
                      {renderDatePicker(s.expiredDate, (v) =>
                        onUpdateDate(s.id, "dateExpired", v),
                      )}
                    </div>
                  </div>

                  {hasDateError && (
                    <p className="mt-2 text-sm text-red-600">
                      Date active must be earlier than date expired
                    </p>
                  )}
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
}
