import style from './_sidebarItem.module.css';
import { Link } from 'react-router-dom';

export function SidebarItem({texto, link, logo}) {
  return (
      <Link to={link} className={style['sidebar-item']}>
        {logo}
        <h3 className={style['texto-link']}>{texto}</h3>
      </Link>
  )
}