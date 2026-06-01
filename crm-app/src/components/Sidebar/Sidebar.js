import { useState, useRef, useEffect } from "react";
import style from "./_sidebar.module.css";
import { SidebarItem } from "../SidebarItem/SidebarItem";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import { RiTeamFill } from "react-icons/ri";
import { MdOutlineDashboard } from "react-icons/md";
import { MdPerson } from "react-icons/md";
import { MdSmartToy } from "react-icons/md";
import { MdViewKanban } from "react-icons/md";
import { MdClose } from "react-icons/md";
import { MdMenu } from "react-icons/md";
import { MdMenuOpen } from "react-icons/md";
import { MdShoppingCart } from "react-icons/md";
import { MdPersonSearch } from "react-icons/md";
import { MdPeople } from "react-icons/md";
import logo from "../../assets/logo.png";

export function Sidebar({ children }) {
  return (
    <div className={style.container}>
      <div className={style["container-sidebar"]}>
        <div className={style["header"]}>
          <Link to="/dashboard">
            <img src={logo} alt="Logo-Leadfy" className={style.logo} />
          </Link>
        </div>
        <hr className={style.divider} />
        <div className={style["style.nav"]}>
          <SidebarItem
            texto="Dashboard"
            link="/dashboard"
            logo={<MdOutlineDashboard />}
          />
          <SidebarItem texto="Kanban" link="/kanban" logo={<MdViewKanban />} />
          <hr className={style.divider} />
          <SidebarItem texto="Leads" link="/leads" logo={<MdPersonSearch />} />
          <SidebarItem
            texto="Produtos"
            link="/produtos"
            logo={<MdShoppingCart />}
          />
          <SidebarItem texto="Usuários" link="/usuarios" logo={<MdPeople />} />

          <hr className={style.divider} />
          <SidebarItem texto="Prompts" link="/prompts" logo={<MdSmartToy />} />
        </div>
      </div>
      <div className={style["pagina-conteudo"]}>{children}</div>
    </div>
  );
}
