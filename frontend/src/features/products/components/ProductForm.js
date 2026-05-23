import style from './_productForm.module.css';

/**
 * Formulário de criação/edição de produto.
 * Campos: Name, Description, Price. Nunca exibe isActive.
 * @param {object} props
 * @param {import('react-hook-form').UseFormReturn} props.form
 */
export function ProductForm({ form }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="product-name">Nome</label>
        <input
          id="product-name"
          type="text"
          className={errors.name ? style.hasError : ''}
          placeholder="Nome do produto"
          {...register('name')}
        />
        {errors.name && (
          <span className={style.fieldError}>{errors.name.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="product-description">Descrição</label>
        <textarea
          id="product-description"
          className={errors.description ? style.hasError : ''}
          placeholder="Descrição do produto"
          {...register('description')}
        />
        {errors.description && (
          <span className={style.fieldError}>{errors.description.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="product-price">Preço</label>
        <input
          id="product-price"
          type="text"
          inputMode="decimal"
          className={errors.price ? style.hasError : ''}
          placeholder="0,00"
          {...register('price')}
        />
        {errors.price && (
          <span className={style.fieldError}>{errors.price.message}</span>
        )}
      </div>
    </div>
  );
}
