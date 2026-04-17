import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { ToastProvider } from './components/ui/Toast/Toast';
import { Home } from './pages/Home/Home';
import { Products } from './pages/Products/Products';

function App() {
  return (
    <ToastProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/products" element={<Products />} />
        </Routes>
      </BrowserRouter>
    </ToastProvider>
  );
}

export default App;
