import { z } from 'zod';

export const aiConfigCreateSchema = z.object({
  promptTemplate: z
    .string()
    .min(1, 'O template do prompt é obrigatório')
    .max(2000, 'O template não pode exceder 2000 caracteres'),
  modelName: z
    .string()
    .min(1, 'O nome do modelo é obrigatório')
    .max(100, 'O nome do modelo não pode exceder 100 caracteres'),
  apiKey: z
    .string()
    .min(1, 'A chave de API é obrigatória'),
});

export const aiConfigEditSchema = z.object({
  promptTemplate: z
    .string()
    .min(1, 'O template do prompt é obrigatório')
    .max(2000, 'O template não pode exceder 2000 caracteres'),
  modelName: z
    .string()
    .min(1, 'O nome do modelo é obrigatório')
    .max(100, 'O nome do modelo não pode exceder 100 caracteres'),
  apiKey: z
    .string()
    .optional()
    .or(z.literal('')),
});
