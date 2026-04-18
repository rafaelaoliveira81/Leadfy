import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { leadSchema } from '../schemas/lead.schema';

/**
 * Hook do formulário de criação/edição de lead.
 * @param {object} options
 * @param {'create'|'edit'} options.mode
 * @param {object|null} [options.lead] - Lead existente (edição).
 * @param {(data: object) => Promise<void>} options.onSubmit - Callback de submit.
 */
export function useLeadForm({ mode, lead, onSubmit }) {
  const isEdit = mode === 'edit';

  const defaultValues = isEdit && lead
    ? {
        name: lead.name ?? '',
        email: lead.email ?? '',
        phoneNumber: lead.phoneNumber ?? '',
      }
    : {
        name: '',
        email: '',
        phoneNumber: '',
      };

  const form = useForm({
    resolver: zodResolver(leadSchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      name: values.name.trim(),
      email: values.email.trim(),
      phoneNumber: values.phoneNumber?.trim() || '',
    };

    if (isEdit && lead) {
      await onSubmit(lead.id, payload);
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
