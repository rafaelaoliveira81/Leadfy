import { AppLayout } from "../../layouts/AppLayout/AppLayout";
import { useToast } from "../../components/ui/Toast/Toast";
import { Pagination } from "../../components/ui/Pagination/Pagination";
import { OpportunityListHeader } from "../../features/opportunities/components/OpportunityListHeader";
import { OpportunityFilters } from "../../features/opportunities/components/OpportunityFilters";
import { OpportunityTable } from "../../features/opportunities/components/OpportunityTable";
import { OpportunityFormModal } from "../../features/opportunities/components/OpportunityFormModal";
import { OpportunityDeleteModal } from "../../features/opportunities/components/OpportunityDeleteModal";
import { useOpportunitiesPage } from "../../features/opportunities/hooks/useOpportunitiesPage";
import style from "./_opportunities.module.css";

export function Opportunities() {
  const addToast = useToast();
  const {
    items,
    isLoading,
    error,
    total,
    totalPages,
    leads,
    owners,
    products,
    search,
    setSearch,
    statusFilter,
    setStatusFilter,
    clearFilters,
    page,
    pageSize,
    setPage,
    setPageSize,
    modal,
    openCreateModal,
    openEditModal,
    closeModal,
    deleteModal,
    openDeleteModal,
    closeDeleteModal,
    createOpportunity,
    updateOpportunity,
    toggleOpportunityStatus,
    deleteOpportunity,
  } = useOpportunitiesPage();

  const handleToggleStatus = async (opportunity) => {
    try {
      await toggleOpportunityStatus(opportunity);
      addToast(
        opportunity.isActive
          ? "Oportunidade desativada com sucesso!"
          : "Oportunidade ativada com sucesso!",
        "success",
      );
    } catch {
      addToast("Erro ao alterar status da oportunidade.", "error");
    }
  };

  const handleDelete = async (opportunityId) => {
    try {
      await deleteOpportunity(opportunityId);
      addToast("Oportunidade excluída com sucesso!", "success");
    } catch {
      addToast("Erro ao excluir oportunidade.", "error");
    }
  };

  return (
    <AppLayout>
      <OpportunityListHeader />

      <div className={style.card}>
        <OpportunityFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewOpportunity={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <OpportunityTable
          items={items}
          isLoading={isLoading}
          onEdit={openEditModal}
          onToggleStatus={handleToggleStatus}
          onDelete={openDeleteModal}
          emptyMessage="Nenhuma oportunidade encontrada."
          emptyActionLabel="+ Nova Oportunidade"
          onEmptyAction={openCreateModal}
        />

        {!isLoading && total > 0 && (
          <Pagination
            page={page}
            pageSize={pageSize}
            total={total}
            totalPages={totalPages}
            onPageChange={setPage}
            onPageSizeChange={setPageSize}
          />
        )}
      </div>

      {modal.open && (
        <OpportunityFormModal
          open={modal.open}
          mode={modal.mode}
          opportunity={modal.opportunity}
          leads={leads}
          owners={owners}
          products={products}
          onClose={closeModal}
          onCreateSubmit={createOpportunity}
          onUpdateSubmit={updateOpportunity}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <OpportunityDeleteModal
          open={deleteModal.open}
          opportunity={deleteModal.opportunity}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}
    </AppLayout>
  );
}
