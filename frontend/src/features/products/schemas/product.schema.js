import { z } from 'zod';

export const productSchema = z.object({
  name: z
    .string()
    .min(1, 'Nome é obrigatório')
    .max(150, 'Nome deve ter no máximo 150 caracteres'),
  description: z
    .string()
    .max(1000, 'Descrição deve ter no máximo 1000 caracteres')
    .optional()
    .or(z.literal('')),
  price: z
    .string()
    .min(1, 'Preço é obrigatório')
    .refine(
      (val) => {
        const num = Number(val.replace(',', '.'));
        return !isNaN(num) && num > 0;
      },
      { message: 'Preço deve ser maior que zero' }
    ),
});
