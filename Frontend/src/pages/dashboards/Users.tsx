import { useEffect, useState } from "react";
import Modal from "@/components/molecules/Modal";
import Button from "@/components/atoms/Button";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { deleteUser, fetchUsers, updateUser } from "@/store/user/user.thunk";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import DataTable from "@/components/organisms/Table/DataTable";
import { useNavigate } from "react-router-dom";
import { PAGE_TYPE, STATUS_ACTION_TYPE } from "@/store/base.types";
import { setUser } from "@/store/user/user.slice";
import { Step } from "@/store/user/user.types";
import { fetchRoles } from "@/store/role/role.thunk";
import { fetchPermissions } from "@/store/permission/permission.thunk";
import { fetchServices } from "@/store/service/service.thunk";
import InitialUser from "./initials/InitialUser";
import { UserDTO } from "@/api/generated";

export default function Users() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { users, pageNum, pageSize, totalPages, user } = useAppSelector(
    (state) => state.user,
  );
  const {
    services,
    pageSize: pageSizeService,
    pageNum: pageNumService,
    totalItems: totalItemsService,
  } = useAppSelector((state) => state.service);
  const { permissions } = useAppSelector((state) => state.permission);
  const { roles } = useAppSelector((state) => state.role);

  const [open, setOpen] = useState(false);
  const [pageNumSelected, setPageNumSelected] = useState(1);
  const [pageSizeSelected, setPageSizeSelected] = useState(10);

  const [pageNumbService, setPageNumbService] = useState(1);

  const [isShow, setIsShow] = useState(false);

  useEffect(() => {
    dispatch(
      fetchUsers({
        pageNum: pageNumSelected.toString(),
        pageSize: pageSizeSelected.toString(),
      }),
    );

    dispatch(fetchRoles({ pageSize: PAGE_TYPE.MAX }));
    dispatch(fetchPermissions({ pageSize: PAGE_TYPE.MAX }));
    //
  }, [pageNumSelected, pageSizeSelected]);

  useEffect(() => {
    dispatch(setUser({}));
  }, [open]);

  useEffect(() => {
    dispatch(
      fetchServices({ pageSize: "5", pageNum: pageNumbService.toString() }),
    );
  }, [pageNumbService]);

  return (
    <div className="p-6">
      <div className="flex justify-between mb-4">
        <h1 className="text-xl font-bold">Users</h1>
        <Button className="w-[5%]" onClick={() => setOpen(true)}>
          <FontAwesomeIcon icon={"add"} />
        </Button>
      </div>

      <DataTable
        data={users ?? []}
        rowKey="guid"
        columns={[
          { key: "fullName", header: "Name", width: "15%" },
          {
            key: "avatarUrl",
            header: "Avatar",
            align: "center",
            width: "10%",
            render: (u) => (
              <img
                className="
                    w-10 h-10
                    sm:w-12 sm:h-12
                    md:w-16 md:h-16
                    lg:w-20 lg:h-20
                    rounded-full
                    object-cover
                    border
                  "
                src={
                  u?.avatarUrl
                    ? `https://drive.google.com/thumbnail?id=${u.avatarUrl}&sz=w500`
                    : "/assets/default_logo.jpg"
                }
                alt="User avatar"
              />
            ),
          },
          { key: "email", header: "Email", width: "15%" },
          {
            key: "userName",
            header: "User Name",
            align: "center",
            width: "10%",
          },
          { key: "phone", header: "Phone", align: "center", width: "10%" },
          {
            key: "step",
            header: "Status",
            align: "center",
            render: (u) => (
              <>
                {u.step === Step.Confirmed && (
                  <span className="rounded-full bg-green-600 px-2 py-1 text-xs text-white font-semibold">
                    Active
                  </span>
                )}

                {u.step === Step.Waiting && (
                  <span className="rounded-full bg-red-600 px-2 py-1 text-xs text-white font-semibold">
                    Waiting
                  </span>
                )}
              </>
            ),
          },
          {
            key: "dateCreated",
            header: "Created At",
            align: "left",
            render: (u) => new Date(u.dateCreated ?? "").toLocaleDateString(),
          },
          { key: "region", header: "Region", align: "center" },
        ]}
        actions={(u) => (
          <div
            className="
                      grid
                      grid-cols-2
                      gap-2
                      sm:grid-cols-3
                      md:grid-cols-4
                      lg:flex
                      lg:justify-center
                      lg:gap-3
                    "
          >
            <Button
              className="w-full bg-green-600 hover:bg-green-500"
              title="Edit"
              onClick={() =>
                navigate(
                  `/user-detail/${u.guid}?type=${STATUS_ACTION_TYPE.EDIT}`,
                  { replace: true },
                )
              }
            >
              <FontAwesomeIcon icon="edit" />
            </Button>

            <Button
              className="w-full bg-blue-600 hover:bg-blue-500"
              title="View"
              onClick={() =>
                navigate(
                  `/user-detail/${u.guid}?type=${STATUS_ACTION_TYPE.VIEW}`,
                )
              }
            >
              <FontAwesomeIcon icon="eye" />
            </Button>

            <Button
              className="w-full bg-red-600 hover:bg-red-500"
              title="Delete"
              onClick={async () => {
                await dispatch(deleteUser(u.guid ?? ""));
                await dispatch(
                  fetchUsers({
                    pageNum: pageNumSelected.toString(),
                    pageSize: pageSizeSelected.toString(),
                  }),
                );
              }}
            >
              <FontAwesomeIcon icon="trash" />
            </Button>

            {u.locked !== null ? (
              <Button
                className="w-full bg-yellow-600 hover:bg-yellow-500"
                title="Unlock"
                onClick={async () => {
                  await dispatch(updateUser({ ...u, locked: "" }));
                  await dispatch(
                    fetchUsers({
                      pageNum: pageNumSelected.toString(),
                      pageSize: pageSizeSelected.toString(),
                    }),
                  );
                }}
              >
                <FontAwesomeIcon icon="lock-open" />
              </Button>
            ) : (
              <Button
                className="w-full bg-yellow-600 hover:bg-yellow-500"
                title="Lock"
                onClick={async () => {
                  await dispatch(
                    updateUser({
                      ...u,
                      locked: new Date(
                        new Date().setDate(new Date().getDate() + 3),
                      ).toUTCString(),
                      isLock: true,
                    }),
                  );
                  await dispatch(
                    fetchUsers({
                      pageNum: pageNumSelected.toString(),
                      pageSize: pageSizeSelected.toString(),
                    }),
                  );
                }}
              >
                <FontAwesomeIcon icon="lock" />
              </Button>
            )}
          </div>
        )}
        totalPages={totalPages}
        pageNum={pageNum}
        pageSize={pageSize}
        onChangePageNum={(p) => setPageNumSelected(p)}
        onChangePageSize={(p) => setPageSizeSelected(p)}
      />

      <Modal
        open={open}
        onClose={() => setOpen(false)}
        closeOnOverlayClick={false}
        childrenTitle={
          <div className="flex items-start justify-between border-b px-6 py-4">
            <div>
              <h2 className="text-lg font-semibold text-gray-800">
                Create New User
              </h2>
              <p className="text-sm text-gray-500">
                Fill in the information below to create a user account
              </p>
            </div>
            <button
              onClick={() => setOpen(false)}
              className="text-gray-400 hover:text-gray-600"
            >
              <FontAwesomeIcon icon={"x"} />
            </button>
          </div>
        }
        childrenBody={
          <InitialUser
            user={{
              user: user !== null ? user : {},
              pageNum: 0,
              pageSize: 0,
              totalItems: 0,
            }}
            roles={roles !== null ? roles : []}
            service={{
              pageNum: pageNumService,
              pageSize: pageSizeService,
              totalItems: totalItemsService,
              services: services !== null ? services : [],
            }}
            permissions={permissions !== null ? permissions : []}
            onCancel={(res) => setOpen(res)}
            onChangePage={(page) => setPageNumbService(page)}
          />
        }
      />
    </div>
  );
}
