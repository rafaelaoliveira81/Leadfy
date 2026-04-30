import { Modal } from '../../../components/ui/Modal/Modal';
import { AiConfigForm } from './AiConfigForm';
import { useAiConfigForm } from '../hooks/useAiConfigForm';

export function AiConfigFormModal({
  open,
  mode,
  config,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === 'edit';

  const { form, handleSubmit, isSubmitting } = useAiConfigForm({
    mode,
    config,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast('Configuração atualizada com sucesso!', 'success');
          } catch {
            addToast('Erro ao atualizar configuração.', 'error');
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast('Configuração criada com sucesso!', 'success');
          } catch {
            addToast('Erro ao criar configuração.', 'error');
          }
        },
  });

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={isEdit ? 'Editar Configuração de IA' : 'Nova Configuração de IA'}
      subtitle={
        isEdit
          ? 'Edite o modelo, template e, opcionalmente, a chave de API.'
          : 'Defina o modelo, o template do prompt e a chave de API.'
      }
      confirmLabel="Salvar"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <AiConfigForm form={form} isEdit={isEdit} />
    </Modal>
  );
}
