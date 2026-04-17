import { useState, useRef, useEffect } from 'react';
import style from './_topbar.module.css';
import { MdSearch, MdPerson, MdMenu } from 'react-icons/md';
import { useNavigate } from 'react-router-dom';

export function Topbar({ onMenuToggle }) {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const dropdownRef = useRef(null);
  const navigate = useNavigate();

  useEffect(() => {
    function handleClickOutside(e) {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
        setIsDropdownOpen(false);
      }
    }
    function handleEscape(e) {
      if (e.key === 'Escape') setIsDropdownOpen(false);
    }
    document.addEventListener('mousedown', handleClickOutside);
    document.addEventListener('keydown', handleEscape);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      document.removeEventListener('keydown', handleEscape);
    };
  }, []);

  return (
    <header className={style.topbar}>
      <button
        className={style.hamburger}
        onClick={onMenuToggle}
        aria-label="Abrir menu"
      >
        <MdMenu />
      </button>

      <div className={style.searchWrapper}>
        <MdSearch className={style.searchIcon} />
        <input
          type="text"
          className={style.searchInput}
          placeholder="Pesquisar..."
          aria-label="Pesquisar"
        />
      </div>

      <div className={style.userMenu} ref={dropdownRef}>
        <button
          className={style.userBtn}
          onClick={() => setIsDropdownOpen((prev) => !prev)}
          aria-label="Menu do usuário"
          aria-expanded={isDropdownOpen}
        >
          <MdPerson />
        </button>
        {isDropdownOpen && (
          <ul className={style.dropdown} role="menu">
            <li role="menuitem">
              <button
                onClick={() => {
                  setIsDropdownOpen(false);
                  navigate('/profile');
                }}
              >
                Perfil
              </button>
            </li>
            <li role="menuitem">
              <button
                onClick={() => {
                  setIsDropdownOpen(false);
                  navigate('/login');
                }}
              >
                Sair
              </button>
            </li>
          </ul>
        )}
      </div>
    </header>
  );
}
