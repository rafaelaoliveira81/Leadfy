import { useState, useEffect, useCallback } from 'react';
import { createPortal } from 'react-dom';
import { MdClose } from 'react-icons/md';
import { KANBAN_STAGES } from '../constants/kanban.constants';
import { Button } from '../../../../components/ui/Button/Button';
import style from './_kanban.module.css';

export function InteractionAddModal({ open, opportunity, onClose, onSubmit }) {
    const [description, setDescription] = useState('');
    const [toStage, setToStage] = useState('');
    const [userId, setUserId] = useState('1');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        if (open && opportunity) {
            setDescription('');
            setToStage(String(opportunity.stage));
            setUserId('1');
            setError('');
        }
    }, [open, opportunity]);

    const handleKeyDown = useCallback((e) => {
        if (e.key === 'Escape') onClose();
    }, [onClose]);

    useEffect(() => {
        if (!open) return;
        document.addEventListener('keydown', handleKeyDown);
        return () => document.removeEventListener('keydown', handleKeyDown);
    }, [open, handleKeyDown]);

    if (!open || !opportunity) return null;

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!description.trim()) {
            setError('Descrição é obrigatória.');
            return;
        }
        if (!toStage) {
            setError('Selecione o stage de destino.');
            return;
        }
        setError('');
        setIsSubmitting(true);
        try {
            await onSubmit({ description: description.trim(), toStage, userId });
        } catch (err) {
            setError(err?.message || 'Erro ao salvar interação.');
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleOverlayClick = (e) => {
        if (e.target === e.currentTarget) onClose();
    };

    return createPortal(
        <div
            className={style.modalOverlay}
            onClick={handleOverlayClick}
            role="dialog"
            aria-modal="true"
            aria-label="Nova Interação"
            style={{ zIndex: 1100 }}
        >
            <div className={`${style.modalContent} ${style.interactionAddModal}`}>
                <div className={style.modalHeader}>
                    <h2 className={style.modalTitle}>Nova Interação</h2>
                    <button className={style.modalCloseBtn} onClick={onClose} aria-label="Fechar">
                        <MdClose />
                    </button>
                </div>

                <form onSubmit={handleSubmit}>
                    <div className={style.interactionFormBody}>
                        <div className={style.interactionFormField}>
                            <label className={style.interactionFormLabel}>Descrição *</label>
                            <textarea
                                className={style.interactionTextarea}
                                rows={4}
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                placeholder="Descreva a interação realizada..."
                            />
                        </div>

                        <div className={style.interactionFormField}>
                            <label className={style.interactionFormLabel}>Mover para o Stage *</label>
                            <select
                                className={style.stageSelect}
                                value={toStage}
                                onChange={(e) => setToStage(e.target.value)}
                            >
                                {KANBAN_STAGES.map((s) => (
                                    <option key={s.value} value={s.value}>{s.label}</option>
                                ))}
                            </select>
                        </div>

                        <div className={style.interactionFormField}>
                            <label className={style.interactionFormLabel}>ID do Usuário</label>
                            <input
                                className={style.interactionInput}
                                type="number"
                                min="1"
                                value={userId}
                                onChange={(e) => setUserId(e.target.value)}
                            />
                        </div>

                        {error && <p className={style.interactionError}>{error}</p>}
                    </div>

                    <div className={style.modalActions}>
                        <Button type="button" variant="secondary" onClick={onClose} disabled={isSubmitting}>
                            Cancelar
                        </Button>
                        <Button type="submit" disabled={isSubmitting}>
                            {isSubmitting ? 'Salvando...' : 'Salvar Interação'}
                        </Button>
                    </div>
                </form>
            </div>
        </div>,
        document.body
    );
}
