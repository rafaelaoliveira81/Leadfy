import { useDroppable } from "@dnd-kit/core";
import {
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import { MdAdd } from "react-icons/md";
import { KanbanCard } from "./KanbanCard";
import style from "./_kanban.module.css";

export function KanbanColumn({ stage, items, onCardClick, onAddClick }) {
  const { setNodeRef, isOver } = useDroppable({ id: String(stage.value) });

  const sortableIds = items.map((o) => String(o.id));

  return (
    <div
      className={style.column}
      style={isOver ? { boxShadow: `0 0 0 2px ${stage.accent}` } : undefined}
    >
      <div className={style.columnHeader}>
        <div
          className={style.columnAccent}
          style={{ background: stage.accent }}
        />
        <div className={style.columnTitleRow}>
          <span className={style.columnTitle}>{stage.label}</span>
          <span className={style.columnCount}>{items.length}</span>
        </div>
        <button
          className={style.columnAddBtn}
          onClick={() => onAddClick(stage.value)}
          aria-label={`Nova oportunidade em ${stage.label}`}
        >
          <MdAdd /> Nova
        </button>
      </div>

      <div className={style.columnCards} ref={setNodeRef}>
        <SortableContext
          items={sortableIds}
          strategy={verticalListSortingStrategy}
        >
          {items.length === 0 ? (
            <div className={style.columnEmpty}>Nenhuma oportunidade</div>
          ) : (
            items.map((opp) => (
              <KanbanCard
                key={opp.id}
                opportunity={opp}
                onClick={onCardClick}
              />
            ))
          )}
        </SortableContext>
      </div>
    </div>
  );
}
