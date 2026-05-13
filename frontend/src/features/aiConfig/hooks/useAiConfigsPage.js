import { useState, useEffect, useMemo, useCallback, useRef } from "react";
import aiConfigApi from "../../../services/aiConfigApi";
import {
  AI_CONFIG_DEFAULT_PAGE_SIZE,
  AI_CONFIG_SEARCH_DEBOUNCE_MS,
} from "../constants/aiConfig.constants";

export function useAiConfigsPage() {
  const [allConfigs, setAllConfigs] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(AI_CONFIG_DEFAULT_PAGE_SIZE);

  const [modal, setModal] = useState({ open: false, mode: null, config: null });
  const [deleteModal, setDeleteModal] = useState({ open: false, config: null });

  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, AI_CONFIG_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  const fetchConfigs = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await aiConfigApi.GetAll();
      setAllConfigs(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || "Erro ao carregar configurações de IA.");
      setAllConfigs([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchConfigs();
  }, [fetchConfigs]);

  const filtered = useMemo(() => {
    let result = allConfigs;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((c) => c.title?.toLowerCase().includes(term));
    }

    if (statusFilter === "active") {
      result = result.filter((c) => c.isActive === true);
    } else if (statusFilter === "inactive") {
      result = result.filter((c) => c.isActive === false);
    }

    return result;
  }, [allConfigs, debouncedSearch, statusFilter]);

  const total = filtered.length;
  const totalPages = Math.max(1, Math.ceil(total / pageSize));
  const safePage = Math.min(page, totalPages);

  const paginatedItems = useMemo(() => {
    const start = (safePage - 1) * pageSize;
    return filtered.slice(start, start + pageSize);
  }, [filtered, safePage, pageSize]);

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

  const openCreateModal = useCallback(() => {
    setModal({ open: true, mode: "create", config: null });
  }, []);

  const openEditModal = useCallback((config) => {
    setModal({ open: true, mode: "edit", config });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, config: null });
  }, []);

  const openDeleteModal = useCallback((config) => {
    setDeleteModal({ open: true, config });
  }, []);

  const closeDeleteModal = useCallback(() => {
    setDeleteModal({ open: false, config: null });
  }, []);

  const createConfig = useCallback(
    async (data) => {
      await aiConfigApi.Create(data);
      closeModal();
      await fetchConfigs();
    },
    [closeModal, fetchConfigs],
  );

  const updateConfig = useCallback(
    async (id, data) => {
      await aiConfigApi.Update(id, data);
      closeModal();
      await fetchConfigs();
    },
    [closeModal, fetchConfigs],
  );

  const toggleConfigStatus = useCallback(
    async (config) => {
      if (config.isActive) {
        await aiConfigApi.Deactivate(config.id);
      } else {
        await aiConfigApi.Activate(config.id);
      }
      await fetchConfigs();
    },
    [fetchConfigs],
  );

  const deleteConfig = useCallback(
    async (id) => {
      await aiConfigApi.Delete(id);
      closeDeleteModal();
      await fetchConfigs();
    },
    [closeDeleteModal, fetchConfigs],
  );

  return {
    items: paginatedItems,
    isLoading,
    error,
    total,
    totalPages,

    search,
    setSearch,
    statusFilter,
    setStatusFilter: handleSetStatusFilter,
    clearFilters: handleClearFilters,

    page: safePage,
    pageSize,
    setPage,
    setPageSize: handleSetPageSize,

    modal,
    openCreateModal,
    openEditModal,
    closeModal,

    deleteModal,
    openDeleteModal,
    closeDeleteModal,

    createConfig,
    updateConfig,
    toggleConfigStatus,
    deleteConfig,

    refetch: fetchConfigs,
  };
}
