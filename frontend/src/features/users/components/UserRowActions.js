import { useState, useRef, useEffect } from 'react';
import { MdMoreVert } from 'react-icons/md';
import style from './_userRowActions.module.css';

/**
 * Menu de ações por linha da tabela de usuários.
 */
export function UserRowActions({ user, onEdit, onToggleStatus, onDelete, onChangePassword }) {
  const [open, setOpen] = useState(false);
  const menuRef = useRef(null);

  useEffect(() => {
    if (!open) return;
    function handleClickOutside(e) {
      if (menuRef.current && !menuRef.current.contains(e.target)) {
        setOpen(false);
      }
    }
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [open]);

  return (
    <div ref={menuRef} style={{ position: 'relative', display: 'inline-block' }}>
      <button
        className={style.actionsBtn}
        onClick={() => setOpen((prev) => !prev)}
        aria-label="Ações do usuário"
        aria-haspopup="true"
        aria-expanded={open}
      >
        <MdMoreVert />
      </button>
      {open && (
        <div className={style.menu} role="menu">
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => {
              setOpen(false);
              onEdit(user);
            }}
          >
            Editar
          </button>
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => {
              setOpen(false);
              onChangePassword(user);
            }}
          >
            Alterar Senha
          </button>
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => {
              setOpen(false);
              onToggleStatus(user);
            }}
          >
            {user.isActive ? 'Desativar' : 'Ativar'}
          </button>
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => {
              setOpen(false);
              onDelete(user);
            }}
          >
            Excluir
          </button>
        </div>
      )}
    </div>
  );
}
