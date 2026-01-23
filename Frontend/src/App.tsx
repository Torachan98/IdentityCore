import { Circles, Oval } from "react-loader-spinner";
import Router from "./router";
import { useAppSelector } from "./store/hooks";

export default function App() {
  const loadingAuth = useAppSelector((state) => state.auth.loading);
  const loadingUser = useAppSelector((state) => state.user.loading);
  const loadingPermission = useAppSelector((state) => state.permission.loading);

  return (
    <>
      <Circles
        height="80"
        width="80"
        color="#3e6ae4ff"
        ariaLabel="circles-loading"
        wrapperStyle={{}}
        wrapperClass="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm"
        visible={loadingAuth || loadingUser || loadingPermission}
      />
      <Router />
    </>
  );
}
