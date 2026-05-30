import { Navigate, Route, Routes, Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useAuth } from "../context/AuthContext";
import { Leads } from "../pages/Leads/Leads";
import NotFound from "../pages/NotFound/NotFound";
import Login from "../pages/Login/Login";

function ProtectedRoute() {
  const { isAuthenticated } = useAuth();

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
}

function PublicRoute({ children }) {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/leads" replace />;
  }

  return children;
}

export function Rotas() {
  const { isAuthenticated } = useAuth();

  return (
    <>
      <Routes>
        <Route path="/login" element={<Login />} />

        <Route element={<ProtectedRoute />}>
          <Route path="/leads" element={<Leads />} />
          {/* <Route path="/dashboard" element={<Dashboard />} /> */}
          {/* <Route path="/users" element={<Users />} /> */}
          <Route path="/*" element={<NotFound />} />
        </Route>
      </Routes>
      <ToastContainer />
    </>
  );
}
