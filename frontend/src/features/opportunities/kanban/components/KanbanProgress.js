import { KANBAN_STAGES, formatCurrency } from '../constants/kanban.constants';
import style from './_kanban.module.css';

export function KanbanProgress({ columns }) {
  const totalCount = Object.values(columns).reduce((sum, arr) => sum + arr.length, 0);
  const totalValue = Object.values(columns)
    .flat()
    .reduce((sum, o) => sum + (Number(o.amount) || 0), 0);

  if (totalCount === 0) return null;

  return (
    <div className={style.progressWrapper}>
      <p className={style.progressTitle}>
        Funil de Vendas — {totalCount} oportunidades — {formatCurrency(totalValue)}
      </p>

      <div className={style.progressBar}>
        {KANBAN_STAGES.map((stage) => {
          const count = (columns[stage.value] || []).length;
          const pct = totalCount > 0 ? (count / totalCount) * 100 : 0;
          if (pct === 0) return null;
          return (
            <div
              key={stage.value}
              className={style.progressSegment}
              style={{ width: `${pct}%`, background: stage.accent }}
              title={`${stage.label}: ${count}`}
            />
          );
        })}
      </div>

      <div className={style.progressLegend}>
        {KANBAN_STAGES.map((stage) => {
          const items = columns[stage.value] || [];
          const stageValue = items.reduce((s, o) => s + (Number(o.amount) || 0), 0);
          return (
            <div key={stage.value} className={style.legendItem}>
              <span className={style.legendDot} style={{ background: stage.accent }} />
              {stage.label}:
              <span className={style.legendValue}>
                {items.length} ({formatCurrency(stageValue)})
              </span>
            </div>
          );
        })}
      </div>
    </div>
  );
}
