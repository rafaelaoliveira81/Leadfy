import { useMemo } from 'react';
import { DataTable } from '../../../components/ui/DataTable/DataTable';
import { StatusBadge } from '../../../components/ui/StatusBadge/StatusBadge';
import { OwerRowActions } from './OwerRowActions';

/**
 * Tabela de owers com colunas pré-configuradas.
 */
export function OwerTable({
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
        key: 'userName',
        header: 'Usuário Vinculado',
        render: (value) => value || '—',
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
          <OwerRowActions
            ower={row}
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
