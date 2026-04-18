import { OPPORTUNITY_STAGE_OPTIONS } from '../constants/opportunity.constants';
import style from './_opportunityForm.module.css';

/**
 * Formulário de criação/edição de oportunidade.
 * @param {object} props
 * @param {import('react-hook-form').UseFormReturn} props.form
 * @param {object[]} props.leads
 * @param {object[]} props.owers
 * @param {object[]} props.products
 */
export function OpportunityForm({ form, leads, owers, products }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="opp-title">Título</label>
        <input
          id="opp-title"
          type="text"
          className={errors.title ? style.hasError : ''}
          placeholder="Título da oportunidade"
          {...register('title')}
        />
        {errors.title && (
          <span className={style.fieldError}>{errors.title.message}</span>
        )}
      </div>

      <div className={style.row}>
        <div className={style.field}>
          <label htmlFor="opp-lead">Lead</label>
          <select
            id="opp-lead"
            className={errors.leadId ? style.hasError : ''}
            {...register('leadId')}
          >
            <option value="">Selecione um lead</option>
            {leads.map((l) => (
              <option key={l.id} value={l.id}>
                {l.name}
              </option>
            ))}
          </select>
          {errors.leadId && (
            <span className={style.fieldError}>{errors.leadId.message}</span>
          )}
        </div>

        <div className={style.field}>
          <label htmlFor="opp-owner">Responsável</label>
          <select
            id="opp-owner"
            className={errors.ownerId ? style.hasError : ''}
            {...register('ownerId')}
          >
            <option value="">Nenhum (opcional)</option>
            {owers.map((o) => (
              <option key={o.id} value={o.id}>
                {o.name}
              </option>
            ))}
          </select>
          {errors.ownerId && (
            <span className={style.fieldError}>{errors.ownerId.message}</span>
          )}
        </div>
      </div>

      <div className={style.row}>
        <div className={style.field}>
          <label htmlFor="opp-product">Produto</label>
          <select
            id="opp-product"
            className={errors.productId ? style.hasError : ''}
            {...register('productId')}
          >
            <option value="">Selecione um produto</option>
            {products.map((p) => (
              <option key={p.id} value={p.id}>
                {p.name}
              </option>
            ))}
          </select>
          {errors.productId && (
            <span className={style.fieldError}>{errors.productId.message}</span>
          )}
        </div>

        <div className={style.field}>
          <label htmlFor="opp-stage">Etapa</label>
          <select
            id="opp-stage"
            className={errors.stage ? style.hasError : ''}
            {...register('stage')}
          >
            {OPPORTUNITY_STAGE_OPTIONS.map((s) => (
              <option key={s.value} value={s.value}>
                {s.label}
              </option>
            ))}
          </select>
          {errors.stage && (
            <span className={style.fieldError}>{errors.stage.message}</span>
          )}
        </div>
      </div>

      <div className={style.row}>
        <div className={style.field}>
          <label htmlFor="opp-amount">Valor</label>
          <input
            id="opp-amount"
            type="text"
            inputMode="decimal"
            className={errors.amount ? style.hasError : ''}
            placeholder="0,00"
            {...register('amount')}
          />
          {errors.amount && (
            <span className={style.fieldError}>{errors.amount.message}</span>
          )}
        </div>

        <div className={style.field}>
          <label htmlFor="opp-date">Data Prevista</label>
          <input
            id="opp-date"
            type="date"
            className={errors.expectedCloseDate ? style.hasError : ''}
            {...register('expectedCloseDate')}
          />
          {errors.expectedCloseDate && (
            <span className={style.fieldError}>{errors.expectedCloseDate.message}</span>
          )}
        </div>
      </div>
    </div>
  );
}
