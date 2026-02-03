import { Circles } from "react-loader-spinner";
import { useEffect } from "react";
import { ToastContainer, toast } from "react-toastify";
import Router from "@/router";
import { useAppSelector } from "@/store/hooks";
import { listenToMessages } from "@/fcmService";
import { NotificationToast } from "@/components/atoms/NotificationToast";

export default function App() {
  const loadingAuth = useAppSelector((state) => state.auth.loading);
  const loadingUser = useAppSelector((state) => state.user.loading);
  const loadingPermission = useAppSelector((state) => state.permission.loading);

  useEffect(() => {
    let unsubscribe: (() => void) | undefined;

    listenToMessages((payload) => {
      toast(
        (props) => (
          <NotificationToast
            {...props}
            title={payload.notification?.title ?? ""}
            message={payload.notification?.body ?? ""}
          />
        ),
        {
          autoClose: 5000,
          closeButton: false,
          hideProgressBar: false,
          draggable: true,
        },
      );
    }).then((u) => (unsubscribe = u));

    return () => unsubscribe?.();
  }, []);

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
      <ToastContainer
        position="bottom-right"
        newestOnTop
        toastStyle={{
          background: "transparent",
          padding: 0,
          boxShadow: "none",
        }}
      />

      <Router />
    </>
  );
}
