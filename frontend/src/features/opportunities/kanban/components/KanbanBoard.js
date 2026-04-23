import {
  DndContext,
  DragOverlay,
  PointerSensor,
  useSensor,
  useSensors,
  closestCorners,
} from '@dnd-kit/core';
import { KANBAN_STAGES } from '../constants/kanban.constants';
import { KanbanColumn } from './KanbanColumn';
import { KanbanCard } from './KanbanCard';
import { KanbanProgress } from './KanbanProgress';
import style from './_kanban.module.css';

export function KanbanBoard({
  columns,
  isLoading,
  error,
  activeCard,
  onDragStart,
  onDragOver,
  onDragEnd,
  onCardClick,
  onAddClick,
}) {
  const sensors = useSensors(
    useSensor(PointerSensor, { activationConstraint: { distance: 5 } })
  );

  if (isLoading) {
    return <div className={style.loadingWrapper}>Carregando oportunidades...</div>;
  }

  return (
    <>
      {error && <div className={style.errorBanner}>{error}</div>}

      <KanbanProgress columns={columns} />

      <DndContext
        sensors={sensors}
        collisionDetection={closestCorners}
        onDragStart={onDragStart}
        onDragOver={onDragOver}
        onDragEnd={onDragEnd}
      >
        <div className={style.columnsContainer}>
          {KANBAN_STAGES.map((stage) => (
            <KanbanColumn
              key={stage.value}
              stage={stage}
              items={columns[stage.value] || []}
              onCardClick={onCardClick}
              onAddClick={onAddClick}
            />
          ))}
        </div>

        <DragOverlay>
          {activeCard ? (
            <KanbanCard opportunity={activeCard} onClick={() => {}} />
          ) : null}
        </DragOverlay>
      </DndContext>
    </>
  );
}
