import style from './_sidebarItem.module.css';
import { Link } from 'react-router-dom';

export function SidebarItem({ texto, link, logo, isCollapsed, onClick }) {
  return (
    <Link
      to={link}
      className={`${style.item} ${isCollapsed ? style.collapsed : ''}`}
      onClick={onClick}
      title={isCollapsed ? texto : undefined}
      aria-label={isCollapsed ? texto : undefined}
    >
      <span className={style.icon}>{logo}</span>
      <span className={style.label}>{texto}</span>
    </Link>
  );
}