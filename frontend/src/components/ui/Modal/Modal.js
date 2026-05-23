import { useEffect, useCallback } from 'react';
import { createPortal } from 'react-dom';
import { MdClose } from 'react-icons/md';
import style from './_modal.module.css';
import { Button } from '../Button/Button';

export function Modal({
  open,
  onClose,
  title,
  subtitle,
  children,
  footer,
  confirmLabel = 'Salvar',
  cancelLabel = 'Cancelar',
  onConfirm,
  isSubmitting = false,
}) {
  const handleKeyDown = useCallback(
    (e) => {
      if (e.key === 'Escape') onClose();
    },
    [onClose]
  );

  useEffect(() => {
    if (!open) return;
    document.addEventListener('keydown', handleKeyDown);
    document.body.style.overflow = 'hidden';
    return () => {
      document.removeEventListener('keydown', handleKeyDown);
      document.body.style.overflow = '';
    };
  }, [open, handleKeyDown]);

  if (!open) return null;

  const handleOverlayClick = (e) => {
    if (e.target === e.currentTarget) onClose();
  };

  return createPortal(
    <div
      className={style.overlay}
      onClick={handleOverlayClick}
      role="dialog"
      aria-modal="true"
      aria-label={title}
    >
      <div className={style.modal}>
        <div className={style.header}>
          <div className={style.headerText}>
            <h2>{title}</h2>
            {subtitle && <p>{subtitle}</p>}
          </div>
          <button
            className={style.closeBtn}
            onClick={onClose}
            aria-label="Fechar"
          >
            <MdClose />
          </button>
        </div>
        <div className={style.body}>{children}</div>
        <div className={style.footer}>
          {footer ?? (
            <>
              <Button variant="secondary" onClick={onClose} disabled={isSubmitting}>
                {cancelLabel}
              </Button>
              <Button onClick={onConfirm} disabled={isSubmitting}>
                {isSubmitting ? 'Salvando...' : confirmLabel}
              </Button>
            </>
          )}
        </div>
      </div>
    </div>,
    document.body
  );
}
