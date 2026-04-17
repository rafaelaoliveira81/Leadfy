import { AppLayout } from '../../layouts/AppLayout/AppLayout';
import { useToast } from '../../components/ui/Toast/Toast';
import { Pagination } from '../../components/ui/Pagination/Pagination';
import { ProductListHeader } from '../../features/products/components/ProductListHeader';
import { ProductFilters } from '../../features/products/components/ProductFilters';
import { ProductTable } from '../../features/products/components/ProductTable';
import { ProductFormModal } from '../../features/products/components/ProductFormModal';
import { ProductDeleteModal } from '../../features/products/components/ProductDeleteModal';
import { useProductsPage } from '../../features/products/hooks/useProductsPage';
import style from './_products.module.css';

export function Products() {
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
    createProduct,
    updateProduct,
    toggleProductStatus,
    deleteProduct,
  } = useProductsPage();

  const handleToggleStatus = async (product) => {
    try {
      await toggleProductStatus(product);
      addToast(
        product.isActive
          ? 'Produto desativado com sucesso!'
          : 'Produto ativado com sucesso!',
        'success'
      );
    } catch {
      addToast('Erro ao alterar status do produto.', 'error');
    }
  };

  const handleDelete = async (productId) => {
    try {
      await deleteProduct(productId);
      addToast('Produto excluído com sucesso!', 'success');
    } catch {
      addToast('Erro ao excluir produto.', 'error');
    }
  };

  return (
    <AppLayout>
      <ProductListHeader />

      <div className={style.card}>
        <ProductFilters
          search={search}
          onSearchChange={setSearch}
          statusFilter={statusFilter}
          onStatusFilterChange={setStatusFilter}
          onClearFilters={clearFilters}
          onNewProduct={openCreateModal}
        />

        {error && <div className={style.errorBanner}>{error}</div>}

        <ProductTable
          items={items}
          isLoading={isLoading}
          onEdit={openEditModal}
          onToggleStatus={handleToggleStatus}
          onDelete={openDeleteModal}
          emptyMessage="Nenhum produto encontrado."
          emptyActionLabel="+ Novo Produto"
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
        <ProductFormModal
          open={modal.open}
          mode={modal.mode}
          product={modal.product}
          onClose={closeModal}
          onCreateSubmit={createProduct}
          onUpdateSubmit={updateProduct}
          addToast={addToast}
        />
      )}

      {deleteModal.open && (
        <ProductDeleteModal
          open={deleteModal.open}
          product={deleteModal.product}
          onClose={closeDeleteModal}
          onConfirm={handleDelete}
        />
      )}
    </AppLayout>
  );
}
