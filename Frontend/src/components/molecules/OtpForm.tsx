import { useEffect, useRef, useState } from "react";

type Props = {
  length?: number;
  onComplete?: (code: string) => void;
};

export default function OTPForm({ length = 5, onComplete }: Props) {
  const [otp, setOtp] = useState<string[]>(Array(length).fill(""));
  const inputsRef = useRef<HTMLInputElement[]>([]);

  const focusInput = (index: number) => {
    inputsRef.current[index]?.focus();
  };

  const handleChange = (value: string, index: number) => {
    if (!/^\d?$/.test(value)) return;

    const next = [...otp];
    next[index] = value;
    setOtp(next);

    if (value && index < length - 1) {
      focusInput(index + 1);
    }
  };

  const handleKeyDown = (
    e: React.KeyboardEvent<HTMLInputElement>,
    index: number,
  ) => {
    if (e.key === "Backspace" && !otp[index] && index > 0) {
      focusInput(index - 1);
    }
  };

  const handlePaste = (e: React.ClipboardEvent<HTMLInputElement>) => {
    e.preventDefault();

    const pasted = e.clipboardData
      .getData("text")
      .slice(0, length)
      .split("")
      .filter((c) => /^\d$/.test(c));

    if (!pasted.length) return;

    const next = Array(length).fill("");
    pasted.forEach((c, i) => {
      next[i] = c;
    });

    setOtp(next);
    focusInput(Math.min(pasted.length, length) - 1);
  };

  useEffect(() => {
    onComplete?.(otp.join(""));
  }, [otp, onComplete]);

  return (
    <div className="flex justify-center gap-3">
      {otp.map((value, index) => (
        <input
          key={index}
          ref={(el) => {
            if (el) inputsRef.current[index] = el;
          }}
          type="text"
          inputMode="numeric"
          maxLength={1}
          value={value}
          onChange={(e) => handleChange(e.target.value, index)}
          onKeyDown={(e) => handleKeyDown(e, index)}
          onPaste={handlePaste}
          className="h-14 w-12 rounded-lg border text-center text-lg font-semibold focus:border-blue-500 focus:outline-none"
        />
      ))}
    </div>
  );
}
