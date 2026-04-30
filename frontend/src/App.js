import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { ToastProvider } from './components/ui/Toast/Toast';
import { Home } from './pages/Home/Home';
import { Products } from './pages/Products/Products';
import { Leads } from './pages/Leads/Leads';
import { Owers } from './pages/Owers/Owers';
import { Users } from './pages/Users/Users';
import { Opportunities } from './pages/Opportunities/Opportunities';
import { OpportunitiesKanban } from './pages/Opportunities/Kanban/OpportunitiesKanban';
import { AiConfig } from './pages/AiConfig/AiConfig';

function App() {
  return (
    <ToastProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/products" element={<Products />} />
          <Route path="/leads" element={<Leads />} />
          <Route path="/owners" element={<Owers />} />
          <Route path="/users" element={<Users />} />
          <Route path="/opportunities" element={<Opportunities />} />
          <Route path="/opportunities/kanban" element={<OpportunitiesKanban />} />
          <Route path="/ai-config" element={<AiConfig />} />
        </Routes>
      </BrowserRouter>
    </ToastProvider>
  );
}

export default App;
