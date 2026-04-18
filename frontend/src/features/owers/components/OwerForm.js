import style from './_owerForm.module.css';

/**
 * Formulário de criação/edição de ower.
 * Campos: Name, UserID (select). Nunca exibe isActive.
 * @param {object} props
 * @param {import('react-hook-form').UseFormReturn} props.form
 * @param {object[]} props.users - Lista de usuários para o select.
 */
export function OwerForm({ form, users }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="ower-name">Nome</label>
        <input
          id="ower-name"
          type="text"
          className={errors.name ? style.hasError : ''}
          placeholder="Nome do responsável"
          {...register('name')}
        />
        {errors.name && (
          <span className={style.fieldError}>{errors.name.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="ower-user">Usuário Vinculado</label>
        <select
          id="ower-user"
          className={errors.userID ? style.hasError : ''}
          {...register('userID')}
        >
          <option value="">Selecione um usuário</option>
          {users.map((u) => (
            <option key={u.id} value={u.id}>
              {u.name}
            </option>
          ))}
        </select>
        {errors.userID && (
          <span className={style.fieldError}>{errors.userID.message}</span>
        )}
      </div>
    </div>
  );
}
