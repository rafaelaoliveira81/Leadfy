import { AppLayout } from '../../layouts/AppLayout/AppLayout';
import { useToast } from '../../components/ui/Toast/Toast';
import { Pagination } from '../../components/ui/Pagination/Pagination';
import { LeadListHeader } from '../../features/leads/components/LeadListHeader';
import { LeadFilters } from '../../features/leads/components/LeadFilters';
import { LeadTable } from '../../features/leads/components/LeadTable';
import { LeadFormModal } from '../../features/leads/components/LeadFormModal';
import { LeadDeleteModal } from '../../features/leads/components/LeadDeleteModal';
import { useLeadsPage } from '../../features/leads/hooks/useLeadsPage';
import style from './_leads.module.css';

export function Leads() {
  const addToast = useToast();
  const {
    items,
    isLoading,
    error,
    total,
    totalPages,
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
    createLead,
    updateLead,
    toggleLeadStatus,
    deleteLead,
  } = useLeadsPage();

  const handleToggleStatus = async (lead) => {
    try {
      await toggleLeadStatus(lead);
      addToast(
        lead.isActive
          ? 'Lead desativado com sucesso!'
          : 'Lead ativado com sucesso!',
        'success'
      );
    } catch {
      addToast('Erro ao alterar status do lead.', 'error');
    }
  };

  const handleDelete = async (leadId) => {
    try {
      await deleteLead(leadId);
      addToast('Lead excluído com sucesso!', 'success');
    } catch {
      addToast('Erro ao excluir lead.', 'error');
    }
  };

  return (
    <AppLayout>
      <LeadListHeader />

      <div className={style.card}>
        <LeadFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewLead={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <LeadTable
          items={items}
          isLoading={isLoading}
          onEdit={openEditModal}
          onToggleStatus={handleToggleStatus}
          onDelete={openDeleteModal}
          emptyMessage="Nenhum lead encontrado."
          emptyActionLabel="+ Novo Lead"
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
        <LeadFormModal
          open={modal.open}
          mode={modal.mode}
          lead={modal.lead}
          onClose={closeModal}
          onCreateSubmit={createLead}
          onUpdateSubmit={updateLead}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <LeadDeleteModal
          open={deleteModal.open}
          lead={deleteModal.lead}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}
    </AppLayout>
  );
}
