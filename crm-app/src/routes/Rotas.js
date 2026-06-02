import { Navigate, Route, Routes, Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useAuth } from "../context/AuthContext";
import { Leads } from "../pages/Leads/Leads";
import NotFound from "../pages/NotFound/NotFound";
import Login from "../pages/Login/Login";
import { Users } from "../pages/Users/Users";
import Kanban from "../pages/Kanban/Kanban";
import { Prompts } from "../pages/Prompts/Prompts";
import { Products } from "../pages/Products/Products";
import { Dashboard } from "../pages/Dashboard/Dashboard";
import { Registro } from "../pages/Registro/Registro";

function ProtectedRoute() {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return null;
  }

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
}

function PublicRoute({ children }) {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return null;
  }

  if (isAuthenticated) {
    return <Navigate to="/dashboard" replace />;
  }

  return children;
}

export function Rotas() {
  return (
    <>
      <Routes>
        <Route
          path="/login"
          element={
            <PublicRoute>
              <Login />
            </PublicRoute>
          }
        />
        <Route path="/register" element={<PublicRoute><Registro /></PublicRoute>} />

        <Route element={<ProtectedRoute />}>
          <Route path="/leads" element={<Leads />} />
          <Route path="/produtos" element={<Products />} />
          <Route path="/prompts" element={<Prompts />} />
          <Route path="/usuarios" element={<Users />} />
          <Route path="/kanban" element={<Kanban />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/*" element={<NotFound />} />
        </Route>
      </Routes>
      <ToastContainer />
    </>
  );
}
