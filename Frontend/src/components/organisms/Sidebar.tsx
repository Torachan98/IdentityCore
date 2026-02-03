import { SidebarItem } from "@/components/molecules/SidebarItem";
import { Icon } from "@/components/atoms/Icon";
import { replace, useNavigate } from "react-router-dom";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useAppDispatch } from "@/store/hooks";
import { UserGreeting } from "@/components/molecules/UserGreeting";
import { signOut } from "@/store/auth/auth.thunk";
import { BooleanApiResponse } from "@/api/generated";
import { toast } from "react-toastify";

type Props = {
  firstName: string;
  lastName: string;
  collapsed: boolean;
  onToggle: () => void;
};

export function Sidebar({ firstName, lastName, collapsed, onToggle }: Props) {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const isActive = (path: string) => location.pathname.startsWith(path);
  //const { roles } = useAppSelector((state) => state.auth);

  return (
    <aside
      className={`h-screen border-r bg-white transition-all duration-300
      ${collapsed ? "w-16" : "w-64"}`}
    >
      <div className="flex justify-end px-3 py-2">
        <button onClick={onToggle} className="rounded-md p-1 hover:bg-gray-100">
          <FontAwesomeIcon icon={"bars"} />
        </button>
      </div>

      <UserGreeting
        firstName={firstName}
        lastName={lastName}
        collapsed={collapsed}
      />

      <nav className="space-y-1 px-2">
        <SidebarItem
          icon={
            <Icon>
              <FontAwesomeIcon icon={"user-gear"} />
            </Icon>
          }
          label={"Users"}
          collapsed={false}
          active={isActive("/users")}
          onClick={() => {
            navigate("/users", { replace: true });
          }}
        />

        <SidebarItem
          icon={
            <Icon>
              <FontAwesomeIcon icon={"person-praying"} />
            </Icon>
          }
          label={"Permissions"}
          collapsed={false}
          active={isActive("/permissions")}
          onClick={() => {
            navigate("/permissions", { replace: true });
          }}
        />

        <SidebarItem
          icon={
            <Icon>
              <FontAwesomeIcon icon={"person-military-rifle"} />
            </Icon>
          }
          label={"Roles"}
          collapsed={false}
          active={isActive("/roles")}
          onClick={() => {
            navigate("/roles", { replace: true });
          }}
        />

        <SidebarItem
          icon={
            <Icon>
              <FontAwesomeIcon icon={"screwdriver-wrench"} />
            </Icon>
          }
          label={"Services"}
          collapsed={false}
          active={isActive("/services")}
          onClick={() => {
            navigate("/services", { replace: true });
          }}
        />
        {/* {roles.includes("Guest") && (
          <>
            <SidebarItem
              icon={
                <Icon>
                  <FontAwesomeIcon icon={"utensils"} />
                </Icon>
              }
              label="Menu"
              collapsed={collapsed}
              active={isActive("/products")}
              onClick={() => {
                navigate("/products", { replace: true });
              }}
            />

            <SidebarItem
              icon={
                <Icon>
                  <FontAwesomeIcon icon={"clock-rotate-left"} />
                </Icon>
              }
              label="Order History"
              collapsed={collapsed}
              active={isActive("/orders-history")}
              onClick={() => {
                navigate("/orders-history", { replace: true });
              }}
            />
          </>
        )}

        {roles.includes("Admininstrator") && <></>} */}

        <SidebarItem
          icon={
            <Icon>
              <FontAwesomeIcon icon="arrow-right-from-bracket" />
            </Icon>
          }
          label="Sign out"
          collapsed={collapsed}
          active={false}
          onClick={async () => {
            await dispatch(signOut()).then((s) => {
              const res = s.payload as BooleanApiResponse;
              if (res.isSuccess && res.data) {
                navigate("/login", { replace: true });
              } else {
                toast.error(res.message);
              }
            });
          }}
        />
      </nav>
    </aside>
  );
}
