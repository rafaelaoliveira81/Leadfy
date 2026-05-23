import style from './_topbar.module.css';
import { MdLogout } from 'react-icons/md';
import { Link } from 'react-router-dom';

export function Topbar({children}) {
  return (
    <div>
        <div className={style['topbar-conteudo']}>
          <Link to="/login" className={style['botao-logout']}>
            <MdLogout />
          </Link>
        </div>
        <div className={style['pagina-conteudo']}>
            {children}
        </div>
    </div>

  )
}