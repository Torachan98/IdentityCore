import { Navigate, Outlet } from "react-router-dom";
import { useAppSelector } from "@/store/hooks";

export default function PermissionGuard() {
  const { token, userToken, loading } = useAppSelector((s) => s.auth);

  if (loading) {
    return <div>loading...</div>;
  }

  if (!userToken) {
    return <Navigate to="/404" replace />;
  }

  return <Outlet />;
}
