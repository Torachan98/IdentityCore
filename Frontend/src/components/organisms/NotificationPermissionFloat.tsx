import { useEffect, useState } from "react";
import { getToken } from "firebase/messaging";
import { jwtDecode } from "jwt-decode";
import { NotificationPromptCard } from "@/components/molecules/NotificationPromptCard";
import { fetchMessaging } from "@/fcmService";
import { fetchUserById, updateUser } from "@/store/user/user.thunk";
import { useAppDispatch, useAppSelector } from "@/store/hooks";

const STORAGE_KEY = "notification_prompt_status";

enum STATUS_NOTIFICATION_FLOAT {
  ACCEPT = "accepted",
  BLOCK = "blocked",
  DISMISS = "dismissed",
}

export const NotificationPermissionFloat = () => {
  const dispatch = useAppDispatch();
  const { token } = useAppSelector((state) => state.auth);
  const { user } = useAppSelector((state) => state.user);

  const [visible, setVisible] = useState(false);

  useEffect(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (
      (!stored && Notification.permission === "default") ||
      stored === STATUS_NOTIFICATION_FLOAT.DISMISS
    ) {
      setVisible(true);
    }
  }, []);

  const handleAllow = async () => {
    try {
      await requestPermissionAndToken();
      localStorage.setItem(STORAGE_KEY, STATUS_NOTIFICATION_FLOAT.ACCEPT);
    } catch {
      localStorage.setItem(STORAGE_KEY, STATUS_NOTIFICATION_FLOAT.BLOCK);
    } finally {
      setVisible(false);
    }
  };

  const requestPermissionAndToken = async () => {
    const permission = await Notification.requestPermission();

    if (permission !== "granted") {
      localStorage.setItem(STORAGE_KEY, STATUS_NOTIFICATION_FLOAT.DISMISS);
    }

    const messaging = await fetchMessaging();
    if (!messaging) return null;

    const fmcToken = await getToken(messaging);

    const userToken = jwtDecode<{ userId: string }>(token);

    await dispatch(fetchUserById(userToken.userId)).then(async () => {
      await dispatch(updateUser({ ...user, fcmToken: fmcToken }));
    });
  };

  const handleDismiss = () => {
    localStorage.setItem(STORAGE_KEY, STATUS_NOTIFICATION_FLOAT.DISMISS);
    setVisible(false);
  };

  if (!visible) return null;

  return (
    <div className="fixed bottom-6 right-6 z-50 animate-fade-in">
      <NotificationPromptCard onAllow={handleAllow} onDismiss={handleDismiss} />
    </div>
  );
};
