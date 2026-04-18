import { useState, useEffect, useMemo, useCallback, useRef } from 'react';
import leadAPI from '../../../services/leadApi';
import {
  LEAD_DEFAULT_PAGE_SIZE,
  LEAD_SEARCH_DEBOUNCE_MS,
} from '../constants/lead.constants';

/**
 * Hook de orquestração da tela de listagem de leads.
 * Centraliza carregamento, filtros, paginação no cliente, estado do modal e mutações.
 */
export function useLeadsPage() {
  // --- Dados brutos ---
  const [allLeads, setAllLeads] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Filtros ---
  const [search, setSearch] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');

  // --- Paginação ---
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(LEAD_DEFAULT_PAGE_SIZE);

  // --- Modal ---
  const [modal, setModal] = useState({ open: false, mode: null, lead: null });
  const [deleteModal, setDeleteModal] = useState({ open: false, lead: null });

  // --- Debounce da busca ---
  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, LEAD_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  // --- Carregamento ---
  const fetchLeads = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await leadAPI.GetAll();
      setAllLeads(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || 'Erro ao carregar leads.');
      setAllLeads([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchLeads();
  }, [fetchLeads]);

  // --- Dados filtrados e paginados ---
  const filtered = useMemo(() => {
    let result = allLeads;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((l) => l.name?.toLowerCase().includes(term));
    }

    if (statusFilter === 'active') {
      result = result.filter((l) => l.isActive === true);
    } else if (statusFilter === 'inactive') {
      result = result.filter((l) => l.isActive === false);
    }

    return result;
  }, [allLeads, debouncedSearch, statusFilter]);

  const total = filtered.length;
  const totalPages = Math.max(1, Math.ceil(total / pageSize));
  const safePage = Math.min(page, totalPages);

  const paginatedItems = useMemo(() => {
    const start = (safePage - 1) * pageSize;
    return filtered.slice(start, start + pageSize);
  }, [filtered, safePage, pageSize]);

  // --- Handlers de filtro ---
  const handleSetStatusFilter = useCallback((value) => {
    setStatusFilter(value);
    setPage(1);
  }, []);

  const handleSetPageSize = useCallback((size) => {
    setPageSize(size);
    setPage(1);
  }, []);

  const handleClearFilters = useCallback(() => {
    setSearch('');
    setDebouncedSearch('');
    setStatusFilter('all');
    setPage(1);
  }, []);

  // --- Modal ---
  const openCreateModal = useCallback(() => {
    setModal({ open: true, mode: 'create', lead: null });
  }, []);

  const openEditModal = useCallback((lead) => {
    setModal({ open: true, mode: 'edit', lead });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, lead: null });
  }, []);

  const openDeleteModal = useCallback((lead) => {
    setDeleteModal({ open: true, lead });
  }, []);

  const closeDeleteModal = useCallback(() => {
    setDeleteModal({ open: false, lead: null });
  }, []);

  // --- Mutações ---
  const createLead = useCallback(
    async (data) => {
      await leadAPI.Create(data);
      closeModal();
      await fetchLeads();
    },
    [closeModal, fetchLeads]
  );

  const updateLead = useCallback(
    async (id, data) => {
      await leadAPI.Update(id, data);
      closeModal();
      await fetchLeads();
    },
    [closeModal, fetchLeads]
  );

  const toggleLeadStatus = useCallback(
    async (lead) => {
      if (lead.isActive) {
        await leadAPI.Deactivate(lead.id);
      } else {
        await leadAPI.Activate(lead.id);
      }
      await fetchLeads();
    },
    [fetchLeads]
  );

  const deleteLead = useCallback(
    async (leadId) => {
      await leadAPI.Delete(leadId);
      closeDeleteModal();
      await fetchLeads();
    },
    [closeDeleteModal, fetchLeads]
  );

  return {
    // Listagem
    items: paginatedItems,
    isLoading,
    error,
    total,
    totalPages,

    // Filtros
    search,
    setSearch,
    statusFilter,
    setStatusFilter: handleSetStatusFilter,
    clearFilters: handleClearFilters,

    // Paginação
    page: safePage,
    pageSize,
    setPage,
    setPageSize: handleSetPageSize,

    // Modal de criar/editar
    modal,
    openCreateModal,
    openEditModal,
    closeModal,

    // Modal de exclusão
    deleteModal,
    openDeleteModal,
    closeDeleteModal,

    // Mutações
    createLead,
    updateLead,
    toggleLeadStatus,
    deleteLead,

    // Refetch manual
    refetch: fetchLeads,
  };
}
