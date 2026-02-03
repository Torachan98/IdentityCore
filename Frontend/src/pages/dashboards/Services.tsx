import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { fetchServices } from "@/store/service/service.thunk";
import DataTable from "@/components/organisms/Table/DataTable";
import Button from "@/components/atoms/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function Services() {
  const dispatch = useAppDispatch();
  const [pageNumSelected, setPageNumSelected] = useState(1);
  const [pageSizeSelected, setPageSizeSelected] = useState(10);
  const { services, pageNum, pageSize, totalPages } = useAppSelector(
    (state) => state.service,
  );

  useEffect(() => {
    dispatch(
      fetchServices({
        pageNum: pageNumSelected.toString(),
        pageSize: pageSizeSelected.toString(),
      }),
    );
  }, [pageNumSelected, pageSizeSelected]);

  return (
    <div className="p-6">
      <div className="flex justify-between mb-4">
        <h1 className="text-xl font-bold">Services</h1>
      </div>

      <DataTable
        data={services ?? []}
        rowKey="guid"
        columns={[
          { key: "name", header: "Name", align: "center" },
          {
            key: "signatureKey",
            header: "Key",
            align: "center",
          },
        ]}
        actions={(u) => (
          <div
            className="grid
                      grid-cols-2
                      gap-2
                      sm:grid-cols-3
                      md:grid-cols-4
                      lg:flex
                      lg:justify-center
                      lg:gap-2"
          >
            <Button
              className="w-full bg-blue-600 hover:bg-blue-400"
              onClick={() => console.log(u.guid ?? "")}
            >
              <FontAwesomeIcon icon={"eye"} />
            </Button>
            <Button
              className="w-full bg-yellow-600 hover:bg-yellow-400"
              onClick={() => console.log(u)}
            >
              <FontAwesomeIcon icon={"edit"} />
            </Button>

            <Button
              className="w-full bg-red-600 hover:bg-red-400"
              onClick={() => console.log(u.guid ?? "")}
            >
              <FontAwesomeIcon icon={"trash"} />
            </Button>
          </div>
        )}
        totalPages={totalPages}
        pageNum={pageNum}
        pageSize={pageSize}
        onChangePageNum={(p) => setPageNumSelected(p)}
        onChangePageSize={(p) => setPageSizeSelected(p)}
      />
    </div>
  );
}
