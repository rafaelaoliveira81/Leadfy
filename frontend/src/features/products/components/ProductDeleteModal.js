import { useState } from 'react';
import { Modal } from '../../../components/ui/Modal/Modal';
import style from './_productDeleteModal.module.css';

/**
 * Modal de confirmação para exclusão de produto.
 * @param {object} props
 * @param {boolean} props.open
 * @param {object|null} props.product
 * @param {() => void} props.onClose
 * @param {(productId: number) => Promise<void>} props.onConfirm
 */
export function ProductDeleteModal({
  open,
  product,
  onClose,
  onConfirm,
}) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleConfirm = async () => {
    if (!product) return;
    setIsSubmitting(true);
    try {
      await onConfirm(product.id);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Excluir Produto"
      subtitle="Confirme a exclusão do produto"
      confirmLabel="Excluir"
      cancelLabel="Cancelar"
      onConfirm={handleConfirm}
      isSubmitting={isSubmitting}
    >
      <div className={style.content}>
        <p className={style.message}>
          Tem certeza que deseja excluir o produto{' '}
          <strong>{product?.name}</strong>?
        </p>
        <p className={style.warning}>
          Esta ação não pode ser desfeita.
        </p>
      </div>
    </Modal>
  );
}
