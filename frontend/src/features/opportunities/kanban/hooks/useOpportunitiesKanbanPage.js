import { useState, useEffect, useCallback, useRef } from 'react';
import { arrayMove } from '@dnd-kit/sortable';
import opportunityAPI from '../../../../services/opportunityApi';
import leadAPI from '../../../../services/leadApi';
import owerAPI from '../../../../services/owerApi';
import productAPI from '../../../../services/productApi';
import { KANBAN_STAGES } from '../constants/kanban.constants';

/**
 * Agrupa oportunidades por stage (valor numérico do enum).
 * Retorna um objeto { [stageValue]: Opportunity[] }.
 */
function groupByStage(opportunities) {
  const grouped = {};
  KANBAN_STAGES.forEach((s) => {
    grouped[s.value] = [];
  });
  opportunities.forEach((opp) => {
    const stage = opp.stage;
    if (grouped[stage]) {
      grouped[stage].push(opp);
    }
  });
  // Ordenar cada coluna por sortOrder
  Object.keys(grouped).forEach((key) => {
    grouped[key].sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0));
  });
  return grouped;
}

/**
 * Encontra em qual coluna está um card pelo id.
 */
function findColumnOfCard(columns, cardId) {
  for (const [stage, items] of Object.entries(columns)) {
    if (items.some((o) => String(o.id) === String(cardId))) {
      return stage;
    }
  }
  return null;
}

/**
 * Hook de orquestração do Kanban de oportunidades.
 * Isolado do useOpportunitiesPage para não afetar a tela CRUD.
 */
