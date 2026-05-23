import style from './_dataTable.module.css';
import { LoadingState } from '../LoadingState/LoadingState';
import { EmptyState } from '../EmptyState/EmptyState';

/**
 * @typedef {{ key: string, header: string, render?: (value: any, row: object) => JSX.Element }} Column
 */

/**
 * Tabela genérica reutilizável.
 * @param {object} props
 * @param {Column[]} props.columns - Definição das colunas.
 * @param {object[]} props.data - Linhas da tabela.
 * @param {boolean} [props.isLoading] - Exibir skeleton de carregamento.
 * @param {string} [props.emptyMessage] - Mensagem quando não há dados.
 * @param {string} [props.emptyActionLabel] - Label do botão no empty state.
 * @param {Function} [props.onEmptyAction] - Callback do botão no empty state.
 * @param {(row: object) => string|number} [props.rowKey] - Chave única da linha.
 */
export function DataTable({
  columns,
  data,
  isLoading = false,
  emptyMessage = 'Nenhum registro encontrado.',
  emptyActionLabel,
  onEmptyAction,
  rowKey = (row) => row.id,
}) {
  if (isLoading) {
    return <LoadingState rows={5} />;
  }

  if (!data || data.length === 0) {
    return (
      <EmptyState
        message={emptyMessage}
        actionLabel={emptyActionLabel}
        onAction={onEmptyAction}
      />
    );
  }

  return (
    <div className={style.wrapper}>
      <table className={style.table}>
        <thead>
          <tr>
            {columns.map((col) => (
              <th key={col.key}>{col.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row) => (
            <tr key={rowKey(row)}>
              {columns.map((col) => (
                <td key={col.key}>
                  {col.render ? col.render(row[col.key], row) : row[col.key]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
