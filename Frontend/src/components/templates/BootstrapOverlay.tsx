import { useEffect } from "react";
import { renewToken } from "@/store/auth/auth.thunk";
import { useAppDispatch, useAppSelector } from "@/store/hooks";

export default function BootstrapOverlay({
  children,
}: {
  children: React.ReactNode;
}) {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(renewToken(null));
  }, [dispatch]);

  return <>{children}</>;
}
