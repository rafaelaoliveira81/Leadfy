import { Modal } from '../../../components/ui/Modal/Modal';
import { OwerForm } from './OwerForm';
import { useOwerForm } from '../hooks/useOwerForm';

/**
 * Modal de criação/edição de ower.
 */
export function OwerFormModal({
  open,
  mode,
  ower,
  users,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === 'edit';

  const { form, handleSubmit, isSubmitting } = useOwerForm({
    mode,
    ower,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast('Responsável atualizado com sucesso!', 'success');
          } catch {
            addToast('Erro ao atualizar responsável.', 'error');
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast('Responsável criado com sucesso!', 'success');
          } catch {
            addToast('Erro ao criar responsável.', 'error');
          }
        },
  });

  const title = isEdit ? 'Editar Responsável' : 'Novo Responsável';
  const subtitle = isEdit
    ? 'Edite as informações do responsável selecionado.'
    : 'Insira as informações básicas para criar um novo responsável.';

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={title}
      subtitle={subtitle}
      confirmLabel="Salvar Responsável"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <OwerForm form={form} users={users} />
    </Modal>
  );
}
