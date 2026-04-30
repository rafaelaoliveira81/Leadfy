import { useMemo } from 'react';
import { DataTable } from '../../../components/ui/DataTable/DataTable';
import { StatusBadge } from '../../../components/ui/StatusBadge/StatusBadge';
import { AiConfigRowActions } from './AiConfigRowActions';
import { formatCreatedAt, truncatePrompt } from '../mappers/aiConfig.mapper';

export function AiConfigTable({
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
      { key: 'modelName', header: 'Modelo' },
      {
        key: 'promptTemplate',
        header: 'Template do Prompt',
        render: (value) => (
          <span title={value}>{truncatePrompt(value)}</span>
        ),
      },
      {
        key: 'apiKeyMasked',
        header: 'Chave de API',
        render: (value) => (
          <code style={{ fontSize: '13px', color: '#8d99ae' }}>{value}</code>
        ),
      },
      {
        key: 'isActive',
        header: 'Status',
        render: (value) => <StatusBadge isActive={value} />,
      },
      {
        key: 'createdAt',
        header: 'Criado em',
        render: (value) => formatCreatedAt(value),
      },
      {
        key: 'actions',
        header: 'Ações',
        render: (_value, row) => (
          <AiConfigRowActions
            config={row}
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
