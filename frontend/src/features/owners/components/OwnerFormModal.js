import { Modal } from "../../../components/ui/Modal/Modal";
import { OwnerForm } from "./OwnerForm";
import { useOwnerForm } from "../hooks/useOwnerForm";

/**
 * Modal de criação/edição de owner.
 */
export function OwnerFormModal({
  open,
  mode,
  owner,
  users,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === "edit";

  const { form, handleSubmit, isSubmitting } = useOwnerForm({
    mode,
    owner,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast("Responsável atualizado com sucesso!", "success");
          } catch {
            addToast("Erro ao atualizar responsável.", "error");
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast("Responsável criado com sucesso!", "success");
          } catch {
            addToast("Erro ao criar responsável.", "error");
          }
        },
  });

  const title = isEdit ? "Editar Responsável" : "Novo Responsável";
  const subtitle = isEdit
    ? "Edite as informações do responsável selecionado."
    : "Insira as informações básicas para criar um novo responsável.";

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
      <OwnerForm form={form} users={users} />
    </Modal>
  );
}
