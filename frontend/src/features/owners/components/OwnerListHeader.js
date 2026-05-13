import style from "./_ownerListHeader.module.css";

export function OwnerListHeader() {
  return (
    <div className={style.header}>
      <h1>Base de Responsáveis</h1>
      <p>Gerencie os responsáveis cadastrados</p>
    </div>
  );
}
