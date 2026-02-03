import { useEffect } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { fetchRoleById, updateRole } from "@/store/role/role.thunk";
import { InputLabel } from "@/components/atoms/InputLabel";
import { ParagraphLabel } from "@/components/atoms/ParagraphLabel";
import { fetchPermissions } from "@/store/permission/permission.thunk";
import { MultiSelect } from "@/components/atoms/MultipleSelect";
import { setRole } from "@/store/role/role.slice";
import { PermissionDTO } from "@/api/generated";
import { PAGE_TYPE, STATUS_ACTION_TYPE } from "@/store/base.types";
import Button from "@/components/atoms/Button";

type WithGuid = {
  guid?: string;
};

export default function RoleDetail() {
  const { id } = useParams();
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const { role, error } = useAppSelector((state) => state.role);
  const { permissions } = useAppSelector((state) => state.permission);

  useEffect(() => {
    dispatch(fetchRoleById(id ?? ""));
    dispatch(fetchPermissions({ pageSize: PAGE_TYPE.MAX }));

    if (error != "Successfully") {
      return navigate("/404", { replace: true });
    }
  }, [id]);

  const fetchListNonUndefined = <T extends WithGuid>(
    array: T[] | undefined,
  ) => {
    if (array !== undefined) {
      return array?.reduce<string[]>((per, item) => {
        if (item.guid) per.push(item.guid);
        return per;
      }, []);
    }
    return [];
  };

  return (
    <div className="max-w-3xl mx-auto">
      <div className="flex items-center gap-4 mb-6">
        <Button className="px-2" onClick={() => navigate("/roles")}>
          Back
        </Button>

        <div>
          <h2 className="text-lg font-semibold text-gray-800">
            Role Information
          </h2>
          <p className="text-sm text-gray-500">
            Define role details and assign permissions
          </p>
        </div>
      </div>
      <div className="rounded-xl border border-gray-200 bg-white shadow-sm">
        <div className="flex flex-col gap-6 px-6 py-6">
          <div className="flex flex-col gap-1">
            <InputLabel
              label="Role name"
              value={role?.name ?? ""}
              onChange={async (e) =>
                await dispatch(setRole({ ...role, name: e.target.value }))
              }
              disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
            />
          </div>

          <div className="flex flex-col gap-1">
            <ParagraphLabel
              label="Description"
              value={role?.description ?? ""}
              onChange={async (e) =>
                await dispatch(
                  setRole({ ...role, description: e.target.value }),
                )
              }
              disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
            />
          </div>

          <div className="flex flex-col gap-2">
            <label className="text-sm font-medium text-gray-700">
              Permissions
            </label>
            <p className="text-xs text-gray-500">
              Select permissions assigned to this role
            </p>

            <MultiSelect
              disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
              options={
                permissions?.map((p) => ({
                  label: p.name ?? "",
                  value: p.guid ?? "",
                })) ?? []
              }
              value={fetchListNonUndefined<PermissionDTO>(role?.permissions)}
              onChange={async (ids) => {
                if (!permissions) return;

                const permissionItems = permissions.filter((p) =>
                  ids.includes(p.guid!),
                );

                await dispatch(
                  setRole({ ...role, permissions: permissionItems }),
                );
              }}
            />
          </div>

          {searchParams.get("type") !== STATUS_ACTION_TYPE.VIEW && (
            <div className="flex justify-end gap-3 mt-8">
              <Button
                onClick={() => navigate("/roles", { replace: true })}
                className="px-2"
              >
                Cancel
              </Button>
              <Button
                onClick={async () => {
                  await dispatch(
                    updateRole({
                      guid: role?.guid ?? "",
                      name: role?.name ?? "",
                      description: role?.description ?? "",
                      isLock: false,
                      permissions: fetchListNonUndefined<PermissionDTO>(
                        role?.permissions,
                      ),
                    }),
                  );
                }}
                className="px-2"
              >
                Update
              </Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
