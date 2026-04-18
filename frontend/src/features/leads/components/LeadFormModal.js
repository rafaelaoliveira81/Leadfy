import { Modal } from '../../../components/ui/Modal/Modal';
import { LeadForm } from './LeadForm';
import { useLeadForm } from '../hooks/useLeadForm';

/**
 * Modal de criação/edição de lead.
 * @param {object} props
 * @param {boolean} props.open
 * @param {'create'|'edit'} props.mode
 * @param {object|null} props.lead
 * @param {() => void} props.onClose
 * @param {(data: object) => Promise<void>} props.onCreateSubmit
 * @param {(id: number, data: object) => Promise<void>} props.onUpdateSubmit
 * @param {(message: string, type: string) => void} props.addToast
 */
export function LeadFormModal({
  open,
  mode,
  lead,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === 'edit';

  const { form, handleSubmit, isSubmitting } = useLeadForm({
    mode,
    lead,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast('Lead atualizado com sucesso!', 'success');
          } catch {
            addToast('Erro ao atualizar lead.', 'error');
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast('Lead criado com sucesso!', 'success');
          } catch {
            addToast('Erro ao criar lead.', 'error');
          }
        },
  });

  const title = isEdit ? 'Editar Lead' : 'Novo Lead';
  const subtitle = isEdit
    ? 'Edite as informações do lead selecionado.'
    : 'Insira as informações básicas para criar um novo lead.';

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={title}
      subtitle={subtitle}
      confirmLabel="Salvar Lead"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <LeadForm form={form} />
    </Modal>
  );
}
