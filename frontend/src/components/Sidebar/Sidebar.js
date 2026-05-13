import style from "./_sidebar.module.css";
import { RiTeamFill } from "react-icons/ri";
import {
  MdOutlineDashboard,
  MdPeople,
  MdPersonSearch,
  MdShoppingCart,
  MdMenuOpen,
  MdMenu,
  MdClose,
  MdTrendingUp,
  MdViewKanban,
  MdSmartToy,
} from "react-icons/md";
import { SidebarItem } from "../SidebarItem/SidebarItem";

const menuItems = [
  { texto: "Dashboard", link: "/dashboard", logo: <MdOutlineDashboard /> },
  { texto: "Kanban", link: "/opportunities/kanban", logo: <MdViewKanban /> },
  { texto: "Leads", link: "/leads", logo: <MdPersonSearch /> },
  // { texto: 'Oportunidades', link: '/opportunities', logo: <MdTrendingUp /> },
  { texto: "Produtos", link: "/products", logo: <MdShoppingCart /> },
  { texto: "Responsáveis", link: "/owners", logo: <RiTeamFill /> },
  { texto: "Usuários", link: "/users", logo: <MdPeople /> },
  { texto: "Config. de IA", link: "/ai-config", logo: <MdSmartToy /> },
];

export function Sidebar({
  isCollapsed,
  onToggle,
  isMobileOpen,
  onCloseMobile,
}) {
  return (
    <>
      {isMobileOpen && (
        <div
          className={style.overlay}
          onClick={onCloseMobile}
          aria-hidden="true"
        />
      )}
      <aside
        className={`${style.sidebar} ${isCollapsed ? style.collapsed : ""} ${isMobileOpen ? style.mobileOpen : ""}`}
      >
        <div className={style.header}>
          <span className={style.title}>CRM Admin</span>
          <button
            className={style.toggleBtn}
            onClick={onToggle}
            aria-label={isCollapsed ? "Expandir menu" : "Minimizar menu"}
          >
            {isCollapsed ? <MdMenu /> : <MdMenuOpen />}
          </button>
          <button
            className={style.closeBtn}
            onClick={onCloseMobile}
            aria-label="Fechar menu"
          >
            <MdClose />
          </button>
        </div>
        <hr className={style.divider} />
        <nav className={style.nav}>
          {menuItems.map((item) => (
            <SidebarItem
              key={item.link}
              texto={item.texto}
              link={item.link}
              logo={item.logo}
              isCollapsed={isCollapsed}
              onClick={onCloseMobile}
            />
          ))}
        </nav>
      </aside>
    </>
  );
}
