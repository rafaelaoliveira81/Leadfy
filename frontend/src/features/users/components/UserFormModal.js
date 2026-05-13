import { Modal } from '../../../components/ui/Modal/Modal';
import { UserForm } from './UserForm';
import { useUserForm } from '../hooks/useUserForm';

/**
 * Modal de criação/edição de usuário.
 */
export function UserFormModal({
  open,
  mode,
  user,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === 'edit';

  const { form, handleSubmit, isSubmitting } = useUserForm({
    mode,
    user,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast('Usuário atualizado com sucesso!', 'success');
          } catch {
            addToast('Erro ao atualizar usuário.', 'error');
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast('Usuário criado com sucesso!', 'success');
          } catch {
            addToast('Erro ao criar usuário.', 'error');
          }
        },
  });

  const title = isEdit ? 'Editar Usuário' : 'Novo Usuário';
  const subtitle = isEdit
    ? 'Edite as informações do usuário selecionado.'
    : 'Insira as informações básicas para criar um novo usuário.';

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={title}
      subtitle={subtitle}
      confirmLabel="Salvar Usuário"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <UserForm form={form} isEdit={isEdit} />
    </Modal>
  );
}
