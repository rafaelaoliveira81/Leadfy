import style from './_statusBadge.module.css';

/**
 * Badge de status genérico.
 * @param {object} props
 * @param {boolean} props.isActive
 * @param {string} [props.activeLabel] - Texto quando ativo.
 * @param {string} [props.inactiveLabel] - Texto quando inativo.
 */
export function StatusBadge({
  isActive,
  activeLabel = 'Ativo',
  inactiveLabel = 'Inativo',
}) {
  return (
    <span className={`${style.badge} ${isActive ? style.active : style.inactive}`}>
      {isActive ? activeLabel : inactiveLabel}
    </span>
  );
}
