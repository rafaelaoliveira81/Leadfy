import { useMemo } from 'react';
import style from './_pagination.module.css';

const PAGE_SIZES = [10, 25, 50, 100];

/**
 * Rodapé de paginação reutilizável.
 * @param {object} props
 * @param {number} props.page - Página atual (1-based).
 * @param {number} props.pageSize - Itens por página.
 * @param {number} props.total - Total de itens.
 * @param {number} props.totalPages - Total de páginas.
 * @param {(page: number) => void} props.onPageChange
 * @param {(size: number) => void} props.onPageSizeChange
 */
export function Pagination({
  page,
  pageSize,
  total,
  totalPages,
  onPageChange,
  onPageSizeChange,
}) {
  const pages = useMemo(() => {
    const result = [];
    const maxVisible = 5;
    let start = Math.max(1, page - Math.floor(maxVisible / 2));
    let end = start + maxVisible - 1;

    if (end > totalPages) {
      end = totalPages;
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      result.push(i);
    }
    return result;
  }, [page, totalPages]);

  const from = total === 0 ? 0 : (page - 1) * pageSize + 1;
  const to = Math.min(page * pageSize, total);

  return (
    <div className={style.pagination}>
      <div className={style.info}>
        <span>
          Mostrando {from}–{to} de {total}
        </span>
        <select
          className={style.pageSizeSelect}
          value={pageSize}
          onChange={(e) => onPageSizeChange(Number(e.target.value))}
          aria-label="Itens por página"
        >
          {PAGE_SIZES.map((s) => (
            <option key={s} value={s}>
              {s} / página
            </option>
          ))}
        </select>
      </div>
      <div className={style.pages}>
        <button
          className={style.pageBtn}
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
          aria-label="Página anterior"
        >
          ‹
        </button>
        {pages.map((p) => (
          <button
            key={p}
            className={`${style.pageBtn} ${p === page ? style.active : ''}`}
            onClick={() => onPageChange(p)}
            aria-label={`Página ${p}`}
            aria-current={p === page ? 'page' : undefined}
          >
            {p}
          </button>
        ))}
        <button
          className={style.pageBtn}
          disabled={page >= totalPages}
          onClick={() => onPageChange(page + 1)}
          aria-label="Próxima página"
        >
          ›
        </button>
      </div>
    </div>
  );
}
