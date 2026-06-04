import { useState, useEffect, useCallback } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Modal from "react-bootstrap/Modal";
import Spinner from "react-bootstrap/Spinner";
import Form from "react-bootstrap/Form";
import {
  MdEdit,
  MdDelete,
  MdChevronLeft,
  MdChevronRight,
  MdBlock,
  MdCheckCircleOutline,
} from "react-icons/md";
import { BsStars } from "react-icons/bs";
import { Button } from "../../components/Button/Button";
import { Sidebar } from "../../components/Sidebar/Sidebar";

import { ListingHeader } from "../../components/ListingHeader/ListingHeader";
import { useAuth } from "../../context/AuthContext";
import { promptAPI } from "../../services/promptApi";

import GetPageNumbers from "../../utils/Pagination";

import style from "./_prompt.module.css";

const ITEMS_PER_PAGE = 10;

const INITIAL_PROMPT_STATE = {
  id: null,
  title: "",
  content: "",
};

export function Prompts() {
  const { isAuthenticated } = useAuth();
  const [allPrompts, setAllPrompts] = useState([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isOptimizing, setIsOptimizing] = useState(false);
  const [errors, setErrors] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState("active");

  const [prompt, setPrompt] = useState(INITIAL_PROMPT_STATE);
  const [selectedPrompt, setSelectedPrompt] = useState(null);

  const [confirmAction, setConfirmAction] = useState(null);
  const [promptFormMode, setPromptFormMode] = useState(null);

  const totalPages = Math.max(1, Math.ceil(totalRecords / ITEMS_PER_PAGE));

  const isPromptFormOpen = promptFormMode !== null;
  const isEditing = promptFormMode === "edit";
  const promptFormTitle = isEditing ? "Editar Prompt" : "Novo Prompt";

  const isFormValid = () => {
    const newErrors = {};

    if (!prompt.title?.trim()) {
      newErrors.title = true;
    }

    if (!prompt.content?.trim()) {
      newErrors.content = true;
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

  const fetchPrompts = useCallback(async (status, page) => {
    setIsLoading(true);

    try {
      const data = await promptAPI.GetPaged({
        isActive: status,
        pagina: page,
        quantidadePorPagina: ITEMS_PER_PAGE,
      });

      setAllPrompts(Array.isArray(data?.dados) ? data.dados : []);
      setTotalRecords(data?.totalRegistros ?? 0);
    } catch (error) {
      toast.error("Erro ao carregar os prompts.");
      setAllPrompts([]);
      setTotalRecords(0);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (!isAuthenticated) {
      setAllPrompts([]);
      setTotalRecords(0);
      setIsLoading(false);
      return;
    }

    fetchPrompts(getStatusParam(statusFilter), currentPage);
  }, [
    currentPage,
    statusFilter,
    fetchPrompts,
    getStatusParam,
    isAuthenticated,
  ]);

  const closePromptFormModal = () => {
    setPromptFormMode(null);
    setSelectedPrompt(null);
    setPrompt(INITIAL_PROMPT_STATE);
    setErrors({});
  };

  const closeConfirmModal = () => {
    setConfirmAction(null);
    setSelectedPrompt(null);
  };

  const buildPromptPayload = (currentPrompt) => ({
    id: currentPrompt.id ?? 0,
    title: currentPrompt.title?.trim() ?? "",
    content: currentPrompt.content?.trim() ?? "",
  });

  const buildPromptOptimizePayload = (currentPrompt) => ({
    title: currentPrompt.title?.trim() ?? "",
    content: currentPrompt.content?.trim() ?? "",
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    setPrompt((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleStatusFilterChange = (event) => {
    setStatusFilter(event.target.value);
    setCurrentPage(1);
  };

  const handleClickOptimizePrompt = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }

    setIsOptimizing(true);

    try {
      const response = await promptAPI.OptimizePrompt(
        buildPromptOptimizePayload(prompt),
      );

      if (!response?.optimizedPrompt?.content.trim()) {
        throw new Error("A API não retornou um prompt otimizado.");
      }

      setPrompt((prev) => ({
        ...prev,
        content: response.optimizedPrompt.content.trim(),
      }));

      toast.success("Prompt otimizado com sucesso.");
    } catch (error) {
      toast.error(error?.message || "Erro ao otimizar prompt.");
    } finally {
      setIsOptimizing(false);
    }
  };

  const handleClickAddPrompt = () => {
    setSelectedPrompt(null);
    setPrompt(INITIAL_PROMPT_STATE);
    setPromptFormMode("create");
    setErrors({});
  };

  const handleClickEdit = (currentPrompt) => {
    setSelectedPrompt(currentPrompt);
    setPrompt({
      id: currentPrompt.id,
      title: currentPrompt.title ?? "",
      content: currentPrompt.content ?? "",
    });
    setPromptFormMode("edit");
    setErrors({});
  };

  const handleClickDelete = (currentPrompt) => {
    setSelectedPrompt(currentPrompt);
    setConfirmAction("delete");
  };

  const handleClickActive = (currentPrompt) => {
    setSelectedPrompt(currentPrompt);
    setConfirmAction("activate");
  };

  const handleClickDesactive = (currentPrompt) => {
    setSelectedPrompt(currentPrompt);
    setConfirmAction("deactivate");
  };

  const handleSubmitAdd = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }
    setIsSaving(true);
    try {
      await promptAPI.Create(buildPromptPayload(prompt));
      toast.success("Prompt criado com sucesso.");
      closePromptFormModal();
      fetchPrompts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao criar prompt.");
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
      await promptAPI.Update(buildPromptPayload(prompt));
      toast.success("Prompt atualizado com sucesso.");
      closePromptFormModal();
      fetchPrompts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao editar prompt.");
    } finally {
      setIsSaving(false);
    }
  };

  const handlePromptFormSubmit = (e) => {
    if (isSaving) return;
    if (isEditing) {
      handleSubmitEdit(e);
      return;
    }

    handleSubmitAdd(e);
  };

  const handleDeletePrompt = async () => {
    if (!selectedPrompt?.id) return;
    setIsSaving(true);
    try {
      await promptAPI.Delete(selectedPrompt.id);
      toast.success("Prompt excluído com sucesso.");
      closeConfirmModal();
      fetchPrompts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao excluir o prompt.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleActivatePrompt = async () => {
    if (!selectedPrompt?.id) return;
    setIsSaving(true);
    try {
      await promptAPI.Activate(selectedPrompt.id);
      toast.success("Prompt ativado com sucesso.");
      closeConfirmModal();
      fetchPrompts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao ativar o prompt.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeactivatePrompt = async () => {
    if (!selectedPrompt?.id) return;
    setIsSaving(true);
    try {
      await promptAPI.Deactivate(selectedPrompt.id);
      toast.success("Prompt inativado com sucesso.");
      closeConfirmModal();
      fetchPrompts(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error(error?.message || "Erro ao inativar o prompt.");
    } finally {
      setIsSaving(false);
    }
  };

  const CONFIRM_MODAL_CONFIG = {
    delete: {
      title: "Confirmar exclusão",
      body: `Tem certeza que deseja excluir o prompt "${selectedPrompt?.title}"?`,
      variant: "danger",
      label: "Excluir",
      onConfirm: handleDeletePrompt,
    },
    activate: {
      title: "Confirmar ativação",
      body: `Tem certeza que deseja ativar o prompt "${selectedPrompt?.title}"?`,
      variant: "success",
      label: "Ativar",
      onConfirm: handleActivatePrompt,
    },
    deactivate: {
      title: "Confirmar inativação",
      body: `Tem certeza que deseja inativar o prompt "${selectedPrompt?.title}"?`,
      variant: "warning",
      label: "Inativar",
      onConfirm: handleDeactivatePrompt,
    },
  };

  const currentConfirmConfig = CONFIRM_MODAL_CONFIG[confirmAction] ?? null;

  return (
    <Sidebar>
      <>
        <div className={style["pagina-listagem"]}>
          <ListingHeader
            title="Prompts"
            description="Gerencie os prompts cadastrados no CRM."
            buttonLabel="+ Novo"
            onButtonClick={handleClickAddPrompt}
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
            <table className={style["tabela"]}>
              <thead>
                <tr>
                  <th>Título</th>
                  <th>Prompt</th>
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
                ) : allPrompts.length === 0 ? (
                  <tr>
                    <td colSpan={5} className={style["celula-carregando"]}>
                      Nenhum prompt encontrado.
                    </td>
                  </tr>
                ) : (
                  allPrompts.map((listedPrompt) => (
                    <tr key={listedPrompt.id}>
                      <td>{listedPrompt.title}</td>
                      <td>
                        {listedPrompt.content?.length > 50
                          ? `${listedPrompt.content.slice(0, 50)}...`
                          : listedPrompt.content}
                      </td>
                      <td>
                        <span
                          className={`${style.badge} ${
                            listedPrompt.isActive ? style.ativo : style.inativo
                          }`}
                        >
                          {listedPrompt.isActive ? "Ativo" : "Inativo"}
                        </span>
                      </td>
                      <td className={style["acoes"]}>
                        {listedPrompt.isActive ? (
                          <button
                            className={style["botao-desativar"]}
                            onClick={() => handleClickDesactive(listedPrompt)}
                            title="Inativar"
                          >
                            <MdBlock />
                          </button>
                        ) : (
                          <button
                            className={style["botao-ativar"]}
                            onClick={() => handleClickActive(listedPrompt)}
                            title="Ativar"
                          >
                            <MdCheckCircleOutline />
                          </button>
                        )}

                        <button
                          className={style["botao-editar"]}
                          onClick={() => handleClickEdit(listedPrompt)}
                          title="Editar"
                        >
                          <MdEdit />
                        </button>

                        <button
                          className={style["botao-excluir"]}
                          onClick={() => handleClickDelete(listedPrompt)}
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

            <footer className={style["paginacao"]}>
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

        <Modal show={isPromptFormOpen} onHide={closePromptFormModal}>
          <Modal.Header closeButton>
            <Modal.Title>{promptFormTitle}</Modal.Title>
          </Modal.Header>

          <Modal.Body>
            <Form onSubmit={handlePromptFormSubmit}>
              <Form.Group className="mb-3">
                <Form.Label>Título</Form.Label>
                <Form.Control
                  type="text"
                  name="title"
                  placeholder="Digite o título"
                  value={prompt.title}
                  onChange={(e) => {
                    handleInputChange(e);

                    if (errors?.title) {
                      setErrors((prev) => ({
                        ...prev,
                        title: false,
                      }));
                    }
                  }}
                  isInvalid={errors?.title}
                />
                <Form.Control.Feedback type="invalid">
                  Título é obrigatório.
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group className="position-relative">
                <div className={style["textarea-wrapper"]}>
                  <Form.Label>Prompt</Form.Label>
                  <Form.Control
                    as="textarea"
                    rows={6}
                    className={style["textarea-field"]}
                    name="content"
                    placeholder="Digite o conteúdo do prompt"
                    value={prompt.content}
                    onChange={(e) => {
                      handleInputChange(e);

                      if (errors?.content) {
                        setErrors((prev) => ({
                          ...prev,
                          content: false,
                        }));
                      }
                    }}
                    isInvalid={errors?.content}
                  />
                  <div className={style["paginacao-acoes"]}>
                    <button
                      type="button"
                      className={style.ia}
                      onClick={handleClickOptimizePrompt}
                      disabled={isOptimizing}
                    >
                      {isOptimizing ? (
                        <Spinner animation="border" role="status" size="sm" />
                      ) : (
                        <BsStars />
                      )}
                    </button>
                  </div>
                </div>

                <Form.Control.Feedback type="invalid">
                  Conteúdo é obrigatório.
                </Form.Control.Feedback>
              </Form.Group>

              <Modal.Footer>
                <Button
                  variant="secondary"
                  buttonLabel="Cancelar"
                  onButtonClick={closePromptFormModal}
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
