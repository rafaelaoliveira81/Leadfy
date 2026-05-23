import { Modal } from '../../../components/ui/Modal/Modal';
import { ProductForm } from './ProductForm';
import { useProductForm } from '../hooks/useProductForm';

/**
 * Modal de criação/edição de produto.
 * @param {object} props
 * @param {boolean} props.open
 * @param {'create'|'edit'} props.mode
 * @param {object|null} props.product
 * @param {() => void} props.onClose
 * @param {(data: object) => Promise<void>} props.onCreateSubmit
 * @param {(id: number, data: object) => Promise<void>} props.onUpdateSubmit
 * @param {(message: string, type: string) => void} props.addToast
 */
export function ProductFormModal({
  open,
  mode,
  product,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === 'edit';

  const { form, handleSubmit, isSubmitting } = useProductForm({
    mode,
    product,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast('Produto atualizado com sucesso!', 'success');
          } catch {
            addToast('Erro ao atualizar produto.', 'error');
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast('Produto criado com sucesso!', 'success');
          } catch {
            addToast('Erro ao criar produto.', 'error');
          }
        },
  });

  const title = isEdit ? 'Editar Produto' : 'Novo Produto';
  const subtitle = isEdit
    ? 'Edite as informações do produto selecionado.'
    : 'Insira as informações básicas para criar um novo produto.';

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={title}
      subtitle={subtitle}
      confirmLabel="Salvar Produto"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <ProductForm form={form} />
    </Modal>
  );
}
