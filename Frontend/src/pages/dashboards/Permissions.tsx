import { useEffect, useState } from "react";
import Select from "@/components/atoms/Select";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { fetchPermissions } from "@/store/permission/permission.thunk";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Button from "@/components/atoms/Button";
import DataTable from "@/components/organisms/Table/DataTable";

export default function Permissions() {
  const dispatch = useAppDispatch();
  const [pageNumSelected, setPageNumSelected] = useState(1);
  const [pageSizeSelected, setPageSizeSelected] = useState(10);
  const { permissions, pageNum, pageSize, totalPages } = useAppSelector(
    (state) => state.permission,
  );

  useEffect(() => {
    dispatch(
      fetchPermissions({
        pageNum: pageNumSelected.toString(),
        pageSize: pageSizeSelected.toString(),
      }),
    );
  }, [pageNumSelected, pageSizeSelected]);

  return (
    <div className="p-6">
      <div className="flex justify-between mb-4">
        <h1 className="text-xl font-bold">Permissions</h1>
        <Button className="w-[5%]" onClick={() => console.log("click")}>
          <FontAwesomeIcon icon={"add"} />
        </Button>
      </div>

      <DataTable
        data={permissions ?? []}
        rowKey="guid"
        columns={[
          { key: "name", header: "Name", align: "center", width: "45%" },
          { key: "description", header: "Description", width: "45%" },
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
                      lg:gap-3"
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
