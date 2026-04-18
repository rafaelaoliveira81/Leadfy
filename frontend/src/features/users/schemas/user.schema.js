import { z } from 'zod';

export const userSchema = z.object({
  name: z
    .string()
    .min(1, 'Nome é obrigatório')
    .max(150, 'Nome deve ter no máximo 150 caracteres'),
  email: z
    .string()
    .min(1, 'E-mail é obrigatório')
    .email('E-mail inválido'),
  idRole: z
    .union([z.string(), z.number()])
    .refine(
      (val) => {
        const num = Number(val);
        return !isNaN(num) && num > 0;
      },
      { message: 'Perfil é obrigatório' }
    ),
});

export const userCreateSchema = userSchema.extend({
  password: z
    .string()
    .min(8, 'Senha deve ter no mínimo 8 caracteres'),
});

export const passwordSchema = z
  .object({
    currentPassword: z
      .string()
      .min(1, 'Senha atual é obrigatória'),
    newPassword: z
      .string()
      .min(8, 'Nova senha deve ter no mínimo 8 caracteres'),
    confirmPassword: z
      .string()
      .min(1, 'Confirmação de senha é obrigatória'),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: 'As senhas não coincidem',
    path: ['confirmPassword'],
  });
