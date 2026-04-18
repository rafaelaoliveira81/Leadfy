import style from './_leadForm.module.css';

/**
 * Formulário de criação/edição de lead.
 * Campos: Name, Email, PhoneNumber. Nunca exibe isActive.
 * @param {object} props
 * @param {import('react-hook-form').UseFormReturn} props.form
 */
export function LeadForm({ form }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="lead-name">Nome</label>
        <input
          id="lead-name"
          type="text"
          className={errors.name ? style.hasError : ''}
          placeholder="Nome do lead"
          {...register('name')}
        />
        {errors.name && (
          <span className={style.fieldError}>{errors.name.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="lead-email">E-mail</label>
        <input
          id="lead-email"
          type="email"
          className={errors.email ? style.hasError : ''}
          placeholder="email@exemplo.com"
          {...register('email')}
        />
        {errors.email && (
          <span className={style.fieldError}>{errors.email.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="lead-phone">Telefone</label>
        <input
          id="lead-phone"
          type="text"
          inputMode="tel"
          className={errors.phoneNumber ? style.hasError : ''}
          placeholder="(00) 00000-0000"
          {...register('phoneNumber')}
        />
        {errors.phoneNumber && (
          <span className={style.fieldError}>{errors.phoneNumber.message}</span>
        )}
      </div>
    </div>
  );
}
