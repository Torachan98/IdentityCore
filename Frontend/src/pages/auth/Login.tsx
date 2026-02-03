import { useDispatch } from "react-redux";
import { Input } from "@/components/atoms/Input";
import Button from "@/components/atoms/Button";
import { useState } from "react";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { replace, useNavigate } from "react-router-dom";
import { signIn } from "@/store/auth/auth.thunk";

/// @ts-expect-error
import Logo from "@/assets/identity_service_logo.png?w=150";

export default function Login() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { error } = useAppSelector((state) => state.auth);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  return (
    <div className="min-h-screen flex flex-col items-center justify-center">
      <div className="mx-auto px-2 py-2 sm:px-6 lg:px-10 max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg">
        <img
          className="w-full h-auto object-contain"
          alt="Identity Service Logo"
          src={Logo}
        />
      </div>
      <div className="space-y-4">
        <Input
          className="border border-gray-600 p-1"
          icon={"user"}
          placeholder="User name or Email"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />

        <Input
          className="border border-gray-600 p-1"
          icon={"lock"}
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <Button
          className="w-full"
          onClick={async () => {
            const res = await dispatch(
              signIn({ userName: username, password: password }),
            ).unwrap();

            if (res.isSuccess) {
              navigate("/", { replace: true });
              return;
            }
          }}
        >
          Login
        </Button>

        <div
          className="text-center text-sm text-bold text-black cursor-pointer hover:underline underline-offset-1"
          onClick={() => navigate("/forgot-password")}
        >
          Forgot password ?
        </div>

        <hr className="h-px my-8 bg-gray-300 border-0" />

        <Button className="w-full" onClick={() => navigate("/sign-up")}>
          Sign Up
        </Button>

        <p>{error}</p>
      </div>
    </div>
  );
}
