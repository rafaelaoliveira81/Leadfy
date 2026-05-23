import style from "./_ownerForm.module.css";

/**
 * Formulário de criação/edição de owner.
 * Campos: Name, UserID (select). Nunca exibe isActive.
 * @param {object} props
 * @param {import('react-hook-form').UseFormReturn} props.form
 * @param {object[]} props.users - Lista de usuários para o select.
 */
export function OwnerForm({ form, users }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="owner-name">Nome</label>
        <input
          id="owner-name"
          type="text"
          className={errors.name ? style.hasError : ""}
          placeholder="Nome do responsável"
          {...register("name")}
        />
        {errors.name && (
          <span className={style.fieldError}>{errors.name.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="owner-user">Usuário Vinculado</label>
        <select
          id="owner-user"
          className={errors.userID ? style.hasError : ""}
          {...register("userID")}
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
