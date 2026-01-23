import React from "react";
import ReactDOM from "react-dom/client";
import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { AppDispatch, store } from "./store";
import "./index.css";

/* import all the icons in Free Solid, Free Regular, and Brands styles */
import { fas } from "@fortawesome/free-solid-svg-icons";
import { far } from "@fortawesome/free-regular-svg-icons";
import { fab } from "@fortawesome/free-brands-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import { setupInterceptors } from "./api/setupInterceptors";
import { renewToken } from "./store/auth/auth.thunk";

library.add(fas, far, fab);

const dispatch = store.dispatch as AppDispatch;

setupInterceptors(
  () => store.getState().auth.token ?? "",
  async () => {
    await dispatch(renewToken(null)).unwrap();
  },
);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </Provider>
  </React.StrictMode>,
);
