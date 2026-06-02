import "./App.css";
import { BrowserRouter } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { Rotas } from "./routes/Rotas";
import { ToastContainer } from "react-toastify";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Rotas />
        <ToastContainer />
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
