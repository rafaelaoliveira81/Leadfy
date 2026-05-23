import style from './_loadingState.module.css';

/**
 * Skeleton de carregamento genérico.
 * @param {object} props
 * @param {number} [props.rows] - Quantidade de linhas skeleton.
 */
export function LoadingState({ rows = 5 }) {
  return (
    <div className={style.loading} aria-busy="true" aria-label="Carregando...">
      {Array.from({ length: rows }, (_, i) => (
        <div key={i} className={style.row} />
      ))}
    </div>
  );
}
