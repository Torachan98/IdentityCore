import { useEffect, useRef, useState } from "react";
import LoadingBar from "@/components/atoms/StepLoadingBar";
import OtpForm from "@/components/molecules/OtpForm";
import SignUpForm, {
  SignUpFormHandle,
} from "@/components/molecules/SignUpForm";
import Button from "@/components/atoms/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

enum StepForm {
  FORGOT_PASSWORD = "forgot-password",
  SIGN_UP = "sign-up",
  OTP_PROCESS = "otp-process",
}

export default function SignUp() {
  const [step, setStep] = useState<StepForm>(StepForm.SIGN_UP);
  const [stepProcess, setStepProcess] = useState(1);
  const [minutes, setMinutes] = useState(0);
  const [otpCode, setOtpCode] = useState("");

  const signUpRef = useRef<SignUpFormHandle>(null);

  useEffect(() => {
    const id = setInterval(() => {
      setMinutes((prev) => {
        if (prev <= 1) {
          clearInterval(id);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(id);
  }, [minutes]);

  useEffect(() => {
    setMinutes(50);
  }, [step]);

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="w-full max-w-lg bg-white rounded-2xl shadow-lg p-4 space-y-6">
        <div className="space-y-2">
          <LoadingBar steps={2} currentStep={stepProcess} />
          <p className="text-sm text-gray-500 text-right">
            Step {stepProcess} of 2
          </p>
        </div>

        <div className="relative">
          {step === StepForm.SIGN_UP && (
            <div className="animate-fadeIn">
              <SignUpForm ref={signUpRef} onSubmit={() => console.log()} />
            </div>
          )}

          {step === StepForm.OTP_PROCESS && (
            <>
              <div className="text-center py-3">
                <p>
                  Enter your code which already sent from email <br /> Your code
                  only valid at: 00:{minutes < 10 ? "0" + minutes : minutes}
                </p>
              </div>
              <div className="animate-fadeIn">
                <OtpForm onComplete={(val) => setOtpCode(val)} />
              </div>
            </>
          )}
        </div>

        <div className="flex flex-row gap-2 justify-end">
          <Button
            className="px-2"
            disabled={step === StepForm.OTP_PROCESS && otpCode === ""}
            onClick={() => {
              const isSuccess = signUpRef.current?.isValid();
              if (isSuccess) {
                setStepProcess(stepProcess + 1);
                setStep(StepForm.OTP_PROCESS);
              }
            }}
          >
            {step === StepForm.SIGN_UP ? "Submit" : "OK"}
          </Button>

          {step === StepForm.OTP_PROCESS && (
            <Button
              className="px-2"
              disabled={minutes !== 0}
              onClick={() => setMinutes(50)}
            >
              Resent email
            </Button>
          )}
        </div>
      </div>
    </div>
  );
}
