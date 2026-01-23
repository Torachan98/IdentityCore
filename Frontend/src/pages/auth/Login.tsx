import { useDispatch } from "react-redux";
import { Input } from "../../components/atoms/Input";
import Button from "../../components/atoms/Button";
import { useState } from "react";
import { useAppDispatch, useAppSelector } from "../../store/hooks";
import { replace, useNavigate } from "react-router-dom";
import { signIn } from "../../store/auth/auth.thunk";

export default function Login() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { token, error, loading } = useAppSelector((state) => state.auth);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  return (
    <div className="min-h-screen flex flex-col items-center justify-center">
      <div className="space-y-4">
        <Input
          className="border border-gray-600 p-1"
          icon={"user"}
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
              navigate("/users", { replace: true });
              return;
            }
          }}
        >
          Login
        </Button>

        <p>{error}</p>
      </div>
    </div>
  );
}
