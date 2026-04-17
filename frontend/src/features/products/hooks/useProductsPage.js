import { useState, useEffect, useMemo, useCallback, useRef } from 'react';
import productAPI from '../../../services/product';
import {
  PRODUCT_DEFAULT_PAGE_SIZE,
  PRODUCT_SEARCH_DEBOUNCE_MS,
} from '../constants/product.constants';

/**
 * @typedef {'all'|'active'|'inactive'} StatusFilter
 *
 * @typedef {Object} ModalState
 * @property {boolean} open
 * @property {'create'|'edit'|null} mode
 * @property {object|null} product
 */

/**
 * Hook de orquestração da tela de listagem de produtos.
 * Centraliza carregamento, filtros, paginação no cliente, estado do modal e mutações.
 */
export function useProductsPage() {
  // --- Dados brutos ---
  const [allProducts, setAllProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // --- Filtros ---
  const [search, setSearch] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');

  // --- Paginação ---
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(PRODUCT_DEFAULT_PAGE_SIZE);

  // --- Modal ---
  const [modal, setModal] = useState({ open: false, mode: null, product: null });

  // --- Debounce da busca ---
  const debounceRef = useRef(null);

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, PRODUCT_SEARCH_DEBOUNCE_MS);
    return () => clearTimeout(debounceRef.current);
  }, [search]);

  // --- Carregamento ---
  const fetchProducts = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await productAPI.GetAll();
      setAllProducts(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err?.message || 'Erro ao carregar produtos.');
      setAllProducts([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchProducts();
  }, [fetchProducts]);

  // --- Dados filtrados e paginados ---
  const filtered = useMemo(() => {
    let result = allProducts;

    if (debouncedSearch) {
      const term = debouncedSearch.toLowerCase();
      result = result.filter((p) => p.name?.toLowerCase().includes(term));
    }

    if (statusFilter === 'active') {
      result = result.filter((p) => p.isActive === true);
    } else if (statusFilter === 'inactive') {
      result = result.filter((p) => p.isActive === false);
    }

    return result;
  }, [allProducts, debouncedSearch, statusFilter]);

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
    setModal({ open: true, mode: 'create', product: null });
  }, []);

  const openEditModal = useCallback((product) => {
    setModal({ open: true, mode: 'edit', product });
  }, []);

  const closeModal = useCallback(() => {
    setModal({ open: false, mode: null, product: null });
  }, []);

  // --- Mutações ---
  const createProduct = useCallback(
    async (data) => {
      await productAPI.Create(data);
      closeModal();
      await fetchProducts();
    },
    [closeModal, fetchProducts]
  );

  const updateProduct = useCallback(
    async (id, data) => {
      await productAPI.Update(id, data);
      closeModal();
      await fetchProducts();
    },
    [closeModal, fetchProducts]
  );

  const toggleProductStatus = useCallback(
    async (product) => {
      if (product.isActive) {
        await productAPI.Deactivate(product.id);
      } else {
        await productAPI.Activate(product.id);
      }
      await fetchProducts();
    },
    [fetchProducts]
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

    // Modal
    modal,
    openCreateModal,
    openEditModal,
    closeModal,

    // Mutações
    createProduct,
    updateProduct,
    toggleProductStatus,

    // Refetch manual
    refetch: fetchProducts,
  };
}
