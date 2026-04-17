import style from './_productListHeader.module.css';

export function ProductListHeader() {
  return (
    <div className={style.header}>
      <h1>Base de Produtos</h1>
      <p>Gerencie os produtos cadastrados</p>
    </div>
  );
}
