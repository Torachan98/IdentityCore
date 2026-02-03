import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Button from "@/components/atoms/Button";
import { createUser, fetchUsers } from "@/store/user/user.thunk";
import DatePicker from "@/components/atoms/DatePicker";
import { setUser } from "@/store/user/user.slice";
import ServiceSelector, {
  SelectedService,
} from "@/components/atoms/ServiceSelector";
import { MultiSelect } from "@/components/atoms/MultipleSelect";
import {
  PermissionDTO,
  RoleDTO,
  RoleResponse,
  ServiceDTO,
  UserDTO,
} from "@/api/generated";
import DateTimePicker from "@/components/atoms/DateTimePicker";
import Switch from "react-switch";
import { InputLabel } from "@/components/atoms/InputLabel";
import Select from "@/components/atoms/Select";
import { useAppDispatch } from "@/store/hooks";
import { useState } from "react";

type Props = {
  user: UserProps;
  roles: RoleDTO[];
  service: ServiceProps;
  permissions: PermissionDTO[];

  onCancel: (value: boolean) => void;
  onChangePage: (number: number) => void;
};

type ServiceProps = {
  pageNum: number;
  pageSize: number;
  totalItems: number;
  services: ServiceDTO[];
};

type UserProps = {
  user: UserDTO;
  pageNum: number;
  pageSize: number;
  totalItems: number;
};

