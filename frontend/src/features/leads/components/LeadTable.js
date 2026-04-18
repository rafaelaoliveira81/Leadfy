import { useMemo } from 'react';
import { DataTable } from '../../../components/ui/DataTable/DataTable';
import { StatusBadge } from '../../../components/ui/StatusBadge/StatusBadge';
import { LeadRowActions } from './LeadRowActions';
import { formatPhone } from '../mappers/lead.mapper';

/**
 * Tabela de leads com colunas pré-configuradas.
 * @param {object} props
 * @param {object[]} props.items
 * @param {boolean} props.isLoading
 * @param {(lead: object) => void} props.onEdit
 * @param {(lead: object) => void} props.onToggleStatus
 * @param {(lead: object) => void} props.onDelete
 * @param {string} [props.emptyMessage]
 * @param {string} [props.emptyActionLabel]
 * @param {() => void} [props.onEmptyAction]
 */
export function LeadTable({
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
      { key: 'email', header: 'E-mail' },
      {
        key: 'phoneNumber',
        header: 'Telefone',
        render: (value) => formatPhone(value),
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
          <LeadRowActions
            lead={row}
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
