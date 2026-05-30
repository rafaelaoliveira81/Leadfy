import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Button } from "../Button/Button";
import { useAuth } from "../../context/AuthContext";
import { MdLogout } from "react-icons/md";
import style from "./_topbar.module.css";

export function Topbar({ children }) {
  const navigate = useNavigate();
  const { logout } = useAuth();

  const handleClick = () => {
    logout();
    toast.success("Logout efetuado com sucesso! ");
    navigate("/login");
  };

  return (
    <div>
      <div className={style["topbar-conteudo"]}>
        <button className={style["botao-logout"]} onClick={handleClick}>
          <MdLogout />
        </button>
      </div>
      <div className={style["pagina-conteudo"]}>{children}</div>
    </div>
  );
}
