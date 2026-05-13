import { useState, useEffect, useMemo, useCallback, useRef } from "react";
import opportunityAPI from "../../../services/opportunityApi";
import leadAPI from "../../../services/leadApi";
import ownerAPI from "../../../services/ownerApi";
import productAPI from "../../../services/productApi";
import {
  OPPORTUNITY_DEFAULT_PAGE_SIZE,
  OPPORTUNITY_SEARCH_DEBOUNCE_MS,
} from "../constants/opportunity.constants";

/**
 * Hook de orquestração da tela de listagem de oportunidades.
 */
export function useOpportunitiesPage() {
  // --- Dados brutos ---
  const [allOpportunities, setAllOpportunities] = useState([]);
  const [leads, setLeads] = useState([]);
  const [owners, setOwners] = useState([]);
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Filtros ---
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");

  // --- Paginação ---
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(OPPORTUNITY_DEFAULT_PAGE_SIZE);

  // --- Modal ---
  const [modal, setModal] = useState({
    open: false,
    mode: null,
    opportunity: null,
  });
  const [deleteModal, setDeleteModal] = useState({
    open: false,
    opportunity: null,
  });

  // --- Debounce da busca ---
  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, OPPORTUNITY_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  // --- Carregamento ---
  const fetchOpportunities = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await opportunityAPI.GetAll();
      setAllOpportunities(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || "Erro ao carregar oportunidades.");
      setAllOpportunities([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const fetchRelatedData = useCallback(async () => {
    try {
      const [leadsData, ownersData, productsData] = await Promise.all([
        leadAPI.GetAll({ isActive: true }),
        ownerAPI.GetAll({ isActive: true }),
        productAPI.GetAll({ isActive: true }),
      ]);
      setLeads(Array.isArray(leadsData) ? leadsData : []);
      setOwners(Array.isArray(ownersData) ? ownersData : []);
      setProducts(Array.isArray(productsData) ? productsData : []);
    } catch {
      setLeads([]);
      setOwners([]);
      setProducts([]);
    }
  }, []);

  useEffect(() => {
    fetchOpportunities();
    fetchRelatedData();
  }, [fetchOpportunities, fetchRelatedData]);

  // --- Dados filtrados e paginados ---
  const filtered = useMemo(() => {
    let result = allOpportunities;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((o) => o.title?.toLowerCase().includes(term));
    }

    if (statusFilter === "active") {
      result = result.filter((o) => o.isActive === true);
    } else if (statusFilter === "inactive") {
      result = result.filter((o) => o.isActive === false);
    }

    return result;
  }, [allOpportunities, debouncedSearch, statusFilter]);

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
    setModal({ open: true, mode: "create", opportunity: null });
  }, []);

  const openEditModal = useCallback((opportunity) => {
    setModal({ open: true, mode: "edit", opportunity });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, opportunity: null });
  }, []);

  const openDeleteModal = useCallback((opportunity) => {
    setDeleteModal({ open: true, opportunity });
  }, []);

  const closeDeleteModal = useCallback(() => {
    setDeleteModal({ open: false, opportunity: null });
  }, []);

  // --- Mutações ---
  const createOpportunity = useCallback(
    async (data) => {
      await opportunityAPI.Create(data);
      closeModal();
      await fetchOpportunities();
    },
    [closeModal, fetchOpportunities],
  );

  const updateOpportunity = useCallback(
    async (id, data) => {
      await opportunityAPI.Update(id, data);
      closeModal();
      await fetchOpportunities();
    },
    [closeModal, fetchOpportunities],
  );

  const toggleOpportunityStatus = useCallback(
    async (opportunity) => {
      if (opportunity.isActive) {
        await opportunityAPI.Deactivate(opportunity.id);
      } else {
        await opportunityAPI.Activate(opportunity.id);
      }
      await fetchOpportunities();
    },
    [fetchOpportunities],
  );

  const deleteOpportunity = useCallback(
    async (opportunityId) => {
      await opportunityAPI.Delete(opportunityId);
      closeDeleteModal();
      await fetchOpportunities();
    },
    [closeDeleteModal, fetchOpportunities],
  );

  return {
    // Listagem
    items: paginatedItems,
    isLoading,
    error,
    total,
    totalPages,

    // Dados relacionados para selects
    leads,
    owners,
    products,

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
    createOpportunity,
    updateOpportunity,
    toggleOpportunityStatus,
    deleteOpportunity,

    // Refetch manual
    refetch: fetchOpportunities,
  };
}
