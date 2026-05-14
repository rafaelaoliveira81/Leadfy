import { useState } from "react";
import { Modal } from "../../../components/ui/Modal/Modal";
import style from "./_opportunityDeleteModal.module.css";

/**
 * Modal de confirmação para exclusão de oportunidade.
 */
export function OpportunityDeleteModal({
  open,
  opportunity,
  onClose,
  onConfirm,
}) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleConfirm = async () => {
    if (!opportunity) return;
    setIsSubmitting(true);
    try {
      await onConfirm(opportunity.id);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Excluir Oportunidade"
      subtitle="Confirme a exclusão da oportunidade"
      confirmLabel="Excluir"
      cancelLabel="Cancelar"
      onConfirm={handleConfirm}
      isSubmitting={isSubmitting}
    >
      <div className={style.content}>
        <p className={style.message}>
          Tem certeza que deseja excluir a oportunidade selecionada?
        </p>
        <p className={style.warning}>Esta ação não pode ser desfeita.</p>
      </div>
    </Modal>
  );
}
