import { Modal } from "../../../components/ui/Modal/Modal";
import { OpportunityForm } from "./OpportunityForm";
import { useOpportunityForm } from "../hooks/useOpportunityForm";

/**
 * Modal de criação/edição de oportunidade.
 */
export function OpportunityFormModal({
  open,
  mode,
  opportunity,
  leads,
  owners,
  products,
  onClose,
  onCreateSubmit,
  onUpdateSubmit,
  addToast,
}) {
  const isEdit = mode === "edit";

  const { form, handleSubmit, isSubmitting } = useOpportunityForm({
    mode,
    opportunity,
    onSubmit: isEdit
      ? async (id, data) => {
          try {
            await onUpdateSubmit(id, data);
            addToast("Oportunidade atualizada com sucesso!", "success");
          } catch {
            addToast("Erro ao atualizar oportunidade.", "error");
          }
        }
      : async (data) => {
          try {
            await onCreateSubmit(data);
            addToast("Oportunidade criada com sucesso!", "success");
          } catch {
            addToast("Erro ao criar oportunidade.", "error");
          }
        },
  });

  const title = isEdit ? "Editar Oportunidade" : "Nova Oportunidade";
  const subtitle = isEdit
    ? "Edite as informações da oportunidade selecionada."
    : "Insira as informações para criar uma nova oportunidade.";

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={title}
      subtitle={subtitle}
      confirmLabel="Salvar Oportunidade"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <OpportunityForm
        form={form}
        leads={leads}
        owners={owners}
        products={products}
      />
    </Modal>
  );
}
