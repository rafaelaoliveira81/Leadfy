import style from './_userListHeader.module.css';

export function UserListHeader() {
  return (
    <div className={style.header}>
      <h1>Base de Usuários</h1>
      <p>Gerencie os usuários cadastrados</p>
    </div>
  );
}
