import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { MdCalendarToday, MdAutoAwesome } from "react-icons/md";
import {
  formatCurrency,
  formatDateShort,
  getInitials,
  KANBAN_STAGE_BY_VALUE,
} from "../constants/kanban.constants";
import style from "./_kanban.module.css";

export function KanbanCard({ opportunity, onClick, onActionPlanClick }) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: String(opportunity.id) });

  const dragStyle = {
    transform: CSS.Transform.toString(transform),
    transition,
  };

  const stageInfo = KANBAN_STAGE_BY_VALUE[opportunity.stage] || {};

  return (
    <div
      ref={setNodeRef}
      style={dragStyle}
      className={`${style.card} ${isDragging ? style.cardDragging : ""}`}
      onClick={() => onClick(opportunity)}
      {...attributes}
      {...listeners}
    >
      <h4 className={style.cardTitle}>{opportunity.title}</h4>
      <p className={style.cardClient}>{opportunity.leadName || "—"}</p>
      <p className={style.cardAmount}>{formatCurrency(opportunity.amount)}</p>

      <div className={style.cardDateRow}>
        <MdCalendarToday className={style.cardDateIcon} />
        <span>{formatDateShort(opportunity.expectedCloseDate)}</span>
      </div>

      <div className={style.cardFooter}>
        <span
          className={style.cardBadge}
          style={{
            backgroundColor: stageInfo.accent
              ? `${stageInfo.accent}22`
              : "#edf2f4",
            color: stageInfo.accent || "#2b2d42",
          }}
        >
          {stageInfo.label || opportunity.stageName}
        </span>

        <div className={style.cardFooterRight}>
          <button
            className={`${style.cardActionPlanBtn} ${opportunity.actionPlan ? style.cardActionPlanBtnActive : ""}`}
            title={
              opportunity.actionPlan
                ? "Ver / Regenerar Plano de Ação"
                : "Gerar Plano de Ação"
            }
            onClick={(e) => {
              e.stopPropagation();
              if (onActionPlanClick) onActionPlanClick(opportunity);
            }}
          >
            <MdAutoAwesome />
          </button>

          {opportunity.ownerName && (
            <div className={style.cardAvatar} title={opportunity.ownerName}>
              {getInitials(opportunity.ownerName)}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
