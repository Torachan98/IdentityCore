type StepLoadingBarProps = {
  steps: number;
  currentStep: number;
};

export default function StepLoadingBar({
  steps,
  currentStep,
}: StepLoadingBarProps) {
  return (
    <div className="flex gap-2 w-full">
      {Array.from({ length: steps }).map((_, index) => (
        <div
          key={index}
          className={`h-2 flex-1 rounded transition-colors duration-300 ${
            index < currentStep ? "bg-blue-500" : "bg-gray-200"
          }`}
        />
      ))}
    </div>
  );
}