export function useOpportunitiesKanbanPage() {
  // --- Dados ---
  const [columns, setColumns] = useState(() => {
    const init = {};
    KANBAN_STAGES.forEach((s) => { init[s.value] = []; });
    return init;
  });
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Related data for create modal ---
  const [leads, setLeads] = useState([]);
  const [owers, setOwers] = useState([]);
  const [products, setProducts] = useState([]);

  // --- DnD ---
  const [activeCard, setActiveCard] = useState(null);

  // --- Detail modal ---
  const [detailModal, setDetailModal] = useState({ open: false, opportunity: null });

  // --- Create modal ---
  const [createModal, setCreateModal] = useState({ open: false, defaultStage: 1 });

  // Snapshot for optimistic revert
  const prevColumnsRef = useRef(null);

  // --- Fetch ---
  const fetchOpportunities = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await opportunityAPI.GetAll({ isActive: true });
      const list = Array.isArray(data) ? data : [];
      setColumns(groupByStage(list));
    } catch (err) {
      setError(err?.message || 'Erro ao carregar oportunidades.');
    } finally {
      setIsLoading(false);
    }
  }, []);

  const fetchRelatedData = useCallback(async () => {
    try {
      const [leadsData, owersData, productsData] = await Promise.all([
        leadAPI.GetAll({ isActive: true }),
        owerAPI.GetAll({ isActive: true }),
        productAPI.GetAll({ isActive: true }),
      ]);
      setLeads(Array.isArray(leadsData) ? leadsData : []);
      setOwers(Array.isArray(owersData) ? owersData : []);
      setProducts(Array.isArray(productsData) ? productsData : []);
    } catch {
      /* silently fallback */
    }
  }, []);

  useEffect(() => {
    fetchOpportunities();
    fetchRelatedData();
  }, [fetchOpportunities, fetchRelatedData]);

  // --- DnD handlers ---
  const handleDragStart = useCallback(
    (event) => {
      const { active } = event;
      const col = findColumnOfCard(columns, active.id);
      if (col) {
        const card = columns[col].find((o) => String(o.id) === String(active.id));
        setActiveCard(card || null);
      }
    },
    [columns]
  );

  const handleDragOver = useCallback(
    (event) => {
      const { active, over } = event;
      if (!over) return;

      const activeCol = findColumnOfCard(columns, active.id);
      // over.id pode ser o id de um card OU o id de uma coluna (droppable)
      let overCol = findColumnOfCard(columns, over.id);
      if (!overCol) {
        // É um droppable de coluna
        overCol = String(over.id);
      }

      if (!activeCol || !overCol || activeCol === overCol) return;

      setColumns((prev) => {
        const sourceItems = [...prev[activeCol]];
        const destItems = [...prev[overCol]];
        const activeIndex = sourceItems.findIndex((o) => String(o.id) === String(active.id));
        if (activeIndex === -1) return prev;

        const [movedCard] = sourceItems.splice(activeIndex, 1);
        movedCard.stage = Number(overCol);

        // Insere na posição do over, ou no final
        const overIndex = destItems.findIndex((o) => String(o.id) === String(over.id));
        if (overIndex >= 0) {
          destItems.splice(overIndex, 0, movedCard);
        } else {
          destItems.push(movedCard);
        }

        return {
          ...prev,
          [activeCol]: sourceItems,
          [overCol]: destItems,
        };
      });
    },
    [columns]
  );

  const handleDragEnd = useCallback(
    async (event) => {
      const { active, over } = event;
      setActiveCard(null);

      if (!over) return;

      const activeCol = findColumnOfCard(columns, active.id);
      let overCol = findColumnOfCard(columns, over.id);
      if (!overCol) overCol = String(over.id);

      if (!activeCol) return;

      // Save snapshot for revert
      prevColumnsRef.current = JSON.parse(JSON.stringify(columns));

      // Same column reorder
      if (activeCol === overCol) {
        setColumns((prev) => {
          const items = [...prev[activeCol]];
          const oldIndex = items.findIndex((o) => String(o.id) === String(active.id));
          const newIndex = items.findIndex((o) => String(o.id) === String(over.id));
          if (oldIndex === -1 || newIndex === -1 || oldIndex === newIndex) return prev;

          return {
            ...prev,
            [activeCol]: arrayMove(items, oldIndex, newIndex),
          };
        });
      }

      // Persist — build reorder payload for affected columns
      try {
        // Wait for state to update via a microtask
        await new Promise((r) => setTimeout(r, 0));

        // Read current columns from state
        setColumns((current) => {
          const affectedStages = activeCol === overCol
            ? [activeCol]
            : [activeCol, overCol];

          const items = [];
          affectedStages.forEach((stageKey) => {
            (current[stageKey] || []).forEach((opp, idx) => {
              items.push({
                id: opp.id,
                stage: Number(stageKey),
                sortOrder: idx,
              });
            });
          });

          // Fire and forget the API call — revert on error
          opportunityAPI.PatchSortOrder(items).catch(() => {
            if (prevColumnsRef.current) {
              setColumns(prevColumnsRef.current);
            }
          });

          return current;
        });
      } catch {
        if (prevColumnsRef.current) {
          setColumns(prevColumnsRef.current);
        }
      }
    },
    [columns]
  );

  // --- Stage change from modal ---
  const handleStageChange = useCallback(
    async (opportunityId, newStage) => {
      prevColumnsRef.current = JSON.parse(JSON.stringify(columns));
      try {
        await opportunityAPI.PatchStage(opportunityId, newStage);
        await fetchOpportunities();
      } catch (err) {
        if (prevColumnsRef.current) {
          setColumns(prevColumnsRef.current);
        }
        throw err;
      }
    },
    [columns, fetchOpportunities]
  );

  // --- Modal handlers ---
  const openDetailModal = useCallback((opportunity) => {
    setDetailModal({ open: true, opportunity });
  }, []);

  const closeDetailModal = useCallback(() => {
    setDetailModal({ open: false, opportunity: null });
  }, []);

  const openCreateModal = useCallback((stageValue) => {
    setCreateModal({ open: true, defaultStage: stageValue });
  }, []);

  const closeCreateModal = useCallback(() => {
    setCreateModal({ open: false, defaultStage: 1 });
  }, []);

  // --- Create submit ---
  const createOpportunity = useCallback(
    async (data) => {
      await opportunityAPI.Create(data);
      closeCreateModal();
      await fetchOpportunities();
    },
    [closeCreateModal, fetchOpportunities]
  );

  return {
    columns,
    isLoading,
    error,
    activeCard,

    // DnD
    handleDragStart,
    handleDragOver,
    handleDragEnd,

    // Detail modal
    detailModal,
    openDetailModal,
    closeDetailModal,
    handleStageChange,

    // Create modal
    createModal,
    openCreateModal,
    closeCreateModal,
    createOpportunity,

    // Related data
    leads,
    owers,
    products,

    // Refresh
    refresh: fetchOpportunities,
  };
}
