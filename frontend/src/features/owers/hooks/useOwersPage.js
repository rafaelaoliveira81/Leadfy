import { useState, useEffect, useMemo, useCallback, useRef } from 'react';
import owerAPI from '../../../services/owerApi';
import userAPI from '../../../services/userApi';
import {
  OWER_DEFAULT_PAGE_SIZE,
  OWER_SEARCH_DEBOUNCE_MS,
} from '../constants/ower.constants';

/**
 * Hook de orquestração da tela de listagem de owers.
 * Centraliza carregamento, filtros, paginação no cliente, estado do modal e mutações.
 */
export function useOwersPage() {
  // --- Dados brutos ---
  const [allOwers, setAllOwers] = useState([]);
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Filtros ---
  const [search, setSearch] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');

  // --- Paginação ---
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(OWER_DEFAULT_PAGE_SIZE);

  // --- Modal ---
  const [modal, setModal] = useState({ open: false, mode: null, ower: null });
  const [deleteModal, setDeleteModal] = useState({ open: false, ower: null });

  // --- Debounce da busca ---
  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, OWER_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  // --- Carregamento ---
  const fetchOwers = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await owerAPI.GetAll();
      setAllOwers(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || 'Erro ao carregar responsáveis.');
      setAllOwers([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const fetchUsers = useCallback(async () => {
    try {
      const data = await userAPI.GetAll({ isActive: true });
      setUsers(Array.isArray(data) ? data : []);
    } catch {
      setUsers([]);
    }
  }, []);

  useEffect(() => {
    fetchOwers();
    fetchUsers();
  }, [fetchOwers, fetchUsers]);

  // --- Dados filtrados e paginados ---
  const filtered = useMemo(() => {
    let result = allOwers;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((o) => o.name?.toLowerCase().includes(term));
    }

    if (statusFilter === 'active') {
      result = result.filter((o) => o.isActive === true);
    } else if (statusFilter === 'inactive') {
      result = result.filter((o) => o.isActive === false);
    }

    return result;
  }, [allOwers, debouncedSearch, statusFilter]);

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
    setModal({ open: true, mode: 'create', ower: null });
  }, []);

  const openEditModal = useCallback((ower) => {
    setModal({ open: true, mode: 'edit', ower });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, ower: null });
  }, []);

  const openDeleteModal = useCallback((ower) => {
    setDeleteModal({ open: true, ower });
  }, []);

  const closeDeleteModal = useCallback(() => {
    setDeleteModal({ open: false, ower: null });
  }, []);

  // --- Mutações ---
  const createOwer = useCallback(
    async (data) => {
      await owerAPI.Create(data);
      closeModal();
      await fetchOwers();
    },
    [closeModal, fetchOwers]
  );

  const updateOwer = useCallback(
    async (id, data) => {
      await owerAPI.Update(id, data);
      closeModal();
      await fetchOwers();
    },
    [closeModal, fetchOwers]
  );

  const toggleOwerStatus = useCallback(
    async (ower) => {
      if (ower.isActive) {
        await owerAPI.Deactivate(ower.id);
      } else {
        await owerAPI.Activate(ower.id);
      }
      await fetchOwers();
    },
    [fetchOwers]
  );

  const deleteOwer = useCallback(
    async (owerId) => {
      await owerAPI.Delete(owerId);
      closeDeleteModal();
      await fetchOwers();
    },
    [closeDeleteModal, fetchOwers]
  );

  return {
    // Listagem
    items: paginatedItems,
    isLoading,
    error,
    total,
    totalPages,

    // Users para select
    users,

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
    createOwer,
    updateOwer,
    toggleOwerStatus,
    deleteOwer,

    // Refetch manual
    refetch: fetchOwers,
  };
}
