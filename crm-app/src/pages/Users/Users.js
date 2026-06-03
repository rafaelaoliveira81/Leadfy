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
import userApi from "../../services/userApi";

import GetPageNumbers from "../../utils/Pagination";

import style from "./_users.module.css";

const ITEMS_PER_PAGE = 10;

const INITIAL_USER_STATE = {
  id: null,
  name: "",
  email: "",
  password: "",
};

export function Users() {
  const { isAuthenticated } = useAuth();
  const [allUsers, setAllUsers] = useState([]);
  const [totalRecords, setTotalRecords] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState("active");

  const [user, setUser] = useState(INITIAL_USER_STATE);
  const [selectedUser, setSelectedUser] = useState(null);

  const [confirmAction, setConfirmAction] = useState(null);
  const [userFormMode, setUserFormMode] = useState(null);

  const totalPages = Math.max(1, Math.ceil(totalRecords / ITEMS_PER_PAGE));

  const isUserFormOpen = userFormMode !== null;
  const isEditing = userFormMode === "edit";
  const userFormTitle = isEditing ? "Editar Usuário" : "Novo Usuário";

  const isFormValid = () => {
    const newErrors = {};

    if (!user.name?.trim()) {
      newErrors.name = true;
    }

    if (!user.email?.trim()) {
      newErrors.email = true;
    }

    if (!isEditing && !user.password?.trim()) {
      newErrors.password = true;
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

  const fetchUsers = useCallback(async (status, page) => {
    setIsLoading(true);

    try {
      const data = await userApi.GetPaged({
        isActive: status,
        pagina: page,
        quantidadePorPagina: ITEMS_PER_PAGE,
      });

      setAllUsers(Array.isArray(data?.dados) ? data.dados : []);
      setTotalRecords(data?.totalRegistros ?? 0);
    } catch (error) {
      toast.error("Erro ao carregar os usuários.");
      setAllUsers([]);
      setTotalRecords(0);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (!isAuthenticated) {
      setAllUsers([]);
      setTotalRecords(0);
      setIsLoading(false);
      return;
    }

    fetchUsers(getStatusParam(statusFilter), currentPage);
  }, [currentPage, statusFilter, fetchUsers, getStatusParam, isAuthenticated]);

  const closeUserFormModal = () => {
    setUserFormMode(null);
    setSelectedUser(null);
    setUser(INITIAL_USER_STATE);
    setErrors({});
  };

  const closeConfirmModal = () => {
    setConfirmAction(null);
    setSelectedUser(null);
  };

  const buildUserPayload = (currentUser, includePassword = false) => {
    const payload = {
      name: currentUser.name?.trim() ?? "",
      email: currentUser.email?.trim() ?? "",
    };

    if (includePassword) {
      payload.password = currentUser.password?.trim() ?? "";
    }

    return payload;
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    setUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleStatusFilterChange = (event) => {
    setStatusFilter(event.target.value);
    setCurrentPage(1);
  };

  const handleClickAdd = () => {
    setSelectedUser(null);
    setUser(INITIAL_USER_STATE);
    setUserFormMode("create");
    setErrors({});
  };

  const handleClickEdit = (user) => {
    setSelectedUser(user);
    setUser({
      id: user.id,
      name: user.name ?? "",
      email: user.email ?? "",
      password: "",
    });
    setUserFormMode("edit");
    setErrors({});
  };

  const handleClickDelete = (user) => {
    setSelectedUser(user);
    setConfirmAction("delete");
  };

  const handleClickActive = (user) => {
    setSelectedUser(user);
    setConfirmAction("activate");
  };

  const handleClickDesactive = (user) => {
    setSelectedUser(user);
    setConfirmAction("deactivate");
  };

  const handleSubmitAdd = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }
    setIsSaving(true);
    try {
      await userApi.Create(buildUserPayload(user, true));
      toast.success("Usuário criado com sucesso.");
      closeUserFormModal();
      fetchUsers(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error("Erro ao criar usuário.");
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
      await userApi.Update(user.id, buildUserPayload(user));
      toast.success("Usuário atualizado com sucesso.");
      closeUserFormModal();
      fetchUsers(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error("Erro ao editar usuário.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleUserFormSubmit = (e) => {
    if (isSaving) return;
    if (isEditing) {
      handleSubmitEdit(e);
      return;
    }

    handleSubmitAdd(e);
  };

  const handleDeleteUser = async () => {
    if (!selectedUser?.id) return;
    setIsSaving(true);
    try {
      await userApi.Delete(selectedUser.id);
      toast.success("Usuário excluído com sucesso.");
      closeConfirmModal();
      fetchUsers(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error("Erro ao excluir o usuário.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleActivateUser = async () => {
    if (!selectedUser?.id) return;
    setIsSaving(true);
    try {
      await userApi.Activate(selectedUser.id);
      toast.success("Usuário ativado com sucesso.");
      closeConfirmModal();
      fetchUsers(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error("Erro ao ativar o usuário.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeactivateUser = async () => {
    if (!selectedUser?.id) return;
    setIsSaving(true);
    try {
      await userApi.Deactivate(selectedUser.id);
      toast.success("Usuário inativado com sucesso.");
      closeConfirmModal();
      fetchUsers(getStatusParam(statusFilter), currentPage);
    } catch (error) {
      toast.error("Erro ao inativar o usuário.");
    } finally {
      setIsSaving(false);
    }
  };

  const CONFIRM_MODAL_CONFIG = {
    delete: {
      title: "Confirmar exclusão",
      body: `Tem certeza que deseja excluir o usuário "${selectedUser?.name}"?`,
      variant: "danger",
      label: "Excluir",
      onConfirm: handleDeleteUser,
    },
    activate: {
      title: "Confirmar ativação",
      body: `Tem certeza que deseja ativar o usuário "${selectedUser?.name}"?`,
      variant: "success",
      label: "Ativar",
      onConfirm: handleActivateUser,
    },
    deactivate: {
      title: "Confirmar inativação",
      body: `Tem certeza que deseja inativar o usuário "${selectedUser?.name}"?`,
      variant: "warning",
      label: "Inativar",
      onConfirm: handleDeactivateUser,
    },
  };

  const currentConfirmConfig = CONFIRM_MODAL_CONFIG[confirmAction] ?? null;

  return (
    <Sidebar>
      <>
        <div className={style["pagina-listagem"]}>
          <ListingHeader
            title="Usuários"
            description="Gerencie os usuários cadastrados no CRM."
            buttonLabel="+ Novo Usuário"
            onButtonClick={handleClickAdd}
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
                  <th>Nome</th>
                  <th>E-mail</th>
                  <th>Status</th>
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
                ) : allUsers.length === 0 ? (
                  <tr>
                    <td colSpan={4} className={style["celula-carregando"]}>
                      Nenhum usuário encontrado.
                    </td>
                  </tr>
                ) : (
                  allUsers.map((listedUser) => (
                    <tr key={listedUser.id}>
                      <td>{listedUser.name}</td>
                      <td>{listedUser.email}</td>
                      <td>
                        <span
                          className={`${style.badge} ${
                            listedUser.isActive ? style.ativo : style.inativo
                          }`}
                        >
                          {listedUser.isActive ? "Ativo" : "Inativo"}
                        </span>
                      </td>
                      <td className={style["acoes"]}>
                        {listedUser.isActive ? (
                          <button
                            className={style["botao-desativar"]}
                            onClick={() => handleClickDesactive(listedUser)}
                            title="Inativar"
                          >
                            <MdBlock />
                          </button>
                        ) : (
                          <button
                            className={style["botao-ativar"]}
                            onClick={() => handleClickActive(listedUser)}
                            title="Ativar"
                          >
                            <MdCheckCircleOutline />
                          </button>
                        )}

                        <button
                          className={style["botao-editar"]}
                          onClick={() => handleClickEdit(listedUser)}
                          title="Editar"
                        >
                          <MdEdit />
                        </button>

                        <button
                          className={style["botao-excluir"]}
                          onClick={() => handleClickDelete(listedUser)}
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

          <ToastContainer />
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

        <Modal show={isUserFormOpen} onHide={closeUserFormModal}>
          <Modal.Header closeButton>
            <Modal.Title>{userFormTitle}</Modal.Title>
          </Modal.Header>

          <Modal.Body>
            <Form onSubmit={handleUserFormSubmit}>
              <Form.Group className="mb-3">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  name="name"
                  placeholder="Digite o nome"
                  value={user.name}
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
                  value={user.email}
                  onChange={(e) => {
                    handleInputChange(e);

                    if (errors?.email) {
                      setErrors((prev) => ({
                        ...prev,
                        email: false,
                      }));
                    }
                  }}
                  isInvalid={errors?.email}
                />
                <Form.Control.Feedback type="invalid">
                  Email é obrigatório.
                </Form.Control.Feedback>
              </Form.Group>

              {!isEditing && (
                <Form.Group className="mb-3">
                  <Form.Label>Senha</Form.Label>
                  <Form.Control
                    type="password"
                    name="password"
                    placeholder="Digite a senha"
                    value={user.password}
                    onChange={(e) => {
                      handleInputChange(e);

                      if (errors?.password) {
                        setErrors((prev) => ({
                          ...prev,
                          password: false,
                        }));
                      }
                    }}
                    isInvalid={errors?.password}
                  />
                  <Form.Control.Feedback type="invalid">
                    Senha é obrigatória.
                  </Form.Control.Feedback>
                </Form.Group>
              )}

              <Modal.Footer>
                <Button
                  variant="secondary"
                  buttonLabel="Cancelar"
                  onButtonClick={closeUserFormModal}
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
