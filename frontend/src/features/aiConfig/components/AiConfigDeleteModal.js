import { useState } from 'react';
import { Modal } from '../../../components/ui/Modal/Modal';
import style from './_aiConfigDeleteModal.module.css';

export function AiConfigDeleteModal({ open, config, onClose, onConfirm }) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleConfirm = async () => {
    if (!config) return;
    setIsSubmitting(true);
    try {
      await onConfirm(config.id);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Excluir Configuração de IA"
      subtitle="Confirme a exclusão da configuração"
      confirmLabel="Excluir"
      cancelLabel="Cancelar"
      onConfirm={handleConfirm}
      isSubmitting={isSubmitting}
    >
      <div className={style.content}>
        <p className={style.message}>
          Tem certeza que deseja excluir a configuração do modelo{' '}
          <strong>{config?.modelName}</strong>?
        </p>
        <p className={style.warning}>Esta ação não pode ser desfeita.</p>
      </div>
    </Modal>
  );
}
