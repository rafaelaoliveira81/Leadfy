import { AppLayout } from '../../layouts/AppLayout/AppLayout';
import { useToast } from '../../components/ui/Toast/Toast';
import { Pagination } from '../../components/ui/Pagination/Pagination';
import { OwerListHeader } from '../../features/owers/components/OwerListHeader';
import { OwerFilters } from '../../features/owers/components/OwerFilters';
import { OwerTable } from '../../features/owers/components/OwerTable';
import { OwerFormModal } from '../../features/owers/components/OwerFormModal';
import { OwerDeleteModal } from '../../features/owers/components/OwerDeleteModal';
import { useOwersPage } from '../../features/owers/hooks/useOwersPage';
import style from './_owers.module.css';

export function Owers() {
  const addToast = useToast();
  const {
    items,
    isLoading,
    error,
    total,
    totalPages,
    users,
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
    createOwer,
    updateOwer,
    toggleOwerStatus,
    deleteOwer,
  } = useOwersPage();

  const handleToggleStatus = async (ower) => {
    try {
      await toggleOwerStatus(ower);
      addToast(
        ower.isActive
          ? 'Responsável desativado com sucesso!'
          : 'Responsável ativado com sucesso!',
        'success'
      );
    } catch {
      addToast('Erro ao alterar status do responsável.', 'error');
    }
  };

  const handleDelete = async (owerId) => {
    try {
      await deleteOwer(owerId);
      addToast('Responsável excluído com sucesso!', 'success');
    } catch {
      addToast('Erro ao excluir responsável.', 'error');
    }
  };

  return (
    <AppLayout>
      <OwerListHeader />

      <div className={style.card}>
        <OwerFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewOwer={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <OwerTable
          items={items}
          isLoading={isLoading}
          onEdit={openEditModal}
          onToggleStatus={handleToggleStatus}
          onDelete={openDeleteModal}
          emptyMessage="Nenhum responsável encontrado."
          emptyActionLabel="+ Novo Responsável"
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
        <OwerFormModal
          open={modal.open}
          mode={modal.mode}
          ower={modal.ower}
          users={users}
          onClose={closeModal}
          onCreateSubmit={createOwer}
          onUpdateSubmit={updateOwer}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <OwerDeleteModal
          open={deleteModal.open}
          ower={deleteModal.ower}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}
    </AppLayout>
  );
}
