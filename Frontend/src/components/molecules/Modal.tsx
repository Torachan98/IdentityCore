import { ReactNode } from "react";

type ModalProps = {
  open: boolean;
  onClose?: () => void;
  closeOnOverlayClick?: boolean;
  childrenBody: ReactNode;
  childrenTitle: ReactNode;
};

export default function Modal({
  open,
  onClose,
  closeOnOverlayClick,
  childrenBody,
  childrenTitle,
}: ModalProps) {
  if (!open) return null;

  return (
    <div
      className="fixed inset-0 flex items-center justify-center bg-black/40"
      onClick={closeOnOverlayClick ? onClose : undefined}
    >
      <div
        className="w-[30%] rounded bg-white p-6"
        onClick={(e) => e.stopPropagation()}
      >
        {childrenTitle}
        <div className="max-h-[80vh] overflow-y-auto">{childrenBody}</div>
      </div>
    </div>
  );
}