export default function InitialUser({
  user,
  roles,
  service,
  permissions,
  onCancel,
  onChangePage,
}: Props) {
  const dispatch = useAppDispatch();

  const [isHint, setHint] = useState(false);

  return (
    <div className="w-full max-w-3xl rounded-2xl bg-white shadow-xl">
      <div className="px-6 py-6 space-y-6">
        <div className="flex items-center gap-6">
          <div className="relative">
            <img
              src="/assets/default_logo.jpg"
              className="h-24 w-24 rounded-full border object-cover"
            />
            <label className="absolute bottom-0 right-0 flex h-8 w-8 cursor-pointer items-center justify-center rounded-full bg-blue-600 text-white hover:bg-blue-700">
              <FontAwesomeIcon icon={"pen"} />
              <input type="file" hidden accept="image/*" />
            </label>
          </div>
          <div className="text-sm text-gray-500">
            Upload avatar (JPG, PNG, max 2MB)
          </div>
        </div>

        <div>
          <h3 className="mb-3 text-sm font-semibold text-gray-700 uppercase">
            <FontAwesomeIcon icon={"user"} /> Basic Information
          </h3>

          <div className="w-full flex flex-row gap-3 mb-3">
            <InputLabel
              label="First name"
              placeholder="First name"
              value={user?.user.firstName ?? ""}
              onChange={async (e) =>
                await dispatch(setUser({ ...user, firstName: e.target.value }))
              }
            />

            <InputLabel
              label="Middle name"
              placeholder="Middle name"
              value={user?.user.middleName ?? ""}
              onChange={async (e) =>
                await dispatch(
                  setUser({
                    ...user,
                    middleName: e.target.value,
                  }),
                )
              }
            />

            <InputLabel
              label="Last name"
              placeholder="Last name"
              value={user?.user.lastName ?? ""}
              onChange={async (e) =>
                await dispatch(
                  setUser({
                    ...user,
                    lastName: e.target.value,
                  }),
                )
              }
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <InputLabel
              label="Username"
              placeholder="Username"
              value={user?.user.userName ?? ""}
              onChange={async (e) =>
                await dispatch(
                  setUser({
                    ...user,
                    userName: e.target.value,
                  }),
                )
              }
            />

            <InputLabel
              label="Email"
              placeholder="Email"
              value={user?.user.email ?? ""}
              onChange={async (e) =>
                await dispatch(
                  setUser({
                    ...user,
                    email: e.target.value,
                  }),
                )
              }
            />

            <div className="flex flex-row gap-2">
              <InputLabel
                className="w-[50px]"
                label="Code"
                placeholder="+84"
                value={user?.user.phoneCode ?? ""}
                onChange={async (e) =>
                  await dispatch(
                    setUser({
                      ...user,
                      phoneCode: e.target.value,
                    }),
                  )
                }
              />

              <InputLabel
                className="w-full"
                label="Phone"
                placeholder="Phone"
                value={user?.user.phone ?? ""}
                onChange={async (e) =>
                  await dispatch(
                    setUser({
                      ...user,
                      phone: e.target.value,
                    }),
                  )
                }
              />
            </div>

            <div className="flex flex-col justify-between">
              <label className="text-xs font-medium text-gray-500">
                Country
              </label>
              <Select
                className="text-sm"
                options={[
                  { value: "VN", label: "Viet Nam" },
                  { value: "US", label: "United States" },
                  { value: "JP", label: "Japan" },
                  { value: "PH", label: "Philipines" },
                ]}
                value={user?.user.region ?? "VN"}
                onChange={async (e) => {
                  await dispatch(setUser({ ...user, region: e.target.value }));
                }}
              />
            </div>
          </div>
        </div>

        <div>
          <h3 className="mb-3 text-sm font-semibold text-gray-700 uppercase">
            <FontAwesomeIcon icon={"sliders"} /> Config
          </h3>

          <div>
            <div className="overflow-hidden rounded-lg border bg-white">
              <div className="divide-y">
                <div className="grid grid-cols-3 gap-4 px-4 py-3">
                  <dt className="flex self-center text-sm font-medium text-gray-500">
                    Change Password ?
                  </dt>
                  <dd className="flex items-center col-span-2">
                    <Switch
                      width={48}
                      height={20}
                      onColor="#567ff1"
                      onHandleColor="#1d4ed8"
                      handleDiameter={30}
                      uncheckedIcon={false}
                      checkedIcon={false}
                      boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                      activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                      checked={user?.user.isRequiredChangePassword ?? false}
                      onChange={async (e) => {
                        await dispatch(
                          setUser({
                            ...user,
                            isRequiredChangePassword: e,
                          }),
                        );
                      }}
                    />
                  </dd>
                </div>

                <div className="grid grid-cols-3 gap-4 px-4 py-3">
                  <dt className="flex self-center text-sm font-medium text-gray-500">
                    Password
                  </dt>
                  <dd className="flex items-center col-span-2 gap-2">
                    <InputLabel
                      className="w-full"
                      disabled={user?.user.isRequiredChangePassword}
                      type={isHint ? "text" : "password"}
                      placeholder="Password"
                      value={user?.user.password ?? ""}
                      onChange={async (e) =>
                        await dispatch(
                          setUser({
                            ...user,
                            password: e.target.value,
                          }),
                        )
                      }
                    />

                    <Button
                      className="px-2"
                      disabled={user?.user.isRequiredChangePassword}
                      onClick={() => setHint(!isHint)}
                    >
                      Show
                    </Button>
                  </dd>
                </div>

                <div className="grid grid-cols-3 gap-4 px-4 py-3">
                  <dt className="flex self-center text-sm font-medium text-gray-500">
                    Password Expired ?
                  </dt>
                  <dd className="flex items-center col-span-2">
                    <Switch
                      width={48}
                      height={20}
                      onColor="#567ff1"
                      onHandleColor="#1d4ed8"
                      handleDiameter={30}
                      uncheckedIcon={false}
                      checkedIcon={false}
                      boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                      activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                      checked={user?.user.isPasswordExpired ?? false}
                      onChange={async (e) => {
                        await dispatch(
                          setUser({
                            ...user,
                            isPasswordExpired: e,
                          }),
                        );
                      }}
                    />
                  </dd>
                </div>

                <div className="grid grid-cols-3 gap-4 px-4 py-3">
                  <dt className="flex self-center text-sm font-medium text-gray-500">
                    Date Expired ?
                  </dt>
                  <dd className="flex items-center col-span-2">
                    <DateTimePicker
                      disabled={!user?.user.isPasswordExpired}
                      value={user?.user.expirationDate}
                      onChange={async (date) => {
                        await dispatch(
                          setUser({ ...user, expirationDate: date }),
                        );
                      }}
                    />
                  </dd>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div>
          <h3 className="mb-3 text-sm font-semibold text-gray-700 uppercase">
            <FontAwesomeIcon icon={"gears"} /> Access Control
          </h3>

          <div className="space-y-4">
            <div>
              <label className="text-sm font-medium text-gray-600">Roles</label>
              <MultiSelect
                placeholder="Select roles"
                options={
                  roles !== null
                    ? roles.map((p) => {
                        return {
                          label: p.name ?? "",
                          value: p.guid ?? "",
                        };
                      })
                    : []
                }
                value={
                  user?.user.roles !== undefined
                    ? user?.user.roles?.map((s) => {
                        return s.guid ?? "";
                      })
                    : []
                }
                onChange={async (role) => {
                  if (roles != null) {
                    const roleItems = role.map((item) => {
                      const roleItem = roles.find((s) => s.guid === item);
                      return {
                        guid: roleItem?.guid ?? "",
                        role: roleItem?.name ?? "",
                        value: roleItem?.role ?? "",
                        permissions: [],
                      } as RoleResponse;
                    });

                    await dispatch(setUser({ ...user, roles: roleItems }));
                  }
                }}
              />
            </div>

            <div>
              <label className="text-sm font-medium text-gray-600">
                Permissions
              </label>
              <MultiSelect
                placeholder="Select permissions"
                options={
                  permissions !== null
                    ? permissions.map((p) => {
                        return {
                          label: p.name ?? "",
                          value: p.guid ?? "",
                        };
                      })
                    : []
                }
                value={user?.user.permissions ?? []}
                onChange={async (permissions) => {
                  await dispatch(
                    setUser({ ...user, permissions: permissions }),
                  );
                }}
              />
            </div>

            <div>
              <label className="text-sm font-medium text-gray-600">
                Services
              </label>
              <ServiceSelector
                services={
                  service.services !== null
                    ? service.services?.map((s) => {
                        return {
                          id: s.guid ?? "",
                          name: s.name ?? "",
                        };
                      })
                    : []
                }
                pageNum={service.pageNum}
                pageSize={service.pageSize}
                total={service.totalItems}
                selected={
                  user?.user.services
                    ? user?.user.services.map((s) => {
                        return {
                          id: s.guid,
                          name: s.name,
                          activeDate: s.dateActive,
                          expiredDate: s.dateExpired,
                        } as SelectedService;
                      })
                    : []
                }
                onSelect={async (service) => {
                  var assignService = user?.user.services?.find(
                    (s) => s.guid === service.id,
                  );

                  if (assignService !== undefined) return;

                  await dispatch(
                    setUser({
                      ...user,
                      services: [
                        ...(user?.user.services ?? []),
                        { guid: service.id, name: service.name },
                      ],
                    }),
                  );
                }}
                onRemove={async (id) =>
                  await dispatch(
                    setUser({
                      ...user,
                      services: user?.user.services?.filter(
                        (s) => s.guid != id,
                      ),
                    }),
                  )
                }
                onSearch={(keyword) => {
                  // setKeyword(keyword);
                  // setPage(1);
                }}
                onPageChange={(page) => onChangePage(page)}
                onUpdateDate={async (id, field, value) => {
                  const serviceUpdated = user?.user.services?.map((s) =>
                    s.guid === id ? { ...s, [field]: value } : s,
                  );

                  await dispatch(
                    setUser({ ...user, services: serviceUpdated }),
                  );
                }}
                renderDatePicker={(value, onChange) => (
                  <DatePicker value={value} onChange={(v) => onChange?.(v)} />
                )}
              />
            </div>
          </div>
        </div>
      </div>

      <div className="flex justify-end gap-3 border-t px-6 py-4 bg-gray-50">
        <Button
          onClick={() => onCancel(false)}
          className="px-4 bg-gray-400 hover:bg-gray-200"
        >
          <FontAwesomeIcon icon={"x"} />
        </Button>
        <Button
          className="px-4 bg-blue-600 text-white hover:bg-blue-700"
          onClick={async () => {
            await dispatch(
              createUser({
                ...user.user,
                password: !user?.user.isPasswordExpired
                  ? "123456"
                  : user.user.password,
                locked: "",
                region: !user?.user.region ? "VN" : user?.user.region,
                attemptLogin: 0,
                permissions: [],
                services: [],
                roles: [],
              }),
            );

            await dispatch(
              fetchUsers({
                pageNum: user.pageNum.toString(),
                pageSize: user.pageSize.toString(),
              }),
            );
          }}
        >
          <FontAwesomeIcon icon={"user-plus"} />
        </Button>
      </div>
    </div>
  );
}
