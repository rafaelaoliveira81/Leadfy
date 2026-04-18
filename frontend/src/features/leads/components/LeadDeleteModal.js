import { useState } from 'react';
import { Modal } from '../../../components/ui/Modal/Modal';
import style from './_leadDeleteModal.module.css';

/**
 * Modal de confirmação para exclusão de lead.
 * @param {object} props
 * @param {boolean} props.open
 * @param {object|null} props.lead
 * @param {() => void} props.onClose
 * @param {(leadId: number) => Promise<void>} props.onConfirm
 */
export function LeadDeleteModal({
  open,
  lead,
  onClose,
  onConfirm,
}) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleConfirm = async () => {
    if (!lead) return;
    setIsSubmitting(true);
    try {
      await onConfirm(lead.id);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Excluir Lead"
      subtitle="Confirme a exclusão do lead"
      confirmLabel="Excluir"
      cancelLabel="Cancelar"
      onConfirm={handleConfirm}
      isSubmitting={isSubmitting}
    >
      <div className={style.content}>
        <p className={style.message}>
          Tem certeza que deseja excluir o lead{' '}
          <strong>{lead?.name}</strong>?
        </p>
        <p className={style.warning}>
          Esta ação não pode ser desfeita.
        </p>
      </div>
    </Modal>
  );
}
