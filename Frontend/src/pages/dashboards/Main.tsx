import { Outlet } from "react-router-dom";
import { Sidebar } from "@/components/organisms/Sidebar";
import { useState } from "react";
import { useAppSelector } from "@/store/hooks";
import { NotificationPermissionFloat } from "@/components/organisms/NotificationPermissionFloat";

export default function Main() {
  const [collapsed, setCollapsed] = useState(false);
  const { userToken } = useAppSelector((state) => state.auth);
  return (
    <div className="flex h-screen overflow-hidden bg-gray-50">
      <Sidebar
        firstName={userToken?.firstName ?? ""}
        lastName={userToken?.lastName ?? ""}
        collapsed={collapsed}
        onToggle={() => setCollapsed((v) => !v)}
      />

      <main className="flex-1 overflow-y-auto p-6">
        <Outlet />
      </main>

      <NotificationPermissionFloat />
    </div>
  );
}
