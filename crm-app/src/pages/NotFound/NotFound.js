import { Link } from "react-router-dom";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import style from "./_notfound.module.css";

export default function NotFound() {
  return (
    <Sidebar>
      <Topbar>
        <div className={style.container}>
          <h1 className={style.code}>404</h1>
          <h2>Página não encontrada</h2>
          <p>A página que você tentou acessar não existe.</p>
        </div>
      </Topbar>
    </Sidebar>
  );
}
