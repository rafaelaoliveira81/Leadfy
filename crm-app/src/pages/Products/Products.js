import { useState, useEffect, useCallback } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import {
  MdEdit,
  MdDelete,
  MdChevronLeft,
  MdChevronRight,
  MdBlock,
  MdCheckCircleOutline,
} from "react-icons/md";
import { Button } from "../../components/Button/Button";
import { Sidebar } from "../../components/Sidebar/Sidebar";

import { ListingHeader } from "../../components/ListingHeader/ListingHeader";
import { useAuth } from "../../context/AuthContext";
import { productAPI } from "../../services/productApi";
import GetPageNumbers from "../../utils/Pagination";
import style from "./_product.module.css";

const ITEMS_PER_PAGE = 10;

const INITIAL_PRODUCT_STATE = {
  id: null,
  name: "",
  description: "",
  price: "",
};

export function Products() {
  const { isAuthenticated } = useAuth();
  const [allProducts, setAllProducts] = useState([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState("active");

  const [product, setProduct] = useState(INITIAL_PRODUCT_STATE);
  const [selectedProduct, setSelectedProduct] = useState(null);

  const [confirmAction, setConfirmAction] = useState(null);
  const [productFormMode, setProductFormMode] = useState(null);

  const totalPages = Math.max(1, Math.ceil(totalRecords / ITEMS_PER_PAGE));

  const isProductFormOpen = productFormMode !== null;
  const isEditing = productFormMode === "edit";
  const productFormTitle = isEditing ? "Editar Produto" : "Novo Produto";

  const isFormValid = () => {
    const newErrors = {};

    if (!product.name?.trim()) {
      newErrors.name = true;
    }

    if (!product.description?.trim()) {
      newErrors.description = true;
    }

    if (
      product.price === "" ||
      product.price === null ||
      Number.isNaN(Number(product.price)) ||
      Number(product.price) < 0
    ) {
      newErrors.price = true;
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const getPageNumbers = GetPageNumbers(totalPages);

  const getStatusParam = useCallback((filter) => {
    if (filter === "all") {
      return null;
    }

    return filter === "active";
  }, []);

  const buildProductPayload = (currentProduct) => ({
    id: currentProduct.id ?? 0,
    name: currentProduct.name?.trim() ?? "",
    description: currentProduct.description?.trim() ?? "",
    price: Number(currentProduct.price) || 0,
  });

  const formatCurrency = (value) => {
    const numericValue = Number(value);

    if (Number.isNaN(numericValue)) {
      return "-";
    }

    return new Intl.NumberFormat("pt-BR", {
      style: "currency",
      currency: "BRL",
    }).format(numericValue);
  };

  const fetchProducts = useCallback(async (status, page) => {
    setIsLoading(true);

    try {
      const data = await productAPI.GetPaged(status, page, ITEMS_PER_PAGE);
      setAllProducts(Array.isArray(data?.dados) ? data.dados : []);
      setTotalRecords(data?.totalRegistros ?? 0);
    } catch (error) {
      toast.error("Erro ao carregar os produtos.");
      setAllProducts([]);
      setTotalRecords(0);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (!isAuthenticated) {
      setAllProducts([]);
      setTotalRecords(0);
      setIsLoading(false);
      return;
    }

    fetchProducts(getStatusParam(statusFilter), currentPage);
  }, [
    currentPage,
    statusFilter,
    fetchProducts,
    getStatusParam,
    isAuthenticated,
  ]);

  const closeProductFormModal = () => {
    setProductFormMode(null);
    setSelectedProduct(null);
    setProduct(INITIAL_PRODUCT_STATE);
    setErrors({});
  };

  const closeConfirmModal = () => {
    setConfirmAction(null);
    setSelectedProduct(null);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    setProduct((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleStatusFilterChange = (event) => {
    setStatusFilter(event.target.value);
    setCurrentPage(1);
  };

  const handleClickAddProduct = () => {
    setSelectedProduct(null);
    setProduct(INITIAL_PRODUCT_STATE);
    setProductFormMode("create");
    setErrors({});
  };

  const handleClickEdit = (currentProduct) => {
    setSelectedProduct(currentProduct);
    setProduct({
      id: currentProduct.id,
      name: currentProduct.name ?? "",
      description: currentProduct.description ?? "",
      price:
        currentProduct.price !== undefined && currentProduct.price !== null
          ? String(currentProduct.price)
          : "",
    });
    setProductFormMode("edit");
    setErrors({});
  };

  const handleClickDelete = (currentProduct) => {
    setSelectedProduct(currentProduct);
    setConfirmAction("delete");
  };

  const handleClickActive = (currentProduct) => {
    setSelectedProduct(currentProduct);
    setConfirmAction("activate");
  };

  const handleClickDesactive = (currentProduct) => {
    setSelectedProduct(currentProduct);
    setConfirmAction("deactivate");
  };

  const handleSubmitAdd = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }

    setIsSaving(true);
    try {
      await productAPI.Create(buildProductPayload(product));
      toast.success("Produto criado com sucesso.");
      closeProductFormModal();
      fetchProducts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao criar produto.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleSubmitEdit = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }

    setIsSaving(true);
    try {
      await productAPI.Update(buildProductPayload(product));
      toast.success("Produto atualizado com sucesso.");
      closeProductFormModal();
      fetchProducts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao editar produto.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleProductFormSubmit = (e) => {
    if (isSaving) return;

    if (isEditing) {
      handleSubmitEdit(e);
      return;
    }

    handleSubmitAdd(e);
  };

  const handleDeleteProduct = async () => {
    if (!selectedProduct?.id) return;

    setIsSaving(true);
    try {
      await productAPI.Delete(selectedProduct.id);
      toast.success("Produto excluído com sucesso.");
      closeConfirmModal();
      fetchProducts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao excluir o produto.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleActivateProduct = async () => {
    if (!selectedProduct?.id) return;

    setIsSaving(true);
    try {
      await productAPI.Activate(selectedProduct.id);
      toast.success("Produto ativado com sucesso.");
      closeConfirmModal();
      fetchProducts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao ativar o produto.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeactivateProduct = async () => {
    if (!selectedProduct?.id) return;

    setIsSaving(true);
    try {
      await productAPI.Deactivate(selectedProduct.id);
      toast.success("Produto inativado com sucesso.");
      closeConfirmModal();
      fetchProducts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao inativar o produto.");
    } finally {
      setIsSaving(false);
    }
  };

  const CONFIRM_MODAL_CONFIG = {
    delete: {
      title: "Confirmar exclusão",
      body: `Tem certeza que deseja excluir o produto "${selectedProduct?.name}"?`,
      variant: "danger",
      label: "Excluir",
      onConfirm: handleDeleteProduct,
    },
    activate: {
      title: "Confirmar ativação",
      body: `Tem certeza que deseja ativar o produto "${selectedProduct?.name}"?`,
      variant: "success",
      label: "Ativar",
      onConfirm: handleActivateProduct,
    },
    deactivate: {
      title: "Confirmar inativação",
      body: `Tem certeza que deseja inativar o produto "${selectedProduct?.name}"?`,
      variant: "warning",
      label: "Inativar",
      onConfirm: handleDeactivateProduct,
    },
  };

  const currentConfirmConfig = CONFIRM_MODAL_CONFIG[confirmAction] ?? null;

  return (
    <Sidebar>
      <>
        <div className={style["pagina-listagem"]}>
          <ListingHeader
            title="Produtos"
            description="Gerencie os produtos cadastrados no CRM."
            buttonLabel="+ Novo Produto"
            onButtonClick={handleClickAddProduct}
            selectLabel="Status"
            selectOptions={[
              { label: "Todos", value: "all" },
              { label: "Ativo", value: "active" },
              { label: "Inativo", value: "inactive" },
            ]}
            selectValue={statusFilter}
            onSelectChange={handleStatusFilterChange}
          />
          <section className={style["tabela-card"]}>
            <table className={style.tabela}>
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>Descrição</th>
                  <th>Preço</th>
                  <th>Status</th>
                  <th className={style["coluna-acoes"]}></th>
                </tr>
              </thead>

              <tbody>
                {isLoading ? (
                  <tr>
                    <td colSpan={5} className={style["celula-carregando"]}>
                      Carregando...
                    </td>
                  </tr>
                ) : allProducts.length === 0 ? (
                  <tr>
                    <td colSpan={5} className={style["celula-carregando"]}>
                      Nenhum produto encontrado.
                    </td>
                  </tr>
                ) : (
                  allProducts.map((listedProduct) => (
                    <tr key={listedProduct.id}>
                      <td>{listedProduct.name}</td>
                      <td>{listedProduct.description}</td>
                      <td>{formatCurrency(listedProduct.price)}</td>
                      <td>
                        <span
                          className={`${style.badge} ${
                            listedProduct.isActive ? style.ativo : style.inativo
                          }`}
                        >
                          {listedProduct.isActive ? "Ativo" : "Inativo"}
                        </span>
                      </td>
                      <td className={style.acoes}>
                        {listedProduct.isActive ? (
                          <button
                            className={style["botao-desativar"]}
                            onClick={() => handleClickDesactive(listedProduct)}
                            title="Inativar"
                          >
                            <MdBlock />
                          </button>
                        ) : (
                          <button
                            className={style["botao-ativar"]}
                            onClick={() => handleClickActive(listedProduct)}
                            title="Ativar"
                          >
                            <MdCheckCircleOutline />
                          </button>
                        )}

                        <button
                          className={style["botao-editar"]}
                          onClick={() => handleClickEdit(listedProduct)}
                          title="Editar"
                        >
                          <MdEdit />
                        </button>

                        <button
                          className={style["botao-excluir"]}
                          onClick={() => handleClickDelete(listedProduct)}
                          title="Excluir"
                        >
                          <MdDelete />
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>

            <footer className={style.paginacao}>
              <span>Total de registros: {totalRecords}</span>

              <div className={style["paginacao-acoes"]}>
                <span>
                  Página {currentPage} de {totalPages}
                </span>
                <button
                  onClick={() => setCurrentPage((page) => page - 1)}
                  disabled={currentPage === 1}
                >
                  <MdChevronLeft />
                </button>

                {getPageNumbers(currentPage).map((page) => (
                  <button
                    key={page}
                    onClick={() => setCurrentPage(page)}
                    className={
                      page === currentPage ? style["pagina-ativa"] : undefined
                    }
                  >
                    {page}
                  </button>
                ))}

                <button
                  onClick={() => setCurrentPage((page) => page + 1)}
                  disabled={currentPage === totalPages}
                >
                  <MdChevronRight />
                </button>
              </div>
            </footer>
          </section>

          <ToastContainer position="top-right" autoClose={3000} />
        </div>

        <Modal show={confirmAction !== null} onHide={closeConfirmModal}>
          <Modal.Header closeButton>
            <Modal.Title>{currentConfirmConfig?.title}</Modal.Title>
          </Modal.Header>

          <Modal.Body>{currentConfirmConfig?.body}</Modal.Body>

          <Modal.Footer>
            <Button
              variant="secondary"
              buttonLabel="Cancelar"
              onButtonClick={closeConfirmModal}
            />

            <Button
              variant={currentConfirmConfig?.variant}
              buttonLabel={currentConfirmConfig?.label}
              onButtonClick={currentConfirmConfig?.onConfirm}
              disabled={isSaving}
            />
          </Modal.Footer>
        </Modal>

        <Modal show={isProductFormOpen} onHide={closeProductFormModal}>
          <Modal.Header closeButton>
            <Modal.Title>{productFormTitle}</Modal.Title>
          </Modal.Header>

          <Modal.Body>
            <Form onSubmit={handleProductFormSubmit}>
              <Form.Group className="mb-3">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  name="name"
                  placeholder="Digite o nome do produto"
                  value={product.name}
                  onChange={(e) => {
                    handleInputChange(e);

                    if (errors?.name) {
                      setErrors((prev) => ({
                        ...prev,
                        name: false,
                      }));
                    }
                  }}
                  isInvalid={errors?.name}
                />
                <Form.Control.Feedback type="invalid">
                  Nome é obrigatório.
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Descrição</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={3}
                  name="description"
                  placeholder="Digite a descrição do produto"
                  value={product.description}
                  onChange={(e) => {
                    handleInputChange(e);

                    if (errors?.description) {
                      setErrors((prev) => ({
                        ...prev,
                        description: false,
                      }));
                    }
                  }}
                  isInvalid={errors?.description}
                />
                <Form.Control.Feedback type="invalid">
                  Descrição é obrigatória.
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Preço</Form.Label>
                <Form.Control
                  type="number"
                  min="0"
                  step="0.01"
                  name="price"
                  placeholder="Digite o preço do produto"
                  value={product.price}
                  onChange={(e) => {
                    handleInputChange(e);

                    if (errors?.price) {
                      setErrors((prev) => ({
                        ...prev,
                        price: false,
                      }));
                    }
                  }}
                  isInvalid={errors?.price}
                />
                <Form.Control.Feedback type="invalid">
                  Informe um preço válido.
                </Form.Control.Feedback>
              </Form.Group>

              <Modal.Footer>
                <Button
                  variant="secondary"
                  buttonLabel="Cancelar"
                  onButtonClick={closeProductFormModal}
                />

                <Button
                  variant={isEditing ? "warning" : "success"}
                  type="submit"
                  buttonLabel={isEditing ? "Atualizar" : "Salvar"}
                  disabled={isSaving}
                />
              </Modal.Footer>
            </Form>
          </Modal.Body>
        </Modal>
      </>
    </Sidebar>
  );
}
