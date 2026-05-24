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
import { Topbar } from "../../components/Topbar/Topbar";
import { ListingHeader } from "../../components/ListingHeader/ListingHeader";
import { leadAPI } from "../../services/leadApi";
import style from "./_leads.module.css";

const ITEMS_PER_PAGE = 10;

const INITIAL_LEAD_STATE = {
  id: null,
  name: "",
  email: "",
  phoneNumber: "",
};

const formatPhoneNumber = (value) => {
  const digits = value.replace(/\D/g, "").slice(0, 11);

  if (!digits) {
    return "";
  }

  if (digits.length <= 2) {
    return `(${digits}`;
  }

  if (digits.length <= 7) {
    return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
  }

  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`;
};

const normalizeLeadForApi = (lead) => ({
  ...lead,
  phoneNumber: (lead.phoneNumber || "").replace(/\D/g, ""),
});

export function Leads() {
  const [allLeads, setAllLeads] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errors, setErrors] = useState({});
  const [currentPage, setCurrentPage] = useState(1);

  const [lead, setLead] = useState(INITIAL_LEAD_STATE);
  const [selectedLead, setSelectedLead] = useState(null);

  const [confirmAction, setConfirmAction] = useState(null);
  const [leadFormMode, setLeadFormMode] = useState(null);

  const totalPages = Math.max(1, Math.ceil(allLeads.length / ITEMS_PER_PAGE));

  const paginatedLeads = allLeads.slice(
    (currentPage - 1) * ITEMS_PER_PAGE,
    currentPage * ITEMS_PER_PAGE,
  );

  const isLeadFormOpen = leadFormMode !== null;
  const isEditing = leadFormMode === "edit";
  const leadFormTitle = isEditing ? "Edição de Leads" : "Novo Lead";

  const isFormValid = () => {
    const newErrors = {};

    if (!lead.name?.trim()) {
      newErrors.name = true;
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const getPageNumbers = () => {
    const maxVisible = 5;
    let start = Math.max(1, currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(totalPages, start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    const pages = [];

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  };

  const fetchLeads = useCallback(async () => {
    setIsLoading(true);

    try {
      const data = await leadAPI.GetAll();
      setAllLeads(Array.isArray(data) ? data : []);
      setCurrentPage(1);
    } catch (error) {
      toast.error("Erro ao carregar os leads.");
      setAllLeads([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchLeads();
  }, [fetchLeads]);

  const closeLeadFormModal = () => {
    setLeadFormMode(null);
    setSelectedLead(null);
    setLead(INITIAL_LEAD_STATE);
  };

  const closeConfirmModal = () => {
    setConfirmAction(null);
    setSelectedLead(null);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    const formattedValue =
      name === "phoneNumber" ? formatPhoneNumber(value) : value;

    setLead((prev) => ({
      ...prev,
      [name]: formattedValue,
    }));
  };

  const handleClickAddLead = () => {
    setSelectedLead(null);
    setLead(INITIAL_LEAD_STATE);
    setLeadFormMode("create");
  };

  const handleClickEdit = (lead) => {
    setSelectedLead(lead);
    setLead({
      ...lead,
      phoneNumber: formatPhoneNumber(lead.phoneNumber || ""),
    });
    setLeadFormMode("edit");
  };

  const handleClickDelete = (lead) => {
    setSelectedLead(lead);
    setConfirmAction("delete");
  };

  const handleClickActive = (lead) => {
    setSelectedLead(lead);
    setConfirmAction("activate");
  };

  const handleClickDesactive = (lead) => {
    setSelectedLead(lead);
    setConfirmAction("deactivate");
  };

  const handleSubmitAdd = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }

    try {
      await leadAPI.Create(normalizeLeadForApi(lead));
      toast.success("Lead criado com sucesso.");
      closeLeadFormModal();
      fetchLeads();
    } catch (error) {
      toast.error("Erro ao criar lead.");
    }
  };

  const handleSubmitEdit = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }

    try {
      await leadAPI.Update(normalizeLeadForApi(lead));
      toast.success("Lead atualizado com sucesso.");
      closeLeadFormModal();
      fetchLeads();
    } catch (error) {
      toast.error("Erro ao editar lead.");
    }
  };

  const handleLeadFormSubmit = (e) => {
    if (isEditing) {
      handleSubmitEdit(e);
      return;
    }

    handleSubmitAdd(e);
  };

  const handleDeleteLead = async () => {
    if (!selectedLead?.id) return;
    try {
      await leadAPI.Delete(selectedLead.id);
      toast.success("Lead deletado com sucesso.");
      closeConfirmModal();
      fetchLeads();
    } catch (error) {
      toast.error("Erro ao deletar o lead.");
    }
  };

  const handleActivateLead = async () => {
    if (!selectedLead?.id) return;
    try {
      await leadAPI.Activate(selectedLead.id);
      toast.success("Lead ativado com sucesso.");
      closeConfirmModal();
      fetchLeads();
    } catch (error) {
      toast.error("Erro ao ativar o lead.");
    }
  };

  const handleDeactivateLead = async () => {
    if (!selectedLead?.id) return;
    try {
      await leadAPI.Deactivate(selectedLead.id);
      toast.success("Lead inativado com sucesso.");
      closeConfirmModal();
      fetchLeads();
    } catch (error) {
      toast.error("Erro ao inativar o lead.");
    }
  };

  const CONFIRM_MODAL_CONFIG = {
    delete: {
      title: "Confirmar exclusão",
      body: `Tem certeza que deseja deletar o lead "${selectedLead?.name}"?`,
      variant: "danger",
      label: "Deletar",
      onConfirm: handleDeleteLead,
    },
    activate: {
      title: "Confirmar ativação",
      body: `Tem certeza que deseja ativar o lead "${selectedLead?.name}"?`,
      variant: "success",
      label: "Ativar",
      onConfirm: handleActivateLead,
    },
    deactivate: {
      title: "Confirmar inativação",
      body: `Tem certeza que deseja inativar o lead "${selectedLead?.name}"?`,
      variant: "warning",
      label: "Inativar",
      onConfirm: handleDeactivateLead,
    },
  };

  const currentConfirmConfig = CONFIRM_MODAL_CONFIG[confirmAction] ?? null;

  return (
    <Sidebar>
      <Topbar>
        <div className={style["pagina-listagem"]}>
          <ListingHeader
            title="Leads"
            description="Gerencie os leads cadastrados no CRM."
            buttonLabel="+ Novo"
            onButtonClick={handleClickAddLead}
          />

          <section className={style["tabela-card"]}>
            <table className={style["tabela"]}>
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>E-mail</th>
                  <th>Telefone</th>
                  <th className={style["coluna-acoes"]}></th>
                </tr>
              </thead>

              <tbody>
                {isLoading ? (
                  <tr>
                    <td colSpan={4} className={style["celula-carregando"]}>
                      Carregando...
                    </td>
                  </tr>
                ) : (
                  paginatedLeads.map((lead) => (
                    <tr key={lead.id}>
                      <td>{lead.name}</td>
                      <td>{lead.email}</td>
                      <td>{formatPhoneNumber(lead.phoneNumber || "")}</td>
                      <td className={style["acoes"]}>
                        {lead.isActive ? (
                          <button
                            className={style["botao-desativar"]}
                            onClick={() => handleClickDesactive(lead)}
                            title="Inativar"
                          >
                            <MdBlock />
                          </button>
                        ) : (
                          <button
                            className={style["botao-ativar"]}
                            onClick={() => handleClickActive(lead)}
                            title="Ativar"
                          >
                            <MdCheckCircleOutline />
                          </button>
                        )}

                        <button
                          className={style["botao-editar"]}
                          onClick={() => handleClickEdit(lead)}
                          title="Editar"
                        >
                          <MdEdit />
                        </button>

                        <button
                          className={style["botao-excluir"]}
                          onClick={() => handleClickDelete(lead)}
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
              <span>
                Página {currentPage} de {totalPages}
              </span>

              <div className={style["paginacao-acoes"]}>
                <button
                  onClick={() => setCurrentPage((page) => page - 1)}
                  disabled={currentPage === 1}
                >
                  <MdChevronLeft />
                </button>

                {getPageNumbers().map((page) => (
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
            />
          </Modal.Footer>
        </Modal>

        <Modal show={isLeadFormOpen} onHide={closeLeadFormModal}>
          <Modal.Header closeButton>
            <Modal.Title>{leadFormTitle}</Modal.Title>
          </Modal.Header>

          <Modal.Body>
            <Form onSubmit={handleLeadFormSubmit}>
              <Form.Group className="mb-3">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  name="name"
                  placeholder="Digite o nome"
                  value={lead.name}
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
                <Form.Label>Email</Form.Label>
                <Form.Control
                  type="email"
                  name="email"
                  placeholder="Digite o email"
                  value={lead.email}
                  onChange={handleInputChange}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Telefone</Form.Label>
                <Form.Control
                  type="text"
                  name="phoneNumber"
                  placeholder="Digite o telefone"
                  value={lead.phoneNumber}
                  onChange={handleInputChange}
                  inputMode="numeric"
                  maxLength={15}
                />
              </Form.Group>

              <Modal.Footer>
                <Button
                  variant="secondary"
                  buttonLabel="Cancelar"
                  onButtonClick={closeLeadFormModal}
                />

                <Button
                  variant={isEditing ? "warning" : "success"}
                  type="submit"
                  buttonLabel={isEditing ? "Atualizar" : "Salvar"}
                />
              </Modal.Footer>
            </Form>
          </Modal.Body>
        </Modal>
      </Topbar>
    </Sidebar>
  );
}
