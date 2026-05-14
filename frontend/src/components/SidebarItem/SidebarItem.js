import style from "./_sidebarItem.module.css";
import { NavLink } from "react-router-dom";

export function SidebarItem({ texto, link, logo, isCollapsed, onClick }) {
  return (
    <NavLink
      to={link}
      className={({ isActive }) =>
        `${style.item} ${isCollapsed ? style.collapsed : ""} ${isActive ? style.active : ""}`
      }
      onClick={onClick}
      title={isCollapsed ? texto : undefined}
      aria-label={isCollapsed ? texto : undefined}
    >
      <span className={style.icon}>{logo}</span>
      <span className={style.label}>{texto}</span>
    </NavLink>
  );
}
