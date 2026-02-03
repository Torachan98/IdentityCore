import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { useEffect, useState } from "react";
import { fetchUserById, updateUser } from "@/store/user/user.thunk";
import Button from "@/components/atoms/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  AssigningService,
  FileService,
  RoleResponse,
  UserDTO,
} from "@/api/generated";
import { setUser } from "@/store/user/user.slice";
import Switch from "react-switch";
import { Step } from "@/store/user/user.types";
import { InputLabel } from "@/components/atoms/InputLabel";
import { MultiSelect } from "@/components/atoms/MultipleSelect";
import { fetchRoles } from "@/store/role/role.thunk";
import { fetchPermissions } from "@/store/permission/permission.thunk";
import { PAGE_TYPE, STATUS_ACTION_TYPE } from "@/store/base.types";
import DateTimePicker from "@/components/atoms/DateTimePicker";
import { fetchServices } from "@/store/service/service.thunk";
import DatePicker from "@/components/atoms/DatePicker";
import ServiceMultiSelect, {
  SelectedService,
  ServiceOption,
} from "@/components/atoms/ServiceSelector";
import ServiceSelector from "@/components/atoms/ServiceSelector";
import { resentOtp } from "@/store/auth/auth.thunk";

export default function UserDetail() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { id } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();
  const { user, error } = useAppSelector((state) => state.user);
  const { roles } = useAppSelector((state) => state.role);
  const { permissions } = useAppSelector((state) => state.permission);
  const { services, pageNum, pageSize, totalItems } = useAppSelector(
    (state) => state.service,
  );

  const [file, setFile] = useState<FileList | null>(null);
  const [minutes, setMinutes] = useState(0);

  const [selected, setSelected] = useState<SelectedService[]>([]);

  const [page, setPage] = useState(1);
  const [keyword, setKeyword] = useState("");

  useEffect(() => {
    dispatch(fetchUserById(id ?? ""));
    dispatch(fetchRoles({ pageSize: PAGE_TYPE.MAX }));
    dispatch(fetchPermissions({ pageSize: PAGE_TYPE.MAX }));

    if (error != "Successfully") {
      return navigate("/404", { replace: true });
    }
  }, [id]);

  useEffect(() => {
    const id = setInterval(() => {
      setMinutes((prev) => {
        if (prev <= 1) {
          clearInterval(id);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(id);
  }, [minutes]);

  useEffect(() => {
    dispatch(
      fetchServices({
        pageNum: page.toString(),
        pageSize: "3",
        keyword: keyword,
      }),
    );
  }, [page, keyword]);

  const statusStep = (status: boolean) => {
    return (
      <>
        {!status && (
          <span className="rounded-full bg-green-100 px-2 py-1 text-xs text-green-700">
            Active
          </span>
        )}

        {status && (
          <span className="rounded-full bg-yellow-100 px-2 py-1 text-xs text-yellow-700">
            Waiting
          </span>
        )}
      </>
    );
  };

  const isLogin = (status: boolean) => {
    return (
      <>
        {status && (
          <span className="rounded-full bg-green-100 px-2 py-1 text-xs text-green-700">
            Login
          </span>
        )}

        {!status && (
          <span className="rounded-full bg-yellow-100 px-2 py-1 text-xs text-yellow-700">
            Logout
          </span>
        )}
      </>
    );
  };

  const addService = async (service: ServiceOption): Promise<void> => {
    var assignService = user?.services?.find((s) => s.guid === service.id);

    if (assignService !== undefined) return;

    await dispatch(
      setUser({
        ...user,
        services: [
          ...(user?.services ?? []),
          { guid: service.id, name: service.name },
        ],
      }),
    );
  };

  const handleSearch = (keyword: string) => {
    setKeyword(keyword);
    setPage(1);
  };

  const handlePageChange = (page: number) => {
    setPage(page);
  };

  const updateDate = async (
    id: string,
    field: "dateActive" | "dateExpired",
    value: string,
  ): Promise<void> => {
    const serviceUpdated = user?.services?.map((s) =>
      s.guid === id ? { ...s, [field]: value } : s,
    );

    await dispatch(setUser({ ...user, services: serviceUpdated }));
  };

  return (
    <div className="p-6 max-w-5xl mx-auto">
      <div className="flex items-center gap-4 mb-6">
        <Button className="px-2" onClick={() => navigate("/users")}>
          Back
        </Button>
        <h1 className="text-xl font-semibold">User Details</h1>
      </div>

      <div className="bg-white rounded-2xl shadow p-6">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div className="flex flex-col items-center gap-4">
            <div className="relative">
              <img
                src={
                  file && file.length > 0
                    ? URL.createObjectURL(file[0])
                    : user?.avatarUrl
                      ? `https://drive.google.com/thumbnail?id=${user.avatarUrl}&sz=w500`
                      : "/assets/default_logo.jpg"
                }
                className="w-32 h-32 rounded-full object-cover border"
              />

              <label className="absolute bottom-0 right-0 flex h-10 w-10 cursor-pointer items-center justify-center rounded-full bg-blue-600 text-white hover:bg-blue-700">
                <FontAwesomeIcon icon={"pen"} />
                {searchParams.get("type") !== STATUS_ACTION_TYPE.VIEW && (
                  <input
                    type="file"
                    hidden
                    accept="image/*"
                    onChange={(e) => {
                      if (e.target.files?.[0]) {
                        setFile(e.target.files);
                      }
                    }}
                  />
                )}
              </label>
            </div>

            <span className="text-sm text-gray-500">JPG, PNG up to 2MB</span>
          </div>

          <div className="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="col-span-2">
              <InputLabel
                className="p-2 bg-[#bbbdc1]-600"
                label="Full name"
                placeholder="Full name"
                disabled={true}
                value={user?.fullName ?? ""}
              />
            </div>

            <div className="col-span-2 flex gap-3 justify-between">
              <InputLabel
                className="p-2 bg-[#bbbdc1]-600"
                label="First Name"
                placeholder="First Name"
                disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
                value={user?.firstName ?? ""}
                onChange={(e) =>
                  dispatch(setUser({ ...user, firstName: e.target.value }))
                }
              />

              <InputLabel
                className="p-2 bg-[#bbbdc1]-600"
                label="Middle Name"
                placeholder="Middle Name"
                disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
                value={user?.middleName ?? ""}
                onChange={(e) =>
                  dispatch(setUser({ ...user, middleName: e.target.value }))
                }
              />

              <InputLabel
                className="p-2 bg-[#bbbdc1]-600"
                label="Last Name"
                placeholder="Last Name"
                disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
                value={user?.lastName ?? ""}
                onChange={(e) =>
                  dispatch(setUser({ ...user, lastName: e.target.value }))
                }
              />
            </div>

            <InputLabel
              className="p-2 bg-[#bbbdc1]-600"
              label="Username"
              placeholder="Username"
              disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
              value={user?.userName ?? ""}
              onChange={(e) =>
                dispatch(setUser({ ...user, userName: e.target.value }))
              }
            />

            <InputLabel
              className="p-2 bg-[#bbbdc1]-600"
              label="Email"
              placeholder="Email"
              disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
              value={user?.email ?? ""}
              onChange={(e) =>
                dispatch(setUser({ ...user, email: e.target.value.trim() }))
              }
            />

            <div className="flex flex-row gap-2">
              <InputLabel
                className="p-2 bg-[#bbbdc1]-600"
                label="Code"
                placeholder="Phone"
                disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
                value={user?.phoneCode ?? ""}
                onChange={(e) =>
                  dispatch(
                    setUser({ ...user, phoneCode: e.target.value.trim() }),
                  )
                }
              />

              <InputLabel
                className="p-2 bg-[#bbbdc1]-600"
                label="Phone"
                placeholder="Phone"
                disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
                value={user?.phone ?? ""}
                onChange={(e) =>
                  dispatch(setUser({ ...user, phone: e.target.value.trim() }))
                }
              />
            </div>

            <InputLabel
              className="p-2 bg-[#bbbdc1]-600"
              label="Country"
              placeholder="Country"
              disabled={searchParams.get("type") === STATUS_ACTION_TYPE.VIEW}
              value={user?.region ?? ""}
              onChange={(e) =>
                dispatch(setUser({ ...user, region: e.target.value.trim() }))
              }
            />

            <div className="col-span-2">
              <label className="text-sm text-gray-400 italic">Info</label>
              <div className="overflow-hidden rounded-lg border bg-white">
                <div className="divide-y">
                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Login status?
                    </dt>
                    <dd className="col-span-2">
                      {isLogin(user?.isLogin ?? false)}
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Latest Login:
                    </dt>
                    <dd className="col-span-2 text-sm text-gray-900">
                      {user?.lastLogin
                        ? new Date(user?.lastLogin).toLocaleString()
                        : "None"}
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Number of retry:
                    </dt>
                    <dd className="col-span-2 text-sm text-gray-900">
                      <div className="flex flex-row justify-between items-center">
                        <div>{user?.attemptLogin}</div>
                        <Button
                          className="px-2 py-2"
                          disabled={
                            searchParams.get("type") === STATUS_ACTION_TYPE.VIEW
                          }
                          onClick={() =>
                            dispatch(setUser({ ...user, attemptLogin: 0 }))
                          }
                        >
                          <FontAwesomeIcon icon={"arrow-rotate-left"} />
                        </Button>
                      </div>
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Status
                    </dt>
                    <dd className="col-span-2">
                      <div className="flex flex-row items-center justify-between">
                        {statusStep(user?.step === Step.Waiting)}

                        {user?.step === Step.Waiting && (
                          <div className="flex gap-2">
                            <Button
                              className="px-2"
                              disabled={minutes > 0}
                              onClick={async () => {
                                await dispatch(resentOtp(user.guid ?? ""));
                                setMinutes(50);
                              }}
                            >
                              {minutes > 0
                                ? `00:${minutes >= 10 ? minutes : "0" + minutes.toString()}`
                                : "Re-Sent Mail"}
                            </Button>
                          </div>
                        )}
                      </div>
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Expiration Date
                    </dt>
                    <dd className="col-span-2">
                      {user?.expirationDate
                        ? new Date(user?.expirationDate ?? "").toLocaleString()
                        : "None"}
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Password Expired?
                    </dt>
                    <dd className="col-span-2">
                      <div className="flex flex-row gap-2 items-center">
                        <Switch
                          disabled={
                            searchParams.get("type") === STATUS_ACTION_TYPE.VIEW
                          }
                          checked={user?.isPasswordExpired ?? false}
                          onChange={(e) =>
                            dispatch(setUser({ ...user, isPasswordExpired: e }))
                          }
                          onColor="#567ff1"
                          onHandleColor="#1d4ed8"
                          handleDiameter={30}
                          uncheckedIcon={false}
                          checkedIcon={false}
                          boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                          activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                          height={20}
                          width={48}
                        />

                        {user?.isPasswordExpired && (
                          <DatePicker
                            value={user?.expirationDate}
                            onChange={(value) =>
                              dispatch(
                                setUser({ ...user, expirationDate: value }),
                              )
                            }
                          />
                        )}
                      </div>
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      <div>Change Password?</div>
                    </dt>
                    <dd className="col-span-2">
                      <Switch
                        disabled={
                          searchParams.get("type") === STATUS_ACTION_TYPE.VIEW
                        }
                        checked={user?.isRequiredChangePassword ?? false}
                        onChange={(e) =>
                          dispatch(
                            setUser({ ...user, isRequiredChangePassword: e }),
                          )
                        }
                        onColor="#567ff1"
                        onHandleColor="#1d4ed8"
                        handleDiameter={30}
                        uncheckedIcon={false}
                        checkedIcon={false}
                        boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                        activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                        height={20}
                        width={48}
                      />
                    </dd>
                  </div>

                  <div className="grid grid-cols-3 gap-4 px-4 py-3">
                    <dt className="flex self-center text-sm font-medium text-gray-500">
                      Lock Until:
                    </dt>
                    <dd className="col-span-2">
                      {user?.locked ? (
                        <DateTimePicker
                          value={user?.locked}
                          onChange={async (value) => {
                            await dispatch(setUser({ ...user, locked: value }));
                          }}
                        />
                      ) : (
                        "None"
                      )}
                    </dd>
                  </div>
                </div>
              </div>
            </div>

            <div className="col-span-2">
              <label className="text-sm text-gray-400 italic ">
                Access Control
              </label>

              <div className="mb-3">
                <label className="text-sm font-medium text-gray-500">
                  Roles
                </label>
                <MultiSelect
                  disabled={
                    searchParams.get("type") === STATUS_ACTION_TYPE.VIEW
                  }
                  options={
                    roles !== null
                      ? roles?.map((s) => {
                          return { label: s.name ?? "", value: s.guid ?? "" };
                        })
                      : []
                  }
                  value={
                    user?.roles !== undefined
                      ? user?.roles?.map((s) => {
                          return s.guid ?? "";
                        })
                      : []
                  }
                  onChange={async (r) => {
                    if (roles != null) {
                      const roleItems = r.map((item) => {
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
                  placeholder="Select roles"
                />
              </div>

              <div className="mb-3">
                <label className="text-sm font-medium text-gray-500">
                  Permissions
                </label>
                <MultiSelect
                  disabled={
                    searchParams.get("type") === STATUS_ACTION_TYPE.VIEW
                  }
                  options={
                    permissions !== null
                      ? permissions?.map((s) => {
                          return {
                            label: s.name ?? "",
                            value: s.guid?.toUpperCase() ?? "",
                          };
                        })
                      : []
                  }
                  value={user?.permissions ?? []}
                  onChange={async (permissions) => {
                    await dispatch(
                      setUser({ ...user, permissions: permissions }),
                    );
                  }}
                  placeholder="Select permissions"
                />
              </div>

              <div className="mb-3">
                <label className="text-sm font-medium text-gray-500">
                  Services
                </label>
                <ServiceSelector
                  services={
                    services !== null
                      ? services?.map((s) => {
                          return {
                            id: s.guid ?? "",
                            name: s.name ?? "",
                          };
                        })
                      : []
                  }
                  pageNum={pageNum}
                  pageSize={pageSize}
                  total={totalItems}
                  selected={
                    user?.services
                      ? user?.services.map((s) => {
                          return {
                            id: s.guid,
                            name: s.name,
                            activeDate: s.dateActive,
                            expiredDate: s.dateExpired,
                          } as SelectedService;
                        })
                      : []
                  }
                  onSelect={addService}
                  onRemove={async (id) =>
                    await dispatch(
                      setUser({
                        ...user,
                        services: user?.services?.filter((s) => s.guid != id),
                      }),
                    )
                  }
                  onSearch={handleSearch}
                  onPageChange={handlePageChange}
                  onUpdateDate={updateDate}
                  renderDatePicker={(value, onChange) => (
                    <DatePicker value={value} onChange={(v) => onChange?.(v)} />
                  )}
                />
              </div>
            </div>
          </div>
        </div>

        {searchParams.get("type") !== STATUS_ACTION_TYPE.VIEW && (
          <div className="flex justify-end gap-3 mt-8">
            <Button
              onClick={() => navigate("/users", { replace: true })}
              className="px-2"
            >
              Cancel
            </Button>
            <Button
              onClick={async () => {
                let fileId = null;
                if (file !== null && file.length > 0) {
                  fileId = await FileService.postApiFile({
                    formData: {
                      File: file[0],
                      FileType: 0,
                    },
                  });
                }

                const userPayload = { ...user };
                if (fileId != null && fileId.data) {
                  userPayload.avatarUrl = fileId.data;
                }

                await dispatch(
                  updateUser({
                    ...userPayload,
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
  );
}
