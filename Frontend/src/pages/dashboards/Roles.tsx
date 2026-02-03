import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import Button from "@/components/atoms/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import DataTable from "@/components/organisms/Table/DataTable";
import { fetchRoles } from "@/store/role/role.thunk";
import { useNavigate } from "react-router-dom";
import { STATUS_ACTION_TYPE } from "@/store/base.types";

export default function Roles() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [pageNumSelected, setPageNumSelected] = useState(1);
  const [pageSizeSelected, setPageSizeSelected] = useState(10);
  const { roles, pageNum, pageSize, totalPages } = useAppSelector(
    (state) => state.role,
  );

  useEffect(() => {
    dispatch(
      fetchRoles({
        pageNum: pageNumSelected.toString(),
        pageSize: pageSizeSelected.toString(),
      }),
    );
  }, [pageNumSelected, pageSizeSelected]);

  return (
    <div className="p-6">
      <div className="flex justify-between mb-4">
        <h1 className="text-xl font-bold">Roles</h1>
        <Button className="w-[5%]" onClick={() => console.log("click")}>
          <FontAwesomeIcon icon={"add"} />
        </Button>
      </div>

      <DataTable
        data={roles ?? []}
        rowKey="guid"
        columns={[
          { key: "name", header: "Name", align: "center", width: "40%" },
          { key: "description", header: "Description", width: "30%" },
          {
            key: "permissions",
            header: "Permissions Availble",
            width: "15%",
            render: (r) => (
              <span className="bg-green-300 px-3 py-2 rounded-full">
                {r.permissions?.length}
              </span>
            ),
          },
          {
            key: "isLock",
            header: "Status",
            render: (r) => (
              <>
                {r.isLock ? (
                  <span className="bg-red-300 px-3 py-2 rounded-full">
                    Locked
                  </span>
                ) : (
                  <span className="bg-green-300 px-3 py-2 rounded-full">
                    Ready
                  </span>
                )}
              </>
            ),
          },
        ]}
        actions={(r) => (
          <div
            className="
                    grid
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
              onClick={() =>
                navigate(
                  `/role-detail/${r.guid}?type=${STATUS_ACTION_TYPE.VIEW}`,
                  { replace: true },
                )
              }
            >
              <FontAwesomeIcon icon={"eye"} />
            </Button>
            <Button
              className="w-full bg-yellow-600 hover:bg-yellow-400"
              onClick={() =>
                navigate(
                  `/role-detail/${r.guid}?type=${STATUS_ACTION_TYPE.EDIT}`,
                  { replace: true },
                )
              }
            >
              <FontAwesomeIcon icon={"edit"} />
            </Button>

            <Button
              className="w-full bg-red-600 hover:bg-red-400"
              onClick={() => console.log(r.guid ?? "")}
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
