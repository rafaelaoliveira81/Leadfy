import style from './_userForm.module.css';

/**
 * Formulário de criação/edição de usuário.
 * Campos: Name e Email. Password apenas em criação. Nunca exibe isActive.
 * @param {object} props
 * @param {import('react-hook-form').UseFormReturn} props.form
 * @param {boolean} props.isEdit - Se é edição (oculta campo de senha).
 */
export function UserForm({ form, isEdit }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="user-name">Nome</label>
        <input
          id="user-name"
          type="text"
          className={errors.name ? style.hasError : ''}
          placeholder="Nome do usuário"
          {...register('name')}
        />
        {errors.name && (
          <span className={style.fieldError}>{errors.name.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="user-email">E-mail</label>
        <input
          id="user-email"
          type="email"
          className={errors.email ? style.hasError : ''}
          placeholder="email@exemplo.com"
          {...register('email')}
        />
        {errors.email && (
          <span className={style.fieldError}>{errors.email.message}</span>
        )}
      </div>

      {!isEdit && (
        <div className={style.field}>
          <label htmlFor="user-password">Senha</label>
          <input
            id="user-password"
            type="password"
            className={errors.password ? style.hasError : ''}
            placeholder="Mínimo 8 caracteres"
            {...register('password')}
          />
          {errors.password && (
            <span className={style.fieldError}>{errors.password.message}</span>
          )}
        </div>
      )}
    </div>
  );
}
