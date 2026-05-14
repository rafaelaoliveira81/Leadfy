import { AppLayout } from "../../../layouts/AppLayout/AppLayout";
import { useToast } from "../../../components/ui/Toast/Toast";
import { KanbanBoard } from "../../../features/opportunities/kanban/components/KanbanBoard";
import { KanbanModal } from "../../../features/opportunities/kanban/components/KanbanModal";
import { OpportunityFormModal } from "../../../features/opportunities/components/OpportunityFormModal";
import { useOpportunitiesKanbanPage } from "../../../features/opportunities/kanban/hooks/useOpportunitiesKanbanPage";
import style from "../../../features/opportunities/kanban/components/_kanban.module.css";

export function OpportunitiesKanban() {
  const addToast = useToast();
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
    try {
      await createOpportunity(data);
      addToast("Oportunidade criada com sucesso!", "success");
    } catch {
      addToast("Erro ao criar oportunidade.", "error");
    }
  };

  return (
    <AppLayout>
      <div className={style.boardWrapper}>
        <div className={style.boardHeader}>
          <div>
            <h1 className={style.boardTitle}>Kanban de Oportunidades</h1>
            {/* <p className={style.boardSubtitle}>
              Gerencie o funil de vendas arrastando os cards entre as colunas.
            </p> */}
          </div>
        </div>

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
    </AppLayout>
  );
}
