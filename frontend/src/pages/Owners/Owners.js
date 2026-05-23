import { AppLayout } from "../../layouts/AppLayout/AppLayout";
import { useToast } from "../../components/ui/Toast/Toast";
import { Pagination } from "../../components/ui/Pagination/Pagination";
import { OwnerListHeader } from "../../features/owners/components/OwnerListHeader";
import { OwnerFilters } from "../../features/owners/components/OwnerFilters";
import { OwnerTable } from "../../features/owners/components/OwnerTable";
import { OwnerFormModal } from "../../features/owners/components/OwnerFormModal";
import { OwnerDeleteModal } from "../../features/owners/components/OwnerDeleteModal";
import { useOwnersPage } from "../../features/owners/hooks/useOwnersPage";
import style from "./_owners.module.css";

export function Owners() {
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
    createOwner,
    updateOwner,
    toggleOwnerStatus,
    deleteOwner,
  } = useOwnersPage();

  const handleToggleStatus = async (owner) => {
    try {
      await toggleOwnerStatus(owner);
      addToast(
        owner.isActive
          ? "Responsável desativado com sucesso!"
          : "Responsável ativado com sucesso!",
        "success",
      );
    } catch {
      addToast("Erro ao alterar status do responsável.", "error");
    }
  };

  const handleDelete = async (ownerId) => {
    try {
      await deleteOwner(ownerId);
      addToast("Responsável excluído com sucesso!", "success");
    } catch {
      addToast("Erro ao excluir responsável.", "error");
    }
  };

  return (
    <AppLayout>
      <OwnerListHeader />

      <div className={style.card}>
        <OwnerFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewOwner={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <OwnerTable
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
        <OwnerFormModal
          open={modal.open}
          mode={modal.mode}
          owner={modal.owner}
          users={users}
          onClose={closeModal}
          onCreateSubmit={createOwner}
          onUpdateSubmit={updateOwner}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <OwnerDeleteModal
          open={deleteModal.open}
          owner={deleteModal.owner}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}
    </AppLayout>
  );
}
