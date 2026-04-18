import { z } from 'zod';

export const owerSchema = z.object({
  name: z
    .string()
    .min(1, 'Nome é obrigatório')
    .max(150, 'Nome deve ter no máximo 150 caracteres'),
  userID: z
    .union([z.string(), z.number()])
    .refine(
      (val) => {
        const num = Number(val);
        return !isNaN(num) && num > 0;
      },
      { message: 'Usuário é obrigatório' }
    ),
});
