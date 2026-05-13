import "./App.css";
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { ProtectedRoute } from "./components/auth/ProtectedRoute";
import { ToastProvider } from "./components/ui/Toast/Toast";
import { Home } from "./pages/Home/Home";
import { Products } from "./pages/Products/Products";
import { Leads } from "./pages/Leads/Leads";
import { Owners } from "./pages/Owners/Owners";
import { Users } from "./pages/Users/Users";
import { Opportunities } from "./pages/Opportunities/Opportunities";
import { OpportunitiesKanban } from "./pages/Opportunities/Kanban/OpportunitiesKanban";
import { AiConfig } from "./pages/AiConfig/AiConfig";
import { Login } from "./pages/Login/Login";

function App() {
  return (
    <ToastProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route element={<ProtectedRoute />}>
            <Route path="/" element={<Home />} />
            <Route path="/products" element={<Products />} />
            <Route path="/leads" element={<Leads />} />
            <Route path="/owners" element={<Owners />} />
            <Route path="/users" element={<Users />} />
            <Route path="/opportunities" element={<Opportunities />} />
            <Route
              path="/opportunities/kanban"
              element={<OpportunitiesKanban />}
            />
            <Route path="/ai-config" element={<AiConfig />} />
          </Route>
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
    </ToastProvider>
  );
}

export default App;
