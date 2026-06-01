import { useState, useEffect, useCallback } from "react";
import { Badge, Card, Col, Form, Modal, Row } from "react-bootstrap";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { FaDollarSign } from "react-icons/fa";
import { MdExpandMore, MdInventory2 } from "react-icons/md";
import { BsCalendarCheck } from "react-icons/bs";

import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import opportunityAPI from "../../services/opportunityApi";
import { productAPI } from "../../services/productApi";
import interactionApi from "../../services/interactionApi";
import { useAuth } from "../../context/AuthContext";

import style from "./_kanban.module.css";
import { ListingHeader } from "../../components/ListingHeader/ListingHeader";
import { Button } from "../../components/Button/Button";

const STAGES = [
  { id: 1, label: "Novo Lead", headerClassName: "stage-blue" },
  { id: 2, label: "Em contato", headerClassName: "stage-cyan" },
  { id: 3, label: "Qualificado", headerClassName: "stage-purple" },
  { id: 4, label: "Proposta Enviada", headerClassName: "stage-orange" },
  { id: 5, label: "Negociação", headerClassName: "stage-yellow" },
  { id: 6, label: "Ganho", headerClassName: "stage-green" },
  { id: 7, label: "Perdido", headerClassName: "stage-red" },
];

function formatDate(dateString) {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleDateString("pt-BR");
}

function formatDateTime(dateString) {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleString("pt-BR");
}

function formatCurrencyInput(value) {
  const numericValue = Number(value || 0);

  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(numericValue);
}

function parseCurrencyInput(value) {
  const digitsOnly = value.replace(/\D/g, "");

  if (!digitsOnly) {
    return "";
  }

  return (Number(digitsOnly) / 100).toFixed(2);
}

