import { useState, useRef, useEffect } from 'react';
import { MdMoreVert } from 'react-icons/md';
import style from './_productRowActions.module.css';

/**
 * Menu de ações por linha da tabela de produtos.
 * @param {object} props
 * @param {object} props.product
 * @param {(product: object) => void} props.onEdit
 * @param {(product: object) => void} props.onToggleStatus
 * @param {(product: object) => void} props.onDelete
 */
export function ProductRowActions({ product, onEdit, onToggleStatus, onDelete }) {
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
        aria-label="Ações do produto"
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
              onEdit(product);
            }}
          >
            Editar
          </button>
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => {
              setOpen(false);
              onToggleStatus(product);
            }}
          >
            {product.isActive ? 'Desativar' : 'Ativar'}
          </button>
          <button
            className={style.menuItem}
            role="menuitem"
            onClick={() => {
              setOpen(false);
              onDelete(product);
            }}
          >
            Excluir
          </button>
        </div>
      )}
    </div>
  );
}
