import { initializeApp } from "firebase/app";
import { getMessaging, isSupported, onMessage } from "firebase/messaging";

const firebaseConfig = {
  apiKey: import.meta.env.VITE_FIREBASE_API_KEY,
  authDomain: import.meta.env.VITE_FIREBASE_AUTH_DOMAIN,
  projectId: import.meta.env.VITE_FIREBASE_PROJECT_ID,
  storageBucket: import.meta.env.VITE_FIREBASE_STORAGE_BUCKET,
  messagingSenderId: import.meta.env.VITE_FIREBASE_MESSAGING_SENDER_ID,
  appId: import.meta.env.VITE_FIREBASE_APP_ID,
};

// const requiredEnv = [
//   "VITE_FIREBASE_API_KEY",
//   "VITE_FIREBASE_PROJECT_ID",
//   "VITE_FIREBASE_MESSAGING_SENDER_ID",
//   "VITE_FIREBASE_APP_ID",
// ] as const;

// requiredEnv.forEach((key) => {
//   if (!import.meta.env[key]) {
//     throw new Error(`Missing environment variable: ${key}`);
//   }
// });

export const app = initializeApp(firebaseConfig);

export const fetchMessaging = async () => {
  if (!(await isSupported())) {
    return null;
  }

  return await getMessaging(app);
};

export const listenToMessages = async (onReceive: (payload: any) => void) => {
  const messaging = await fetchMessaging();
  if (!messaging) return;

  return onMessage(messaging, onReceive);
};
