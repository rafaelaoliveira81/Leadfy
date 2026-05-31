import { Navigate, Route, Routes, Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useAuth } from "../context/AuthContext";
import { Leads } from "../pages/Leads/Leads";
import NotFound from "../pages/NotFound/NotFound";
import Login from "../pages/Login/Login";
import { Users } from "../pages/Users/Users";
import Kanban from "../pages/Kanban/Kanban";

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
    return <Navigate to="/leads" replace />;
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

        <Route element={<ProtectedRoute />}>
          <Route path="/leads" element={<Leads />} />
          {/* <Route path="/dashboard" element={<Dashboard />} /> */}
          <Route path="/usuarios" element={<Users />} />
          <Route path="/kanban" element={<Kanban />} />
          <Route path="/*" element={<NotFound />} />
        </Route>
      </Routes>
      <ToastContainer />
    </>
  );
}
