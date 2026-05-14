import { useMemo } from "react";
import { DataTable } from "../../../components/ui/DataTable/DataTable";
import { StatusBadge } from "../../../components/ui/StatusBadge/StatusBadge";
import { OpportunityRowActions } from "./OpportunityRowActions";
import {
  formatStageName,
  formatAmount,
  formatDate,
} from "../mappers/opportunity.mapper";

/**
 * Tabela de oportunidades com colunas pré-configuradas.
 */
export function OpportunityTable({
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
      {
        key: "leadName",
        header: "Lead",
        render: (value) => value || "—",
      },
      {
        key: "ownerName",
        header: "Responsável",
        render: (value) => value || "—",
      },
      {
        key: "productName",
        header: "Produto",
        render: (value) => value || "—",
      },
      {
        key: "stageName",
        header: "Etapa",
        render: (value) => formatStageName(value),
      },
      {
        key: "amount",
        header: "Valor",
        render: (value) => formatAmount(value),
      },
      {
        key: "expectedCloseDate",
        header: "Previsão",
        render: (value) => formatDate(value),
      },
      {
        key: "isActive",
        header: "Status",
        render: (value) => <StatusBadge isActive={value} />,
      },
      {
        key: "actions",
        header: "Ações",
        render: (_value, row) => (
          <OpportunityRowActions
            opportunity={row}
            onEdit={onEdit}
            onToggleStatus={onToggleStatus}
            onDelete={onDelete}
          />
        ),
      },
    ],
    [onEdit, onToggleStatus, onDelete],
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
