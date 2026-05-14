import { z } from "zod";

export const aiConfigCreateSchema = z.object({
  title: z
    .string()
    .min(1, "O título é obrigatório")
    .max(150, "O título não pode exceder 150 caracteres"),
  promptTemplate: z
    .string()
    .min(1, "O template do prompt é obrigatório")
    .max(2000, "O template não pode exceder 2000 caracteres"),
  model: z
    .number({ invalid_type_error: "Selecione um modelo" })
    .int()
    .positive("Selecione um modelo"),
  apiKey: z.string().min(1, "A chave de API é obrigatória"),
});

export const aiConfigEditSchema = z.object({
  title: z
    .string()
    .min(1, "O título é obrigatório")
    .max(150, "O título não pode exceder 150 caracteres"),
  promptTemplate: z
    .string()
    .min(1, "O template do prompt é obrigatório")
    .max(2000, "O template não pode exceder 2000 caracteres"),
  model: z
    .number({ invalid_type_error: "Selecione um modelo" })
    .int()
    .positive("Selecione um modelo"),
  apiKey: z.string().optional().or(z.literal("")),
});