function Kanban() {
  const { claims } = useAuth();
  const [board, setBoard] = useState({});
  const [isLoading, setIsLoading] = useState(true);
  const [products, setProducts] = useState([]);
  const [selectedOpportunity, setSelectedOpportunity] = useState(null);
  const [interactions, setInteractions] = useState([]);
  const [isInteractionsLoading, setIsInteractionsLoading] = useState(false);
  const [interactionDescription, setInteractionDescription] = useState("");
  const [isAddingInteraction, setIsAddingInteraction] = useState(false);
  const [isInteractionSectionOpen, setIsInteractionSectionOpen] =
    useState(false);
  const [opportunityForm, setOpportunityForm] = useState({
    amount: "",
    productId: "",
    expectedCloseDate: "",
  });
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const currentUserId = Number(claims?.usuarioId);

  const fetchBoard = useCallback(async () => {
    setIsLoading(true);
    try {
      const results = await Promise.all(
        STAGES.map((stage) => opportunityAPI.GetByStage(stage.id)),
      );
      const newBoard = {};
      STAGES.forEach((stage, index) => {
        newBoard[stage.id] = Array.isArray(results[index])
          ? results[index]
          : [];
      });
      setBoard(newBoard);
    } catch {
      toast.error("Erro ao carregar o board.");
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchBoard();
  }, [fetchBoard]);

  useEffect(() => {
    async function fetchProducts() {
      try {
        const data = await productAPI.GetPaged(true, 1, 1000);
        setProducts(Array.isArray(data?.dados) ? data.dados : []);
      } catch {
        setProducts([]);
        toast.error("Erro ao carregar os produtos.");
      }
    }

    fetchProducts();
  }, []);

  const fetchInteractions = useCallback(async (opportunityId) => {
    if (!opportunityId) {
      setInteractions([]);
      return;
    }

    setIsInteractionsLoading(true);

    try {
      const data = await interactionApi.GetByOpportunityId(opportunityId);
      setInteractions(Array.isArray(data) ? data : []);
    } catch {
      setInteractions([]);
      toast.error("Erro ao carregar o histórico de interações.");
    } finally {
      setIsInteractionsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (!isModalOpen || !selectedOpportunity?.id) {
      setInteractions([]);
      setInteractionDescription("");
      setIsInteractionsLoading(false);
      return;
    }

    fetchInteractions(selectedOpportunity.id);
  }, [fetchInteractions, isModalOpen, selectedOpportunity]);

  async function handleDragEnd(result) {
    const { source, destination } = result;

    if (!destination) return;
    if (
      source.droppableId === destination.droppableId &&
      source.index === destination.index
    )
      return;

    const sourceStageId = parseInt(source.droppableId);
    const destStageId = parseInt(destination.droppableId);

    const sourceItems = [...(board[sourceStageId] || [])];
    const destItems =
      sourceStageId === destStageId
        ? sourceItems
        : [...(board[destStageId] || [])];

    const [movedCard] = sourceItems.splice(source.index, 1);

    if (sourceStageId === destStageId) {
      sourceItems.splice(destination.index, 0, movedCard);
      setBoard((prev) => ({ ...prev, [sourceStageId]: sourceItems }));
      return;
    }

    const savedBoard = { ...board };

    destItems.splice(destination.index, 0, {
      ...movedCard,
      stage: destStageId,
    });
    setBoard((prev) => ({
      ...prev,
      [sourceStageId]: sourceItems,
      [destStageId]: destItems,
    }));

    try {
      await opportunityAPI.PatchStage(movedCard.id, destStageId);
    } catch {
      setBoard(savedBoard);
      toast.error("Erro ao mover a oportunidade. Tente novamente.");
    }
  }

  function handleCardClick(opportunity) {
    setSelectedOpportunity(opportunity);
    setIsInteractionSectionOpen(false);
    setOpportunityForm({
      amount: String(opportunity.amount ?? ""),
      productId: opportunity.productId ? String(opportunity.productId) : "",
      expectedCloseDate: opportunity.expectedCloseDate?.split("T")[0] || "",
    });
    setIsModalOpen(true);
  }

  function handleCloseModal() {
    setIsModalOpen(false);
    setSelectedOpportunity(null);
    setInteractions([]);
    setInteractionDescription("");
    setIsInteractionsLoading(false);
    setIsAddingInteraction(false);
    setIsInteractionSectionOpen(false);
    setOpportunityForm({ amount: "", productId: "", expectedCloseDate: "" });
    setIsSaving(false);
  }

  function handleOpportunityFieldChange(field, value) {
    setOpportunityForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  }

  function handleProductChange(productId) {
    const selectedProduct = products.find(
      (product) => String(product.id) === productId,
    );

    setOpportunityForm((prev) => ({
      ...prev,
      productId,
      amount:
        selectedProduct?.price !== undefined && selectedProduct?.price !== null
          ? String(selectedProduct.price)
          : "",
    }));
  }

  async function handleSaveOpportunity() {
    if (!selectedOpportunity) return;

    const amount = Number(opportunityForm.amount);

    if (Number.isNaN(amount) || amount <= 0) {
      toast.error("Informe um valor maior que zero.");
      return;
    }

    const payload = {
      leadId: selectedOpportunity.leadId,
      productId: opportunityForm.productId
        ? Number(opportunityForm.productId)
        : null,
      stage: selectedOpportunity.stage,
      amount,
      expectedCloseDate: opportunityForm.expectedCloseDate || null,
    };

    setIsSaving(true);

    try {
      await opportunityAPI.Update(selectedOpportunity.id, payload);

      const selectedProduct = products.find(
        (product) => product.id === payload.productId,
      );

      const updatedOpportunity = {
        ...selectedOpportunity,
        amount,
        productId: payload.productId,
        productName: selectedProduct?.name || "",
        expectedCloseDate: payload.expectedCloseDate,
      };

      setBoard((prev) => ({
        ...prev,
        [selectedOpportunity.stage]: (
          prev[selectedOpportunity.stage] || []
        ).map((opportunity) =>
          opportunity.id === selectedOpportunity.id
            ? updatedOpportunity
            : opportunity,
        ),
      }));

      toast.success("Oportunidade atualizada com sucesso.");
      handleCloseModal();
    } catch (error) {
      toast.error(error?.message || "Erro ao salvar a oportunidade.");
    } finally {
      setIsSaving(false);
    }
  }

  async function handleAddInteraction() {
    if (!selectedOpportunity?.id) return;

    const description = interactionDescription.trim();

    if (!description) {
      toast.error("Descreva a interação antes de salvar.");
      return;
    }

    if (!Number.isInteger(currentUserId) || currentUserId <= 0) {
      toast.error("Não foi possível identificar o usuário logado.");
      return;
    }

    setIsAddingInteraction(true);

    try {
      await interactionApi.AddToOpportunity(selectedOpportunity.id, {
        description,
        userId: currentUserId,
        fromStage: selectedOpportunity.stage ?? null,
        toStage: selectedOpportunity.stage ?? null,
        interactionDate: new Date().toISOString(),
      });

      await fetchInteractions(selectedOpportunity.id);
      setInteractionDescription("");
      toast.success("Interação adicionada com sucesso.");
    } catch (error) {
      toast.error(error?.message || "Erro ao adicionar interação.");
    } finally {
      setIsAddingInteraction(false);
    }
  }

  return (
    <Sidebar>
      <Topbar>
        <div className={style["pagina-kanban"]}>
          <ListingHeader
            title="Kanban de Oportunidades"
            description="Gerencie suas oportunidades."
          />

          {isLoading ? (
            <div className={style["kanban-loading"]}>Carregando board...</div>
          ) : (
            <DragDropContext onDragEnd={handleDragEnd}>
              <div className={style["kanban-board"]}>
                {STAGES.map((stage) => {
                  const cards = board[stage.id] || [];
                  return (
                    <div key={stage.id} className={style["kanban-column"]}>
                      <div
                        className={`${style["column-header"]} ${style[stage.headerClassName]}`}
                      >
                        <span className={style["column-title"]}>
                          {stage.label}
                        </span>
                        <span className={style["column-count"]}>
                          {cards.length}
                        </span>
                      </div>

                      <Droppable droppableId={String(stage.id)}>
                        {(provided, snapshot) => (
                          <div
                            ref={provided.innerRef}
                            {...provided.droppableProps}
                            className={`${style["column-body"]} ${
                              snapshot.isDraggingOver
                                ? style["dragging-over"]
                                : ""
                            }`}
                          >
                            {cards.length === 0 && (
                              <p className={style["column-empty"]}>
                                Nenhuma oportunidade
                              </p>
                            )}

                            {cards.map((opportunity, index) => (
                              <Draggable
                                key={String(opportunity.id)}
                                draggableId={String(opportunity.id)}
                                index={index}
                              >
                                {(provided, snapshot) => (
                                  <div
                                    ref={provided.innerRef}
                                    {...provided.draggableProps}
                                    {...provided.dragHandleProps}
                                    className={`${style["opp-card"]} ${
                                      snapshot.isDragging
                                        ? style["is-dragging"]
                                        : ""
                                    }`}
                                    onClick={() => handleCardClick(opportunity)}
                                  >
                                    <p className={style["card-lead"]}>
                                      {opportunity.leadName}
                                    </p>
                                    <p className={style["card-date"]}>
                                      Início:{" "}
                                      {formatDate(opportunity.createdAt)}
                                    </p>
                                  </div>
                                )}
                              </Draggable>
                            ))}

                            {provided.placeholder}
                          </div>
                        )}
                      </Droppable>
                    </div>
                  );
                })}
              </div>
            </DragDropContext>
          )}

          <ToastContainer position="top-right" autoClose={3000} />
        </div>

        <Modal show={isModalOpen} onHide={handleCloseModal} centered size="lg">
          <Modal.Header closeButton>
            <div className={style["modal-header-content"]}>
              <h4 className={style["modal-title"]}>
                {selectedOpportunity?.leadName}
              </h4>
              <Badge bg="primary" className={style["modal-stage-badge"]}>
                {STAGES.find((s) => s.id === selectedOpportunity?.stage)
                  ?.label || "NOVO LEAD"}
              </Badge>
            </div>
          </Modal.Header>

          <Modal.Body>
            {selectedOpportunity && (
              <>
                <Row className={style["modal-form-row"]}>
                  <Col md={4}>
                    <Form.Group>
                      <Form.Label>
                        {" "}
                        <MdInventory2 /> Produto
                      </Form.Label>

                      <Form.Control
                        as="select"
                        value={opportunityForm.productId}
                        onChange={(event) =>
                          handleProductChange(event.target.value)
                        }
                      >
                        <option value="">Selecione um produto</option>
                        {products.map((product) => (
                          <option key={product.id} value={product.id}>
                            {product.name}
                          </option>
                        ))}
                      </Form.Control>
                    </Form.Group>
                  </Col>
                  <Col md={4}>
                    <Form.Group>
                      <Form.Label>
                        {" "}
                        <FaDollarSign /> Valor
                      </Form.Label>

                      <Form.Control
                        type="text"
                        inputMode="numeric"
                        value={formatCurrencyInput(opportunityForm.amount)}
                        onChange={(event) =>
                          handleOpportunityFieldChange(
                            "amount",
                            parseCurrencyInput(event.target.value),
                          )
                        }
                      />
                    </Form.Group>
                  </Col>
                  <Col md={4}>
                    <Form.Group>
                      <Form.Label>
                        <BsCalendarCheck /> Previsão
                      </Form.Label>

                      <Form.Control
                        type="date"
                        value={opportunityForm.expectedCloseDate}
                        onChange={(event) =>
                          handleOpportunityFieldChange(
                            "expectedCloseDate",
                            event.target.value,
                          )
                        }
                      />
                    </Form.Group>
                  </Col>
                </Row>

                <section className={style["interaction-section"]}>
                  <button
                    type="button"
                    className={style["interaction-toggle"]}
                    onClick={() =>
                      setIsInteractionSectionOpen((current) => !current)
                    }
                    aria-expanded={isInteractionSectionOpen}
                  >
                    <div className={style["interaction-toggle-text"]}>
                      <h6 className={style["interaction-section-title"]}>
                        Interações
                      </h6>
                      <span className={style["interaction-counter"]}>
                        {interactions.length} registradas
                      </span>
                    </div>
                    <MdExpandMore
                      className={`${style["interaction-toggle-icon"]} ${
                        isInteractionSectionOpen
                          ? style["interaction-toggle-icon-open"]
                          : ""
                      }`}
                    />
                  </button>

                  {isInteractionSectionOpen && (
                    <div className={style["interaction-content"]}>
                      <div className={style["interaction-composer"]}>
                        <h6 className={style["interaction-subtitle"]}>
                          Registrar interação
                        </h6>

                        <Form.Control
                          as="textarea"
                          rows={4}
                          placeholder="O que foi conversado com o lead?"
                          value={interactionDescription}
                          onChange={(event) =>
                            setInteractionDescription(event.target.value)
                          }
                          disabled={isAddingInteraction}
                          className={style["interaction-textarea"]}
                        />

                        <div className={style["interaction-actions"]}>
                          <Button
                            variant="success"
                            buttonLabel={
                              isAddingInteraction
                                ? "Adicionando..."
                                : "Adicionar interação"
                            }
                            onButtonClick={handleAddInteraction}
                            disabled={isAddingInteraction}
                          />
                        </div>
                      </div>

                      <div className={style["interaction-history"]}>
                        <h6 className={style["interaction-subtitle"]}>
                          Histórico ({interactions.length})
                        </h6>

                        {isInteractionsLoading ? (
                          <Card className={style["interaction-empty-card"]}>
                            <small className={style["interaction-muted-text"]}>
                              Carregando interações...
                            </small>
                          </Card>
                        ) : interactions.length === 0 ? (
                          <Card className={style["interaction-empty-card"]}>
                            <small className={style["interaction-muted-text"]}>
                              Nenhuma interação registrada ainda.
                            </small>
                          </Card>
                        ) : (
                          interactions.map((interaction) => {
                            const stageLabel = STAGES.find(
                              (stage) => stage.id === interaction.toStage,
                            )?.label;

                            return (
                              <Card
                                key={interaction.id}
                                className={style["interaction-card"]}
                              >
                                <div
                                  className={style["interaction-card-header"]}
                                >
                                  <div>
                                    <strong
                                      className={style["interaction-user"]}
                                    >
                                      {interaction.userName ||
                                        `Usuário #${interaction.userId}`}
                                    </strong>
                                    <div>
                                      <small
                                        className={
                                          style["interaction-muted-text"]
                                        }
                                      >
                                        {formatDateTime(
                                          interaction.interactionDate,
                                        )}
                                      </small>
                                    </div>
                                  </div>

                                  <span className={style["interaction-stage"]}>
                                    {stageLabel ||
                                      interaction.toStageName ||
                                      "Sem etapa"}
                                  </span>
                                </div>

                                <small
                                  className={style["interaction-description"]}
                                >
                                  {interaction.description}
                                </small>
                              </Card>
                            );
                          })
                        )}
                      </div>
                    </div>
                  )}
                </section>

                <Card className={style["ai-card"]}>
                  <div className={style["ai-card-content"]}>
                    <div>
                      <h6 className={style["ai-card-title"]}>
                        Plano de ação com IA
                      </h6>

                      <small className={style["interaction-muted-text"]}>
                        Use IA para receber um diagnóstico e próximos passos da
                        oportunidade.
                      </small>
                    </div>

                    <Button
                      variant="danger"
                      buttonLabel="Gerar plano"
                      onButtonClick={() =>
                        toast.info(
                          "Funcionalidade de IA ainda não implementada.",
                        )
                      }
                    />
                  </div>
                </Card>
              </>
            )}
          </Modal.Body>

          <Modal.Footer>
            <Button
              variant="success"
              buttonLabel={isSaving ? "Salvando..." : "Salvar"}
              onButtonClick={handleSaveOpportunity}
              disabled={isSaving}
            />
            <Button
              variant="secondary"
              buttonLabel="Fechar"
              onButtonClick={handleCloseModal}
              disabled={isSaving}
            />
          </Modal.Footer>
        </Modal>
      </Topbar>
    </Sidebar>
  );
}

export default Kanban;
