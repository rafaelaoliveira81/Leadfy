import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { opportunitySchema } from '../schemas/opportunity.schema';
import { amountToNumber, amountToFormValue, dateToInputValue } from '../mappers/opportunity.mapper';

/**
 * Hook do formulário de criação/edição de oportunidade.
 * @param {object} options
 * @param {'create'|'edit'} options.mode
 * @param {object|null} [options.opportunity] - Oportunidade existente (edição).
 * @param {(data: object) => Promise<void>} options.onSubmit - Callback de submit.
 */
export function useOpportunityForm({ mode, opportunity, onSubmit }) {
  const isEdit = mode === 'edit';

  const defaultValues = isEdit && opportunity
    ? {
        title: opportunity.title ?? '',
        leadId: opportunity.leadId ?? '',
        ownerId: opportunity.ownerId ?? '',
        productId: opportunity.productId ?? '',
        stage: opportunity.stage ?? 0,
        amount: amountToFormValue(opportunity.amount),
        expectedCloseDate: dateToInputValue(opportunity.expectedCloseDate),
      }
    : {
        title: '',
        leadId: '',
        ownerId: '',
        productId: '',
        stage: '0',
        amount: '',
        expectedCloseDate: '',
      };

  const form = useForm({
    resolver: zodResolver(opportunitySchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      title: values.title.trim(),
      leadId: Number(values.leadId),
      ownerId: values.ownerId ? Number(values.ownerId) : null,
      productId: Number(values.productId),
      stage: Number(values.stage),
      amount: amountToNumber(values.amount),
      expectedCloseDate: values.expectedCloseDate,
    };

    if (isEdit && opportunity) {
      payload.isActive = opportunity.isActive;
      await onSubmit(opportunity.id, payload);
    } else {
      await onSubmit(payload);
    }
  });

  return {
    form,
    handleSubmit,
    isSubmitting: form.formState.isSubmitting,
    isEdit,
  };
}
