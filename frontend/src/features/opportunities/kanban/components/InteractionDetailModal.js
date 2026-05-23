import { useEffect, useCallback } from 'react';
import { createPortal } from 'react-dom';
import { MdClose } from 'react-icons/md';
import { KANBAN_STAGE_BY_VALUE, formatDateShort } from '../constants/kanban.constants';
import style from './_kanban.module.css';

export function InteractionDetailModal({ interaction, onClose }) {
    const open = !!interaction;

    const handleKeyDown = useCallback((e) => {
        if (e.key === 'Escape') onClose();
    }, [onClose]);

    useEffect(() => {
        if (!open) return;
        document.addEventListener('keydown', handleKeyDown);
        return () => document.removeEventListener('keydown', handleKeyDown);
    }, [open, handleKeyDown]);

    if (!interaction) return null;

    const fromStageName = interaction.fromStage != null
        ? (KANBAN_STAGE_BY_VALUE[interaction.fromStage]?.label ?? interaction.fromStageName ?? '—')
        : '—';
    const toStageName = interaction.toStage != null
        ? (KANBAN_STAGE_BY_VALUE[interaction.toStage]?.label ?? interaction.toStageName ?? '—')
        : '—';

    const handleOverlayClick = (e) => {
        if (e.target === e.currentTarget) onClose();
    };

    return createPortal(
        <div
            className={style.modalOverlay}
            onClick={handleOverlayClick}
            role="dialog"
            aria-modal="true"
            aria-label="Detalhe da Interação"
            style={{ zIndex: 1200 }}
        >
            <div className={`${style.modalContent} ${style.interactionDetailModal}`}>
                <div className={style.modalHeader}>
                    <h2 className={style.modalTitle}>Detalhe da Interação</h2>
                    <button className={style.modalCloseBtn} onClick={onClose} aria-label="Fechar">
                        <MdClose />
                    </button>
                </div>

                <div className={style.interactionDetailBody}>
                    <div className={style.interactionDetailRow}>
                        <span className={style.interactionDetailLabel}>Data</span>
                        <span className={style.interactionDetailValue}>{formatDateShort(interaction.interactionDate)}</span>
                    </div>
                    <div className={style.interactionDetailRow}>
                        <span className={style.interactionDetailLabel}>Usuário</span>
                        <span className={style.interactionDetailValue}>{interaction.userName || '—'}</span>
                    </div>
                    <div className={style.interactionDetailRow}>
                        <span className={style.interactionDetailLabel}>Stage Anterior</span>
                        <span className={style.interactionDetailValue}>{fromStageName}</span>
                    </div>
                    <div className={style.interactionDetailRow}>
                        <span className={style.interactionDetailLabel}>Stage Destino</span>
                        <span className={style.interactionDetailValue}>{toStageName}</span>
                    </div>
                    <div className={`${style.interactionDetailRow} ${style.interactionDetailRowFull}`}>
                        <span className={style.interactionDetailLabel}>Descrição</span>
                        <p className={style.interactionDetailDescription}>{interaction.description || '—'}</p>
                    </div>
                </div>
            </div>
        </div>,
        document.body
    );
}
