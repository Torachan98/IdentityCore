import { Routes, Route } from "react-router-dom";
import Login from "@/pages/auth/Login";
import Users from "@/pages/dashboards/Users";
import Permissions from "@/pages/dashboards/Permissions";
import UserDetail from "@/pages/details/UserDetail";
import PermissionGuard from "@/components/templates/PermissionGuard";
import BootstrapOverlay from "@/components/templates/BootstrapOverlay";
import Main from "@/pages/dashboards/Main";
import Roles from "@/pages/dashboards/Roles";
import Services from "@/pages/dashboards/Services";
import RoleDetail from "@/pages/details/RoleDetail";
import NotFound from "@/pages/errors/NotFound";
import Init from "@/pages/dashboards/Init";
import ForgotPassword from "@/pages/auth/ForgotPassword";
import ServiceDetail from "@/pages/details/ServiceDetail";
import SignUp from "@/pages/auth/SignUp";

export default function Router() {
  return (
    <BootstrapOverlay>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/sign-up" element={<SignUp />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route element={<PermissionGuard />}>
          <Route element={<Main />}>
            <Route path="/" element={<Init />} />
            <Route path="/users" element={<Users />} />
            <Route path="/permissions" element={<Permissions />} />
            <Route path="/services" element={<Services />} />
            <Route path="/roles" element={<Roles />} />
            <Route path="/user-detail/:id" element={<UserDetail />} />
            <Route path="/role-detail/:id" element={<RoleDetail />} />
            <Route path="/service-detail/:id" element={<ServiceDetail />} />
          </Route>
        </Route>
        <Route path="/404" element={<NotFound />} />
      </Routes>
    </BootstrapOverlay>
  );
}
