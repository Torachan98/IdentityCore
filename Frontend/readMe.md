# Identity Admin – ReactJS Frontend

A modern admin frontend built with **React + TypeScript**, following **Atomic Design**, **Redux Toolkit**, **TailwindCSS**, and **Swagger-based API generation**.

## React-Spinner (https://github.com/mhnpd/react-loader-spinner)

## 🚀 Tech Stack

- **React 18**
- **TypeScript**
- **Redux Toolkit** (state management)
- **Axios** (HTTP client)
- **openapi-typescript-codegen** (API client generation)
- **Tailwind CSS** (UI styling)
- **Vite** (build tool)

---

## 📁 Project Structure

```txt
src/
├── app/                # App setup (store, providers)
├── components/
│   ├── atoms/          # Button, Input, Loader...
│   ├── molecules/      # FormField, ModalHeader...
│   ├── organisms/      # DataTable, Pagination...
│   └── templates/      # Page layouts
├── features/           # Redux slices (auth, user, etc.)
├── pages/              # Page-level components
├── shared/
│   ├── api/
│   │   └── generated/  # Swagger auto-generated API
│   ├── hooks/
│   ├── types/
│   └── utils/
├── styles/
├── main.tsx
└── App.tsx
```

## ⚙️ Environment Setup

```npm install

```
