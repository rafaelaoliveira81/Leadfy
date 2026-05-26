import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Leads } from "../pages/Leads/Leads";
import NotFound from "../pages/NotFound/NotFound";

export function Rotas() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/*" element={<NotFound />} />
        <Route path="/leads" element={<Leads />} />
      </Routes>
    </BrowserRouter>
  );
}
