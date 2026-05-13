import { AppLayout } from '../../layouts/AppLayout/AppLayout';
import { useToast } from '../../components/ui/Toast/Toast';
import { Pagination } from '../../components/ui/Pagination/Pagination';
import { UserListHeader } from '../../features/users/components/UserListHeader';
import { UserFilters } from '../../features/users/components/UserFilters';
import { UserTable } from '../../features/users/components/UserTable';
import { UserFormModal } from '../../features/users/components/UserFormModal';
import { UserDeleteModal } from '../../features/users/components/UserDeleteModal';
import { UserPasswordModal } from '../../features/users/components/UserPasswordModal';
import { useUsersPage } from '../../features/users/hooks/useUsersPage';
import style from './_users.module.css';

export function Users() {
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
    passwordModal,
    openPasswordModal,
    closePasswordModal,
    createUser,
    updateUser,
    toggleUserStatus,
    deleteUser,
    updatePassword,
  } = useUsersPage();

  const handleToggleStatus = async (user) => {
    try {
      await toggleUserStatus(user);
      addToast(
        user.isActive
          ? 'Usuário desativado com sucesso!'
          : 'Usuário ativado com sucesso!',
        'success'
      );
    } catch {
      addToast('Erro ao alterar status do usuário.', 'error');
    }
  };

  const handleDelete = async (userId) => {
    try {
      await deleteUser(userId);
      addToast('Usuário excluído com sucesso!', 'success');
    } catch {
      addToast('Erro ao excluir usuário.', 'error');
    }
  };

  return (
    <AppLayout>
      <UserListHeader />

      <div className={style.card}>
        <UserFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewUser={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <UserTable
          items={items}
          isLoading={isLoading}
          onEdit={openEditModal}
          onToggleStatus={handleToggleStatus}
          onDelete={openDeleteModal}
          onChangePassword={openPasswordModal}
          emptyMessage="Nenhum usuário encontrado."
          emptyActionLabel="+ Novo Usuário"
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
        <UserFormModal
          open={modal.open}
          mode={modal.mode}
          user={modal.user}
          onClose={closeModal}
          onCreateSubmit={createUser}
          onUpdateSubmit={updateUser}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <UserDeleteModal
          open={deleteModal.open}
          user={deleteModal.user}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}

      {passwordModal.open && (
        <UserPasswordModal
          open={passwordModal.open}
          user={passwordModal.user}
          onClose={closePasswordModal}
          onSubmit={updatePassword}
          addToast={addToast}
        />
      )}
    </AppLayout>
  );
}
