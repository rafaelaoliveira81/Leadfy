import { AppLayout } from '../../layouts/AppLayout/AppLayout';
import { useToast } from '../../components/ui/Toast/Toast';
import { Pagination } from '../../components/ui/Pagination/Pagination';
import { AiConfigListHeader } from '../../features/aiConfig/components/AiConfigListHeader';
import { AiConfigFilters } from '../../features/aiConfig/components/AiConfigFilters';
import { AiConfigTable } from '../../features/aiConfig/components/AiConfigTable';
import { AiConfigFormModal } from '../../features/aiConfig/components/AiConfigFormModal';
import { AiConfigDeleteModal } from '../../features/aiConfig/components/AiConfigDeleteModal';
import { useAiConfigsPage } from '../../features/aiConfig/hooks/useAiConfigsPage';
import style from './_aiConfig.module.css';

export function AiConfig() {
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
    createConfig,
    updateConfig,
    toggleConfigStatus,
    deleteConfig,
  } = useAiConfigsPage();

  const handleToggleStatus = async (config) => {
    try {
      await toggleConfigStatus(config);
      addToast(
        config.isActive
          ? 'Configuração desativada com sucesso!'
          : 'Configuração ativada com sucesso!',
        'success'
      );
    } catch {
      addToast('Erro ao alterar status da configuração.', 'error');
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteConfig(id);
      addToast('Configuração excluída com sucesso!', 'success');
    } catch {
      addToast('Erro ao excluir configuração.', 'error');
    }
  };

  return (
    <AppLayout>
      <AiConfigListHeader />

      <div className={style.card}>
        <AiConfigFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewConfig={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <AiConfigTable
          items={items}
          isLoading={isLoading}
          onEdit={openEditModal}
          onToggleStatus={handleToggleStatus}
          onDelete={openDeleteModal}
          emptyMessage="Nenhuma configuração de IA encontrada."
          emptyActionLabel="+ Nova configuração"
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
        <AiConfigFormModal
          open={modal.open}
          mode={modal.mode}
          config={modal.config}
          onClose={closeModal}
          onCreateSubmit={createConfig}
          onUpdateSubmit={updateConfig}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <AiConfigDeleteModal
          open={deleteModal.open}
          config={deleteModal.config}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}
    </AppLayout>
  );
}
