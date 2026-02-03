import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import { InputLabel } from "../atoms/InputLabel";
import { UserDTO } from "@/api/generated";
import Select from "@/components/atoms/Select";

type Props = {
  onSubmit: (user: UserDTO) => void;
};

export type SignUpFormHandle = {
  submit: () => boolean;
  isValid: () => boolean;
};

const SignUpForm = forwardRef<SignUpFormHandle, Props>(({ onSubmit }, ref) => {
  const [user, setUser] = useState<UserDTO>({});
  const [isSamePassword, setSamePassword] = useState(true);
  const [password, setPassword] = useState("");
  const [passwordText, setPasswordText] = useState("");

  useEffect(() => {
    setSamePassword(password === passwordText);
  }, [password, passwordText]);

  useImperativeHandle(ref, () => ({
    submit() {
      if (!isSamePassword) return false;
      onSubmit?.({ ...user, password });

      return true;
    },
    isValid() {
      return Boolean(user.email && user.userName && password && isSamePassword);
    },
  }));

  return (
    <div className="flex flex-col py-3 px-4 gap-2">
      <h5 className="text-xl text-bold">Sign up</h5>
      <div className="flex flex-row gap-3">
        <InputLabel
          label="First Name"
          placeholder="First Name"
          value={user.firstName}
          onChange={(e) => setUser({ ...user, firstName: e.target.value })}
        />

        <InputLabel
          label="Middle Name"
          placeholder="Middle Name"
          value={user.middleName}
          onChange={(e) => setUser({ ...user, middleName: e.target.value })}
        />

        <InputLabel
          label="Last Name"
          placeholder="Last Name"
          value={user.lastName}
          onChange={(e) => setUser({ ...user, lastName: e.target.value })}
        />
      </div>

      <InputLabel
        label="Email"
        value={user.email}
        onChange={(e) => {
          setUser({
            ...user,
            email: e.target.value,
            userName: e.target.value.split("@")[0],
          });
        }}
      />

      <InputLabel
        label="Username"
        placeholder="Username"
        value={user.userName}
        onChange={(e) => setUser({ ...user, userName: e.target.value })}
      />

      <div className="flex flex-row gap-3">
        <div className="grow-0">
          <InputLabel label="Code" placeholder="+84" />
        </div>

        <div className="grow">
          <InputLabel label="Phone" placeholder="0xxxxxxx" />
        </div>

        <div className="grow">
          <div className="flex flex-col justify-between gap-1">
            <label className="text-xs font-medium text-gray-500">Country</label>
            <Select
              className="text-sm"
              options={[
                { value: "VN", label: "Viet Nam" },
                { value: "US", label: "United States" },
                { value: "JP", label: "Japan" },
                { value: "PH", label: "Philipines" },
              ]}
              value={user.region}
              onChange={async (e) => {
                setUser({ ...user, region: e.target.value });
              }}
            />
          </div>
        </div>
      </div>

      <div className="flex flex-col gap-2">
        <div className="flex flex-row gap-2">
          <InputLabel
            className={`${!isSamePassword ? "border border-red-500" : ""}`}
            label="Password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <InputLabel
            className={`${!isSamePassword ? "border border-red-500" : ""}`}
            label="Re-password"
            type="password"
            value={passwordText}
            onChange={(e) => setPasswordText(e.target.value)}
          />
        </div>

        {!isSamePassword && (
          <p className="text-red-400 text-bold">Password is not match</p>
        )}
      </div>
    </div>
  );
});

export default SignUpForm;
