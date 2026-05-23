import { MdInbox } from 'react-icons/md';
import { Button } from '../Button/Button';
import style from './_emptyState.module.css';

/**
 * Estado vazio genérico.
 * @param {object} props
 * @param {string} [props.message]
 * @param {string} [props.actionLabel]
 * @param {Function} [props.onAction]
 */
export function EmptyState({
  message = 'Nenhum registro encontrado.',
  actionLabel,
  onAction,
}) {
  return (
    <div className={style.empty}>
      <MdInbox className={style.icon} />
      <p className={style.message}>{message}</p>
      {actionLabel && onAction && (
        <Button onClick={onAction}>{actionLabel}</Button>
      )}
    </div>
  );
}
