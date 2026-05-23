import { useMemo } from 'react';
import { DataTable } from '../../../components/ui/DataTable/DataTable';
import { StatusBadge } from '../../../components/ui/StatusBadge/StatusBadge';
import { UserRowActions } from './UserRowActions';

/**
 * Tabela de usuários com colunas pré-configuradas.
 */
export function UserTable({
  items,
  isLoading,
  onEdit,
  onToggleStatus,
  onDelete,
  onChangePassword,
  emptyMessage,
  emptyActionLabel,
  onEmptyAction,
}) {
  const columns = useMemo(
    () => [
      { key: 'name', header: 'Nome' },
      { key: 'email', header: 'E-mail' },
      {
        key: 'isActive',
        header: 'Status',
        render: (value) => <StatusBadge isActive={value} />,
      },
      {
        key: 'actions',
        header: 'Ações',
        render: (_value, row) => (
          <UserRowActions
            user={row}
            onEdit={onEdit}
            onToggleStatus={onToggleStatus}
            onDelete={onDelete}
            onChangePassword={onChangePassword}
          />
        ),
      },
    ],
    [onEdit, onToggleStatus, onDelete, onChangePassword]
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
