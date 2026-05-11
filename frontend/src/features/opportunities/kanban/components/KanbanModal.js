import { useState, useEffect, useCallback } from "react";
import { createPortal } from "react-dom";
import { MdClose, MdExpandMore } from "react-icons/md";
import { Button } from "../../../../components/ui/Button/Button";
import {
  KANBAN_STAGES,
  KANBAN_STAGE_BY_VALUE,
  formatCurrency,
  formatDateShort,
} from "../constants/kanban.constants";
import leadAPI from "../../../../services/leadApi";
import { useInteractions } from "../hooks/useInteractions";
import { InteractionHistory } from "./InteractionHistory";
import { InteractionAddModal } from "./InteractionAddModal";
import { InteractionDetailModal } from "./InteractionDetailModal";
import style from "./_kanban.module.css";

export function KanbanModal({
  open,
  opportunity,
  onClose,
  onStageChange,
  onAfterInteraction,
}) {
  const [leadDetail, setLeadDetail] = useState(null);
  const [selectedStage, setSelectedStage] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [productOpen, setProductOpen] = useState(false);

  const {
    interactions,
    isLoading: interactionsLoading,
    historyOpen,
    addModalOpen,
    detailModalData,
    openHistory,
    closeHistory,
    openAddModal,
    closeAddModal,
    openDetailModal,
    closeDetailModal,
    addInteraction,
  } = useInteractions(opportunity?.id, open);

  const handleInteractionSubmit = async (data) => {
    await addInteraction(data);
    if (onAfterInteraction) await onAfterInteraction();
    onClose();
  };

  const handleToggleHistory = () => {
    if (historyOpen) closeHistory();
    else openHistory();
  };

  useEffect(() => {
    if (!open || !opportunity) return;
    setSelectedStage(opportunity.stage);
    setLeadDetail(null);

    if (opportunity.leadId) {
      leadAPI.GetById(opportunity.leadId).then((data) => {
        if (data && !data.type) setLeadDetail(data);
      });
    }
  }, [open, opportunity]);

  const handleKeyDown = useCallback(
    (e) => {
      if (e.key === "Escape") onClose();
    },
    [onClose],
  );

  useEffect(() => {
    if (!open) return;
    document.addEventListener("keydown", handleKeyDown);
    document.body.style.overflow = "hidden";
    return () => {
      document.removeEventListener("keydown", handleKeyDown);
      document.body.style.overflow = "";
    };
  }, [open, handleKeyDown]);

  if (!open || !opportunity) return null;

  const stageInfo = KANBAN_STAGE_BY_VALUE[opportunity.stage] || {};

  const handleOverlayClick = (e) => {
    if (e.target === e.currentTarget) onClose();
  };

  const handleSaveStage = async () => {
    if (selectedStage === opportunity.stage) return;
    setIsSubmitting(true);
    try {
      await onStageChange(opportunity.id, selectedStage);
      onClose();
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <>
      {createPortal(
        <div
          className={style.modalOverlay}
          onClick={handleOverlayClick}
          role="dialog"
          aria-modal="true"
          aria-label={opportunity.title}
        >
          <div className={style.modalContent}>
            {/* Header */}
            <div className={style.modalHeader}>
              <h2 className={style.modalTitle}>{opportunity.title}</h2>
              <button
                className={style.modalCloseBtn}
                onClick={onClose}
                aria-label="Fechar"
              >
                <MdClose />
              </button>
            </div>

            {/* Body — two columns: Lead and Opportunity side by side */}
            <div className={style.modalBody}>
              {/* Left */}
              <div className={style.modalSection}>
                <h3 className={style.modalSectionTitle}>Informações do Lead</h3>
                <div className={style.modalField}>
                  <span className={style.modalFieldLabel}>Nome</span>
                  <span className={style.modalFieldValue}>
                    {leadDetail?.name || opportunity.leadName || "—"}
                  </span>
                </div>
                <div className={style.modalField}>
                  <span className={style.modalFieldLabel}>Telefone</span>
                  <span className={style.modalFieldValue}>
                    {leadDetail?.phoneNumber || "—"}
                  </span>
                </div>
              </div>

              {/* Right */}
              <div className={style.modalSection}>
                <h3 className={style.modalSectionTitle}>
                  Informações da Oportunidade
                </h3>
                <div className={style.modalField}>
                  <span className={style.modalFieldLabel}>Owner</span>
                  <span className={style.modalFieldValue}>
                    {opportunity.ownerName || "—"}
                  </span>
                </div>
                <div className={style.modalField}>
                  <span className={style.modalFieldLabel}>Data Prevista</span>
                  <span className={style.modalFieldValue}>
                    {formatDateShort(opportunity.expectedCloseDate)}
                  </span>
                </div>
                <div className={style.modalField}>
                  <span className={style.modalFieldLabel}>Valor</span>
                  <span className={style.modalFieldValue}>
                    {formatCurrency(opportunity.amount)}
                  </span>
                </div>
              </div>
            </div>

            <div className={style.modalBody}>
              {/* Products section */}
              <div className={style.modalSection}>
                <h3 className={style.modalSectionTitle}>Produtos Vinculados</h3>
                <div className={style.accordion}>
                  <button
                    className={style.accordionHeader}
                    onClick={() => setProductOpen((p) => !p)}
                    aria-expanded={productOpen}
                  >
                    {opportunity.productName || "Produto"}
                    <MdExpandMore
                      className={`${style.accordionIcon} ${productOpen ? style.accordionIconOpen : ""}`}
                    />
                  </button>
                  {productOpen && (
                    <div className={style.accordionBody}>
                      <div className={style.modalField}>
                        <span className={style.modalFieldLabel}>Produto</span>
                        <span className={style.modalFieldValue}>
                          {opportunity.productName || "—"}
                        </span>
                      </div>
                      <div
                        className={style.modalField}
                        style={{ marginTop: 8 }}
                      >
                        <span className={style.modalFieldLabel}>
                          Valor da Oportunidade
                        </span>
                        <span className={style.modalFieldValue}>
                          {formatCurrency(opportunity.amount)}
                        </span>
                      </div>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* Interactions section */}
            <InteractionHistory
              historyOpen={historyOpen}
              onToggleHistory={handleToggleHistory}
              interactions={interactions}
              isLoading={interactionsLoading}
              onOpenAddModal={openAddModal}
              onOpenDetail={openDetailModal}
            />

            {/* Footer */}
            <div className={style.modalFooter}>
              Estado atual:{" "}
              <strong style={{ color: stageInfo.accent }}>
                {stageInfo.label || opportunity.stageName}
              </strong>{" "}
              — Criada em {formatDateShort(opportunity.createdAt)}
            </div>
          </div>
        </div>,
        document.body,
      )}

      <InteractionAddModal
        open={addModalOpen}
        opportunity={opportunity}
        onClose={closeAddModal}
        onSubmit={handleInteractionSubmit}
      />

      <InteractionDetailModal
        interaction={detailModalData}
        onClose={closeDetailModal}
      />
    </>
  );
}
