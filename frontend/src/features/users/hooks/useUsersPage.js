import { useState, useEffect, useMemo, useCallback, useRef } from 'react';
import userAPI from '../../../services/userApi';
import {
  USER_DEFAULT_PAGE_SIZE,
  USER_SEARCH_DEBOUNCE_MS,
} from '../constants/user.constants';

/**
 * Hook de orquestração da tela de listagem de usuários.
 */
export function useUsersPage() {
  // --- Dados brutos ---
  const [allUsers, setAllUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Filtros ---
  const [search, setSearch] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');

  // --- Paginação ---
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(USER_DEFAULT_PAGE_SIZE);

  // --- Modal ---
  const [modal, setModal] = useState({ open: false, mode: null, user: null });
  const [deleteModal, setDeleteModal] = useState({ open: false, user: null });
  const [passwordModal, setPasswordModal] = useState({ open: false, user: null });

  // --- Debounce da busca ---
  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, USER_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  // --- Carregamento ---
  const fetchUsers = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await userAPI.GetAll();
      setAllUsers(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || 'Erro ao carregar usuários.');
      setAllUsers([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const fetchRoles = useCallback(async () => {
    try {
      const data = await userAPI.GetRoles();
      setRoles(Array.isArray(data) ? data : []);
    } catch {
      setRoles([]);
    }
  }, []);

  useEffect(() => {
    fetchUsers();
    fetchRoles();
  }, [fetchUsers, fetchRoles]);

  // --- Dados filtrados e paginados ---
  const filtered = useMemo(() => {
    let result = allUsers;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((u) => u.name?.toLowerCase().includes(term));
    }

    if (statusFilter === 'active') {
      result = result.filter((u) => u.isActive === true);
    } else if (statusFilter === 'inactive') {
      result = result.filter((u) => u.isActive === false);
    }

    return result;
  }, [allUsers, debouncedSearch, statusFilter]);

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
    setModal({ open: true, mode: 'create', user: null });
  }, []);

  const openEditModal = useCallback((user) => {
    setModal({ open: true, mode: 'edit', user });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, user: null });
  }, []);

  const openDeleteModal = useCallback((user) => {
    setDeleteModal({ open: true, user });
  }, []);

  const closeDeleteModal = useCallback(() => {
    setDeleteModal({ open: false, user: null });
  }, []);

  const openPasswordModal = useCallback((user) => {
    setPasswordModal({ open: true, user });
  }, []);

  const closePasswordModal = useCallback(() => {
    setPasswordModal({ open: false, user: null });
  }, []);

  // --- Mutações ---
  const createUser = useCallback(
    async (data) => {
      await userAPI.Create(data);
      closeModal();
      await fetchUsers();
    },
    [closeModal, fetchUsers]
  );

  const updateUser = useCallback(
    async (id, data) => {
      await userAPI.Update(id, data);
      closeModal();
      await fetchUsers();
    },
    [closeModal, fetchUsers]
  );

  const toggleUserStatus = useCallback(
    async (user) => {
      if (user.isActive) {
        await userAPI.Deactivate(user.id);
      } else {
        await userAPI.Activate(user.id);
      }
      await fetchUsers();
    },
    [fetchUsers]
  );

  const deleteUser = useCallback(
    async (userId) => {
      await userAPI.Delete(userId);
      closeDeleteModal();
      await fetchUsers();
    },
    [closeDeleteModal, fetchUsers]
  );

  const updatePassword = useCallback(
    async (userId, passwordData) => {
      await userAPI.UpdatePassword(userId, passwordData);
      closePasswordModal();
    },
    [closePasswordModal]
  );

  return {
    // Listagem
    items: paginatedItems,
    isLoading,
    error,
    total,
    totalPages,

    // Roles para select
    roles,

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

    // Modal de senha
    passwordModal,
    openPasswordModal,
    closePasswordModal,

    // Mutações
    createUser,
    updateUser,
    toggleUserStatus,
    deleteUser,
    updatePassword,

    // Refetch manual
    refetch: fetchUsers,
  };
}
