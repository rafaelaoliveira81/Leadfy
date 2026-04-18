import { Modal } from '../../../components/ui/Modal/Modal';
import { useUserPasswordForm } from '../hooks/useUserPasswordForm';
import style from './_userPasswordModal.module.css';

/**
 * Modal de alteração de senha do usuário.
 */
export function UserPasswordModal({
  open,
  user,
  onClose,
  onSubmit,
  addToast,
}) {
  const { form, handleSubmit, isSubmitting } = useUserPasswordForm({
    user,
    onSubmit: async (userId, data) => {
      try {
        await onSubmit(userId, data);
        addToast('Senha alterada com sucesso!', 'success');
      } catch (err) {
        const message = err?.type === 'unauthorized'
          ? 'Senha atual incorreta.'
          : 'Erro ao alterar senha.';
        addToast(message, 'error');
      }
    },
  });

  const {
    register,
    formState: { errors },
  } = form;

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Alterar Senha"
      subtitle={`Alterar senha do usuário ${user?.name || ''}`}
      confirmLabel="Alterar Senha"
      onConfirm={handleSubmit}
      isSubmitting={isSubmitting}
    >
      <div className={style.form}>
        <div className={style.field}>
          <label htmlFor="current-password">Senha Atual</label>
          <input
            id="current-password"
            type="password"
            className={errors.currentPassword ? style.hasError : ''}
            placeholder="Digite a senha atual"
            {...register('currentPassword')}
          />
          {errors.currentPassword && (
            <span className={style.fieldError}>{errors.currentPassword.message}</span>
          )}
        </div>

        <div className={style.field}>
          <label htmlFor="new-password">Nova Senha</label>
          <input
            id="new-password"
            type="password"
            className={errors.newPassword ? style.hasError : ''}
            placeholder="Mínimo 8 caracteres"
            {...register('newPassword')}
          />
          {errors.newPassword && (
            <span className={style.fieldError}>{errors.newPassword.message}</span>
          )}
        </div>

        <div className={style.field}>
          <label htmlFor="confirm-password">Confirmar Nova Senha</label>
          <input
            id="confirm-password"
            type="password"
            className={errors.confirmPassword ? style.hasError : ''}
            placeholder="Repita a nova senha"
            {...register('confirmPassword')}
          />
          {errors.confirmPassword && (
            <span className={style.fieldError}>{errors.confirmPassword.message}</span>
          )}
        </div>
      </div>
    </Modal>
  );
}
