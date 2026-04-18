import { useState } from 'react';
import { Modal } from '../../../components/ui/Modal/Modal';
import style from './_owerDeleteModal.module.css';

/**
 * Modal de confirmação para exclusão de ower.
 */
export function OwerDeleteModal({
  open,
  ower,
  onClose,
  onConfirm,
}) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleConfirm = async () => {
    if (!ower) return;
    setIsSubmitting(true);
    try {
      await onConfirm(ower.id);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Excluir Responsável"
      subtitle="Confirme a exclusão do responsável"
      confirmLabel="Excluir"
      cancelLabel="Cancelar"
      onConfirm={handleConfirm}
      isSubmitting={isSubmitting}
    >
      <div className={style.content}>
        <p className={style.message}>
          Tem certeza que deseja excluir o responsável{' '}
          <strong>{ower?.name}</strong>?
        </p>
        <p className={style.warning}>
          Esta ação não pode ser desfeita.
        </p>
      </div>
    </Modal>
  );
}
