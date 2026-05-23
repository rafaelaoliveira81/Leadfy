import { useState, useRef, useEffect } from "react";
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
  MdViewKanban,
  MdSmartToy,
  MdPerson,
} from "react-icons/md";
import { SidebarItem } from "../SidebarItem/SidebarItem";
import { useNavigate } from "react-router-dom";
import { getAuthSession, clearAuthSession } from "../../services/authStorage";

const menuItems = [
  { texto: "Dashboard", link: "/dashboard", logo: <MdOutlineDashboard /> },
  { texto: "Kanban", link: "/opportunities/kanban", logo: <MdViewKanban /> },
  { texto: "Leads", link: "/leads", logo: <MdPersonSearch /> },
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
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const dropdownRef = useRef(null);
  const navigate = useNavigate();
  const session = getAuthSession();
  const userName = session?.name || "Usuário";

  useEffect(() => {
    function handleClickOutside(e) {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
        setIsDropdownOpen(false);
      }
    }
    function handleEscape(e) {
      if (e.key === "Escape") setIsDropdownOpen(false);
    }
    document.addEventListener("mousedown", handleClickOutside);
    document.addEventListener("keydown", handleEscape);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("keydown", handleEscape);
    };
  }, []);

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

        {/* ── User section ── */}
        <div className={style.userSection} ref={dropdownRef}>
          {isDropdownOpen && (
            <ul className={style.userDropdown} role="menu">
              <li role="menuitem">
                <button
                  onClick={() => {
                    setIsDropdownOpen(false);
                    navigate("/profile");
                  }}
                >
                  Perfil
                </button>
              </li>
              <li role="menuitem">
                <button
                  onClick={() => {
                    setIsDropdownOpen(false);
                    clearAuthSession();
                    navigate("/login", { replace: true });
                  }}
                >
                  Sair
                </button>
              </li>
            </ul>
          )}
          <button
            className={style.userBtn}
            onClick={() => setIsDropdownOpen((prev) => !prev)}
            aria-label="Menu do usuário"
            aria-expanded={isDropdownOpen}
          >
            <span className={style.userIcon}>
              <MdPerson />
            </span>
            <span className={style.userName}>{userName}</span>
          </button>
        </div>
      </aside>
    </>
  );
}
