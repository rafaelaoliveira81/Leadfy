import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { userSchema, userCreateSchema } from '../schemas/user.schema';

/**
 * Hook do formulário de criação/edição de usuário.
 * @param {object} options
 * @param {'create'|'edit'} options.mode
 * @param {object|null} [options.user] - Usuário existente (edição).
 * @param {(data: object) => Promise<void>} options.onSubmit - Callback de submit.
 */
export function useUserForm({ mode, user, onSubmit }) {
  const isEdit = mode === 'edit';

  const defaultValues = isEdit && user
    ? {
        name: user.name ?? '',
        email: user.email ?? '',
        idRole: user.role?.id ?? '',
      }
    : {
        name: '',
        email: '',
        idRole: '',
        password: '',
      };

  const form = useForm({
    resolver: zodResolver(isEdit ? userSchema : userCreateSchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      name: values.name.trim(),
      email: values.email.trim(),
      idRole: Number(values.idRole),
    };

    if (isEdit && user) {
      await onSubmit(user.id, payload);
    } else {
      payload.password = values.password;
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
