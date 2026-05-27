import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Leads } from "../pages/Leads/Leads";
import NotFound from "../pages/NotFound/NotFound";
import Login from "../pages/Login/Login";

export function Rotas() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/*" element={<NotFound />} />
        <Route path="/leads" element={<Leads />} />
      </Routes>
    </BrowserRouter>
  );
}
