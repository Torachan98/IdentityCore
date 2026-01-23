import { Navigate, Outlet } from "react-router-dom";
import { useEffect } from "react";
import { useAppSelector } from "../../store/hooks";

export default function PermissionGuard() {
  const { token, userToken } = useAppSelector((s) => s.auth);
  if (!token) return <Navigate to="/" replace />;

  return <Outlet />;
}
