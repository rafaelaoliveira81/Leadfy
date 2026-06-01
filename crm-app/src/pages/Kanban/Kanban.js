import { useState, useEffect, useCallback } from "react";
import { Modal } from "react-bootstrap";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import opportunityAPI from "../../services/opportunityApi";

import style from "./_kanban.module.css";
import { ListingHeader } from "../../components/ListingHeader/ListingHeader";

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

        <Modal show={isModalOpen} onHide={handleCloseModal} centered>
          <Modal.Header closeButton>
            <Modal.Title>Detalhes da Oportunidade</Modal.Title>
          </Modal.Header>

          <Modal.Body>
            {selectedOpportunity && (
              <div className={style["modal-details"]}>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>Lead</span>
                  <span>{selectedOpportunity.leadName}</span>
                </div>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>Produto</span>
                  <span>{selectedOpportunity.productName}</span>
                </div>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>Estágio</span>
                  <span>
                    {STAGES.find((s) => s.id === selectedOpportunity.stage)
                      ?.label ?? selectedOpportunity.stageName}
                  </span>
                </div>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>Status</span>
                  <span>{selectedOpportunity.status}</span>
                </div>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>Valor</span>
                  <span>{formatCurrency(selectedOpportunity.amount)}</span>
                </div>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>Data de Início</span>
                  <span>{formatDate(selectedOpportunity.createdAt)}</span>
                </div>
                <div className={style["detail-row"]}>
                  <span className={style["detail-label"]}>
                    Previsão de Fechamento
                  </span>
                  <span>
                    {formatDate(selectedOpportunity.expectedCloseDate)}
                  </span>
                </div>
              </div>
            )}
          </Modal.Body>

          <Modal.Footer>
            <button className={style["btn-fechar"]} onClick={handleCloseModal}>
              Fechar
            </button>
          </Modal.Footer>
        </Modal>
      </Topbar>
    </Sidebar>
  );
}

export default Kanban;
