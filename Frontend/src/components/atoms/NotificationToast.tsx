import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useEffect } from "react";
import { toast, ToastContentProps } from "react-toastify";

type Props = ToastContentProps & {
  title: string;
  message: string;
};

export function NotificationToast({ title, message, toastProps }: Props) {
  return (
    <div className="flex items-center w-[340px] gap-3 rounded-xl bg-white p-4 shadow-lg ring-1 ring-black/5 dark:bg-slate-800">
      <div className="flex-1">
        <div className="flex flex-row justify-start items-center gap-3">
          <FontAwesomeIcon icon={"bell"} />
          <p className="text-sm font-semibold text-slate-900 dark:text-slate-100">
            {title}
          </p>
        </div>
        <p className="mt-1 text-sm text-slate-600 dark:text-slate-400">
          {message}
        </p>
      </div>

      <div className="flex flex-col">
        <button
          onClick={() => toast.dismiss(toastProps.toastId)}
          className="self-start rounded-md px-2 py-1 text-sm font-medium text-blue-600 transition hover:bg-blue-50 dark:text-blue-400 dark:hover:bg-blue-500/10"
        >
          Dismiss
        </button>
      </div>
    </div>
  );
}
