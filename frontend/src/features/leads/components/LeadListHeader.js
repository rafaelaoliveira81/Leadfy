import style from './_leadListHeader.module.css';

export function LeadListHeader() {
  return (
    <div className={style.header}>
      <h1>Base de Leads</h1>
      <p>Gerencie os leads cadastrados</p>
    </div>
  );
}
