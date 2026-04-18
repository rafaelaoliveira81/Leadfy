import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { owerSchema } from '../schemas/ower.schema';

/**
 * Hook do formulário de criação/edição de ower.
 * @param {object} options
 * @param {'create'|'edit'} options.mode
 * @param {object|null} [options.ower] - Ower existente (edição).
 * @param {(data: object) => Promise<void>} options.onSubmit - Callback de submit.
 */
export function useOwerForm({ mode, ower, onSubmit }) {
  const isEdit = mode === 'edit';

  const defaultValues = isEdit && ower
    ? {
        name: ower.name ?? '',
        userID: ower.userID ?? '',
      }
    : {
        name: '',
        userID: '',
      };

  const form = useForm({
    resolver: zodResolver(owerSchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      name: values.name.trim(),
      userID: Number(values.userID),
    };

    if (isEdit && ower) {
      payload.isActive = ower.isActive;
      await onSubmit(ower.id, payload);
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
