import { useState, useEffect, useMemo, useCallback, useRef } from "react";
import ownerAPI from "../../../services/ownerApi";
import userAPI from "../../../services/userApi";
import {
  OWNER_DEFAULT_PAGE_SIZE,
  OWNER_SEARCH_DEBOUNCE_MS,
} from "../constants/owner.constants";

/**
 * Hook de orquestração da tela de listagem de owners.
 * Centraliza carregamento, filtros, paginação no cliente, estado do modal e mutações.
 */
export function useOwnersPage() {
  // --- Dados brutos ---
  const [allOwners, setAllOwners] = useState([]);
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Filtros ---
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");

  // --- Paginação ---
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(OWNER_DEFAULT_PAGE_SIZE);

  // --- Modal ---
  const [modal, setModal] = useState({ open: false, mode: null, owner: null });
  const [deleteModal, setDeleteModal] = useState({ open: false, owner: null });

  // --- Debounce da busca ---
  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, OWNER_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  // --- Carregamento ---
  const fetchOwners = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await ownerAPI.GetAll();
      setAllOwners(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || "Erro ao carregar responsáveis.");
      setAllOwners([]);
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
    fetchOwners();
    fetchUsers();
  }, [fetchOwners, fetchUsers]);

  // --- Dados filtrados e paginados ---
  const filtered = useMemo(() => {
    let result = allOwners;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((o) => o.name?.toLowerCase().includes(term));
    }

    if (statusFilter === "active") {
      result = result.filter((o) => o.isActive === true);
    } else if (statusFilter === "inactive") {
      result = result.filter((o) => o.isActive === false);
    }

    return result;
  }, [allOwners, debouncedSearch, statusFilter]);

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
    setSearch("");
    setDebouncedSearch("");
    setStatusFilter("all");
    setPage(1);
  }, []);

  // --- Modal ---
  const openCreateModal = useCallback(() => {
    setModal({ open: true, mode: "create", owner: null });
  }, []);

  const openEditModal = useCallback((owner) => {
    setModal({ open: true, mode: "edit", owner });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, owner: null });
  }, []);

  const openDeleteModal = useCallback((owner) => {
    setDeleteModal({ open: true, owner });
  }, []);

  const closeDeleteModal = useCallback(() => {
    setDeleteModal({ open: false, owner: null });
  }, []);

  // --- Mutações ---
  const createOwner = useCallback(
    async (data) => {
      await ownerAPI.Create(data);
      closeModal();
      await fetchOwners();
    },
    [closeModal, fetchOwners],
  );

  const updateOwner = useCallback(
    async (id, data) => {
      await ownerAPI.Update(id, data);
      closeModal();
      await fetchOwners();
    },
    [closeModal, fetchOwners],
  );

  const toggleOwnerStatus = useCallback(
    async (owner) => {
      if (owner.isActive) {
        await ownerAPI.Deactivate(owner.id);
      } else {
        await ownerAPI.Activate(owner.id);
      }
      await fetchOwners();
    },
    [fetchOwners],
  );

  const deleteOwner = useCallback(
    async (ownerId) => {
      await ownerAPI.Delete(ownerId);
      closeDeleteModal();
      await fetchOwners();
    },
    [closeDeleteModal, fetchOwners],
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
    createOwner,
    updateOwner,
    toggleOwnerStatus,
    deleteOwner,

    // Refetch manual
    refetch: fetchOwners,
  };
}
