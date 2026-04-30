import { useState, useRef, useEffect } from 'react';
import { MdMoreVert } from 'react-icons/md';
import style from './_aiConfigRowActions.module.css';

export function AiConfigRowActions({ config, onEdit, onToggleStatus, onDelete }) {
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
        aria-label="Ações da configuração"
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
            onClick={() => { setOpen(false); onEdit(config); }}
          >
            Editar
          </button>
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => { setOpen(false); onToggleStatus(config); }}
          >
            {config.isActive ? 'Desativar' : 'Ativar'}
          </button>
          <button
            className={`${style.menuItem} ${style.menuItemDanger}`}
            role="menuitem"
            onClick={() => { setOpen(false); onDelete(config); }}
          >
            Excluir
          </button>
        </div>
      )}
    </div>
  );
}
