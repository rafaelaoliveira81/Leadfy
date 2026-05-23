import { useState } from "react";
import { AppLayout } from "../../../layouts/AppLayout/AppLayout";
import { useToast } from "../../../components/ui/Toast/Toast";
import { KanbanBoard } from "../../../features/opportunities/kanban/components/KanbanBoard";
import { KanbanListHeader } from "../../../features/opportunities/kanban/components/KanbanListHeader";
import { KanbanModal } from "../../../features/opportunities/kanban/components/KanbanModal";
import { OpportunityFormModal } from "../../../features/opportunities/components/OpportunityFormModal";
import { OpportunityDeleteModal } from "../../../features/opportunities/components/OpportunityDeleteModal";
import { useOpportunitiesKanbanPage } from "../../../features/opportunities/kanban/hooks/useOpportunitiesKanbanPage";
import style from "../../../features/opportunities/kanban/components/_kanban.module.css";

export function OpportunitiesKanban() {
  const addToast = useToast();
  const [deleteModal, setDeleteModal] = useState({
    open: false,
    opportunity: null,
  });

  const {
    columns,
    isLoading,
    error,
    activeCard,
    handleDragStart,
    handleDragOver,
    handleDragEnd,
    detailModal,
    openDetailModal,
    closeDetailModal,
    handleStageChange,
    createModal,
    openCreateModal,
    closeCreateModal,
    createOpportunity,
    deleteOpportunity,
    handleActionPlanGenerated,
    leads,
    owners,
    products,
    refresh,
  } = useOpportunitiesKanbanPage();

  const onStageChange = async (opportunityId, newStage) => {
    try {
      await handleStageChange(opportunityId, newStage);
      addToast("Etapa atualizado com sucesso!", "success");
    } catch {
      addToast("Erro ao alterar etapa da oportunidade.", "error");
    }
  };

  const onCreateSubmit = async (data) => {
    await createOpportunity(data);
  };

  const onDeleteRequest = (opportunity) => {
    closeDetailModal();
    setDeleteModal({ open: true, opportunity });
  };

  const onDeleteConfirm = async (id) => {
    try {
      await deleteOpportunity(id);
      setDeleteModal({ open: false, opportunity: null });
      addToast("Oportunidade excluída com sucesso!", "success");
    } catch {
      addToast("Erro ao excluir oportunidade.", "error");
    }
  };

  return (
    <AppLayout>
      <KanbanListHeader />
      <div className={style.boardWrapper}>
        <KanbanBoard
          columns={columns}
          isLoading={isLoading}
          error={error}
          activeCard={activeCard}
          onDragStart={handleDragStart}
          onDragOver={handleDragOver}
          onDragEnd={handleDragEnd}
          onCardClick={openDetailModal}
          onAddClick={openCreateModal}
        />
      </div>

      <KanbanModal
        open={detailModal.open}
        opportunity={detailModal.opportunity}
        onClose={closeDetailModal}
        onStageChange={onStageChange}
        onAfterInteraction={refresh}
        onDeleteRequest={onDeleteRequest}
        onActionPlanGenerated={(result) => {
          handleActionPlanGenerated(result);
          addToast("Plano de ação gerado com sucesso!", "success");
        }}
      />

      {createModal.open && (
        <OpportunityFormModal
          open={createModal.open}
          mode="create"
          opportunity={{ stage: createModal.defaultStage }}
          leads={leads}
          owners={owners}
          products={products}
          onClose={closeCreateModal}
          onCreateSubmit={onCreateSubmit}
          onUpdateSubmit={() => {}}
          addToast={addToast}
        />
      )}

      <OpportunityDeleteModal
        open={deleteModal.open}
        opportunity={deleteModal.opportunity}
        onClose={() => setDeleteModal({ open: false, opportunity: null })}
        onConfirm={onDeleteConfirm}
      />
    </AppLayout>
  );
}
