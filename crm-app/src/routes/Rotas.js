import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Home } from "../pages/Home/Home";
import { Leads } from "../pages/Leads/Leads";

export function Rotas() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/*" element={<Home />} />
        <Route path="/leads" element={<Leads />} />
      </Routes>
    </BrowserRouter>
  );
}
