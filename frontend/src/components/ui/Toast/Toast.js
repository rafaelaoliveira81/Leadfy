import { createContext, useContext, useState, useCallback } from 'react';
import { createPortal } from 'react-dom';
import { MdClose } from 'react-icons/md';
import style from './_toast.module.css';

const ToastContext = createContext(null);

let toastId = 0;

export function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([]);

  const removeToast = useCallback((id) => {
    setToasts((prev) => prev.filter((t) => t.id !== id));
  }, []);

  const addToast = useCallback(
    (message, type = 'success', duration = 4000) => {
      const id = ++toastId;
      setToasts((prev) => [...prev, { id, message, type }]);
      if (duration > 0) {
        setTimeout(() => removeToast(id), duration);
      }
    },
    [removeToast]
  );

  return (
    <ToastContext.Provider value={addToast}>
      {children}
      {createPortal(
        <div className={style.toastContainer} aria-live="polite">
          {toasts.map((t) => (
            <div key={t.id} className={`${style.toast} ${style[t.type]}`} role="alert">
              <span>{t.message}</span>
              <button
                className={style.closeBtn}
                onClick={() => removeToast(t.id)}
                aria-label="Fechar notificação"
              >
                <MdClose />
              </button>
            </div>
          ))}
        </div>,
        document.body
      )}
    </ToastContext.Provider>
  );
}

/**
 * Hook para disparar toasts.
 * @returns {(message: string, type?: 'success'|'error'|'info', duration?: number) => void}
 */
export function useToast() {
  const context = useContext(ToastContext);
  if (!context) {
    throw new Error('useToast deve ser usado dentro de um ToastProvider');
  }
  return context;
}
