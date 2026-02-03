import Button from "@/components/atoms/Button";

type Props = {
  onAllow: () => void;
  onDismiss: () => void;
};

export const NotificationPromptCard = ({ onAllow, onDismiss }: Props) => {
  return (
    <div className="w-[360px] bg-white rounded-xl shadow-lg p-5 border border-gray-100">
      <h3 className="text-lg font-semibold text-gray-900">
        Enable notifications?
      </h3>

      <p className="mt-2 text-sm text-gray-600">
        Stay updated with important activity and messages.
      </p>

      <div className="mt-4 flex justify-end gap-2">
        <Button className="px-2" onClick={onDismiss}>
          Maybe later
        </Button>
        <Button className="px-2" onClick={onAllow}>
          Allow
        </Button>
      </div>
    </div>
  );
};
