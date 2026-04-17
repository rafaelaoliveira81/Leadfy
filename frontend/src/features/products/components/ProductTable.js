import { useMemo } from 'react';
import { DataTable } from '../../../components/ui/DataTable/DataTable';
import { StatusBadge } from '../../../components/ui/StatusBadge/StatusBadge';
import { ProductRowActions } from './ProductRowActions';
import { formatPrice } from '../mappers/product.mapper';

/**
 * Tabela de produtos com colunas pré-configuradas.
 * @param {object} props
 * @param {object[]} props.items
 * @param {boolean} props.isLoading
 * @param {(product: object) => void} props.onEdit
 * @param {(product: object) => void} props.onToggleStatus
 * @param {(product: object) => void} props.onDelete
 * @param {string} [props.emptyMessage]
 * @param {string} [props.emptyActionLabel]
 * @param {() => void} [props.onEmptyAction]
 */
export function ProductTable({
  items,
  isLoading,
  onEdit,
  onToggleStatus,
  onDelete,
  emptyMessage,
  emptyActionLabel,
  onEmptyAction,
}) {
  const columns = useMemo(
    () => [
      { key: 'name', header: 'Nome' },
      {
        key: 'description',
        header: 'Descrição',
        render: (value) => value || '—',
      },
      {
        key: 'price',
        header: 'Preço',
        render: (value) => formatPrice(value),
      },
      {
        key: 'isActive',
        header: 'Status',
        render: (value) => <StatusBadge isActive={value} />,
      },
      {
        key: 'actions',
        header: 'Ações',
        render: (_value, row) => (
          <ProductRowActions
            product={row}
            onEdit={onEdit}
            onToggleStatus={onToggleStatus}
            onDelete={onDelete}
          />
        ),
      },
    ],
    [onEdit, onToggleStatus, onDelete]
  );

  return (
    <DataTable
      columns={columns}
      data={items}
      isLoading={isLoading}
      emptyMessage={emptyMessage}
      emptyActionLabel={emptyActionLabel}
      onEmptyAction={onEmptyAction}
    />
  );
}
