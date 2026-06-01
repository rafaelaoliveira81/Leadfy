import { useState, useEffect, useCallback } from "react";
import { Badge, Card, Col, Form, Modal, Row } from "react-bootstrap";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { FaDollarSign } from "react-icons/fa";
import { MdInventory2 } from "react-icons/md";
import { BsCalendarCheck } from "react-icons/bs";

import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import opportunityAPI from "../../services/opportunityApi";

import style from "./_kanban.module.css";
import { ListingHeader } from "../../components/ListingHeader/ListingHeader";
import { Button } from "../../components/Button/Button";

const STAGES = [
  { id: 1, label: "Novo Lead", color: "#2563EB" },
  { id: 2, label: "Em contato", color: "#0891B2" },
  { id: 3, label: "Qualificado", color: "#7C3AED" },
  { id: 4, label: "Proposta Enviada", color: "#EA580C" },
  { id: 5, label: "Negociação", color: "#CA8A04" },
  { id: 6, label: "Ganho", color: "#15803D" },
  { id: 7, label: "Perdido", color: "#B91C1C" },
];

function formatDate(dateString) {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleDateString("pt-BR");
}

function formatCurrency(value) {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value);
}

function Kanban() {
  const [board, setBoard] = useState({});
  const [isLoading, setIsLoading] = useState(true);
  const [selectedOpportunity, setSelectedOpportunity] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

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
      const patchResult = await opportunityAPI.PatchStage(
        movedCard.id,
        destStageId,
      );
    } catch {
      setBoard(savedBoard);
      toast.error("Erro ao mover a oportunidade. Tente novamente.");
    }
  }

  function handleCardClick(opportunity) {
    setSelectedOpportunity(opportunity);
    setIsModalOpen(true);
  }

  function handleCloseModal() {
    setIsModalOpen(false);
    setSelectedOpportunity(null);
  }

  const handleClickAddLead = () => {
    toast.info("Funcionalidade de adicionar lead ainda não implementada.");
  };

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
                        className={style["column-header"]}
                        style={{ backgroundColor: stage.color }}
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
            <div className="d-flex flex-row align-items-center gap-3">
              <h4 className="mb-0">{selectedOpportunity?.leadName}</h4>
              <Badge bg="primary" className="mb-2">
                {STAGES.find((s) => s.id === selectedOpportunity?.stage)
                  ?.label || "NOVO LEAD"}
              </Badge>
            </div>
          </Modal.Header>

          <Modal.Body>
            {selectedOpportunity && (
              <>
                <Row className="mb-4">
                  <Col md={4}>
                    <Form.Group>
                      <Form.Label>
                        {" "}
                        <FaDollarSign /> Valor
                      </Form.Label>

                      <Form.Control
                        type="text"
                        defaultValue={formatCurrency(
                          selectedOpportunity.amount,
                        )}
                      />
                    </Form.Group>
                  </Col>
                  <Col md={4}>
                    <Form.Group>
                      <Form.Label>
                        {" "}
                        <MdInventory2 /> Produto
                      </Form.Label>

                      <Form.Control
                        type="text"
                        defaultValue={selectedOpportunity.productName}
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
                        defaultValue={
                          selectedOpportunity.expectedCloseDate?.split("T")[0]
                        }
                      />
                    </Form.Group>
                  </Col>{" "}
                </Row>

                {/* Registrar interação */}
                <div className="mb-4">
                  <h6>Registrar interação</h6>

                  <Form.Control
                    as="textarea"
                    rows={4}
                    placeholder="O que foi conversado com o lead?"
                  />

                  <div className="d-flex justify-content-end mt-2">
                    <Button
                      variant="success"
                      buttonLabel="Adicionar interação"
                      onButtonClick={() =>
                        toast.info(
                          "Funcionalidade de registrar interação ainda não implementada.",
                        )
                      }
                    />
                  </div>
                </div>

                {/* Histórico */}
                <div className="mb-4">
                  <h6>Histórico (0)</h6>

                  <Card className="p-3 bg-light">
                    <small className="text-muted">
                      Nenhuma interação registrada ainda.
                    </small>
                  </Card>
                </div>

                {/* IA */}
                <Card className="p-3">
                  <div className="d-flex justify-content-between align-items-center">
                    <div>
                      <h6 className="mb-1">Plano de ação com IA</h6>

                      <small className="text-muted">
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
              variant="secondary"
              buttonLabel="Fechar"
              onClick={handleCloseModal}
            />
          </Modal.Footer>
        </Modal>
      </Topbar>
    </Sidebar>
  );
}

export default Kanban;
