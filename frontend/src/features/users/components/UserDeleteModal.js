import { useState } from 'react';
import { Modal } from '../../../components/ui/Modal/Modal';
import style from './_userDeleteModal.module.css';

/**
 * Modal de confirmação para exclusão de usuário.
 */
export function UserDeleteModal({
  open,
  user,
  onClose,
  onConfirm,
}) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleConfirm = async () => {
    if (!user) return;
    setIsSubmitting(true);
    try {
      await onConfirm(user.id);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Excluir Usuário"
      subtitle="Confirme a exclusão do usuário"
      confirmLabel="Excluir"
      cancelLabel="Cancelar"
      onConfirm={handleConfirm}
      isSubmitting={isSubmitting}
    >
      <div className={style.content}>
        <p className={style.message}>
          Tem certeza que deseja excluir o usuário{' '}
          <strong>{user?.name}</strong>?
        </p>
        <p className={style.warning}>
          Esta ação não pode ser desfeita.
        </p>
      </div>
    </Modal>
  );
}
