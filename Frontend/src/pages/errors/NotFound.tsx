import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate } from "react-router-dom";

export default function NotFound() {
  const navigate = useNavigate();

  return (
    <div className="flex min-h-full items-center justify-center px-6 py-6">
      <div className="text-center max-w-md">
        <h1 className="text-7xl font-extrabold text-blue-600">404</h1>

        <p className="mt-4 text-2xl font-semibold text-gray-800">
          Oops! Page not found
        </p>

        <p className="mt-2 text-gray-500">
          The page you’re looking for doesn’t exist or has been moved. Let’s get
          you back on track.
        </p>

        <div className="mt-6 flex justify-center gap-4">
          <button
            onClick={() => navigate(-1)}
            className="rounded-lg border px-5 py-2 text-gray-600 hover:bg-gray-100"
          >
            <FontAwesomeIcon icon={"arrow-left"} /> Go back
          </button>

          <button
            onClick={() => navigate("/", { replace: true })}
            className="rounded-lg bg-blue-600 px-5 py-2 text-white hover:bg-blue-700"
          >
            Go to home
          </button>
        </div>

        <p className="mt-6 text-sm text-gray-400">
          If you think this is a mistake, please contact support.
        </p>
      </div>
    </div>
  );
}
