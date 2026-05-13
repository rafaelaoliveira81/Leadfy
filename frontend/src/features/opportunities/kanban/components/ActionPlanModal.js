import { useState, useEffect, useCallback } from "react";
import { createPortal } from "react-dom";
import { MdClose, MdAutoAwesome } from "react-icons/md";
import { Button } from "../../../../components/ui/Button/Button";
import { useActionPlan } from "../hooks/useActionPlan";
import style from "./_kanban.module.css";

export function ActionPlanModal({ open, opportunity, onClose, onGenerated }) {
  const { isGenerating, error, fetchActiveConfig, generatePlan } =
    useActionPlan();
  const [activeConfig, setActiveConfig] = useState(null);
  const [configError, setConfigError] = useState(null);
  const [localPlan, setLocalPlan] = useState(null);
  const [localGeneratedAt, setLocalGeneratedAt] = useState(null);

  useEffect(() => {
    if (!open || !opportunity) return;
    setLocalPlan(opportunity.actionPlan || null);
    setLocalGeneratedAt(opportunity.actionPlanGeneratedAt || null);
    setActiveConfig(null);
    setConfigError(null);

    fetchActiveConfig()
      .then(setActiveConfig)
      .catch(() => {
        setActiveConfig(null);
        setConfigError(
          "Nenhuma configuração de IA ativa encontrada. Ative uma configuração antes de gerar o plano.",
        );
      });
  }, [open, opportunity, fetchActiveConfig]);

  const handleGenerate = useCallback(async () => {
    if (!activeConfig) return;
    try {
      const result = await generatePlan(opportunity.id, activeConfig.id);
      setLocalPlan(result.actionPlan);
      setLocalGeneratedAt(result.actionPlanGeneratedAt);
      if (onGenerated) onGenerated(result);
    } catch {
      // error is handled by the hook
    }
  }, [activeConfig, generatePlan, opportunity, onGenerated]);

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

  const hasExistingPlan = Boolean(localPlan);

  const handleOverlayClick = (e) => {
    if (e.target === e.currentTarget) onClose();
  };

  return createPortal(
    <div className={style.modalOverlay} onClick={handleOverlayClick}>
      <div className={style.actionPlanModalContent}>
        <div className={style.modalHeader}>
          <div>
            <h2 className={style.modalTitle}>Plano de Ação</h2>
            <p className={style.actionPlanSubtitle}>{opportunity.title}</p>
          </div>
          <button
            className={style.modalCloseBtn}
            onClick={onClose}
            aria-label="Fechar"
          >
            <MdClose />
          </button>
        </div>

        <div className={style.actionPlanBody}>
          {configError && (
            <div className={style.actionPlanWarning}>{configError}</div>
          )}

          {!configError && hasExistingPlan && (
            <>
              <div className={style.actionPlanGeneratedAt}>
                Gerado em:{" "}
                {localGeneratedAt
                  ? new Date(localGeneratedAt).toLocaleString("pt-BR")
                  : "—"}
              </div>
              <div className={style.actionPlanText}>{localPlan}</div>
            </>
          )}

          {!configError && !hasExistingPlan && (
            <p className={style.actionPlanEmpty}>
              Este card ainda não possui um plano de ação. Clique no botão
              abaixo para gerar.
            </p>
          )}

          {error && <div className={style.actionPlanError}>{error}</div>}
        </div>

        <div className={style.actionPlanFooter}>
          <Button variant="ghost" onClick={onClose} disabled={isGenerating}>
            Fechar
          </Button>
          {!configError && (
            <Button
              onClick={handleGenerate}
              disabled={isGenerating || !activeConfig}
            >
              <MdAutoAwesome style={{ marginRight: 6 }} />
              {isGenerating
                ? "Gerando…"
                : hasExistingPlan
                  ? "Regenerar"
                  : "Gerar Plano de Ação"}
            </Button>
          )}
        </div>
      </div>
    </div>,
    document.body,
  );
}
