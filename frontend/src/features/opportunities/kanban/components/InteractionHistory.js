import { MdExpandMore, MdVisibility } from 'react-icons/md';
import { KANBAN_STAGE_BY_VALUE, formatDateShort } from '../constants/kanban.constants';
import { Button } from '../../../../components/ui/Button/Button';
import style from './_kanban.module.css';

export function InteractionHistory({
    historyOpen,
    onToggleHistory,
    interactions,
    isLoading,
    onOpenAddModal,
    onOpenDetail,
}) {
    return (
        <div className={style.interactionSection}>
            <div className={style.interactionSectionHeader}>
                <button
                    className={style.interactionToggleBtn}
                    onClick={onToggleHistory}
                    aria-expanded={historyOpen}
                >
                    <span className={style.interactionSectionTitle}>Interações</span>
                    <MdExpandMore
                        className={`${style.accordionIcon} ${historyOpen ? style.accordionIconOpen : ''}`}
                    />
                </button>
                <Button onClick={onOpenAddModal}>
                    Nova interação
                </Button>
            </div>

            {historyOpen && (
                <div className={style.interactionList}>
                    {isLoading && (
                        <p className={style.interactionEmpty}>Carregando...</p>
                    )}
                    {!isLoading && interactions.length === 0 && (
                        <p className={style.interactionEmpty}>Nenhuma interação registrada.</p>
                    )}
                    {!isLoading && interactions.map((item) => (
                        <div key={item.id} className={style.interactionItem}>
                            <div className={style.interactionItemInfo}>
                                <span className={style.interactionItemDate}>
                                    {formatDateShort(item.interactionDate)}
                                </span>
                                <span className={style.interactionItemUser}>
                                    {item.userName || `Usuário #${item.userId}`}
                                </span>
                                <span className={style.interactionItemStage}>
                                    → {item.toStage != null
                                        ? (KANBAN_STAGE_BY_VALUE[item.toStage]?.label ?? item.toStageName ?? '—')
                                        : '—'}
                                </span>
                            </div>
                            <button
                                className={style.interactionViewBtn}
                                onClick={() => onOpenDetail(item)}
                                title="Ver detalhes"
                                aria-label="Ver detalhes da interação"
                            >
                                <MdVisibility />
                            </button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
