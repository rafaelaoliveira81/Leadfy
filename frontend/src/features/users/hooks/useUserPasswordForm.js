import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { passwordSchema } from '../schemas/user.schema';

/**
 * Hook do formulário de alteração de senha.
 * @param {object} options
 * @param {object} options.user - Usuário alvo.
 * @param {(userId: number, data: object) => Promise<void>} options.onSubmit
 */
export function useUserPasswordForm({ user, onSubmit }) {
  const form = useForm({
    resolver: zodResolver(passwordSchema),
    defaultValues: {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    },
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      currentPassword: values.currentPassword,
      newPassword: values.newPassword,
    };
    await onSubmit(user.id, payload);
  });

  return {
    form,
    handleSubmit,
    isSubmitting: form.formState.isSubmitting,
  };
}
