import { z } from "zod";

export const opportunitySchema = z.object({
  leadId: z.union([z.string(), z.number()]).refine(
    (val) => {
      const num = Number(val);
      return !isNaN(num) && num > 0;
    },
    { message: "Lead é obrigatório" },
  ),
  ownerId: z
    .union([z.string(), z.number(), z.literal("")])
    .optional()
    .transform((val) => {
      if (val === "" || val === undefined || val === null) return null;
      return Number(val);
    }),
  productId: z.union([z.string(), z.number()]).refine(
    (val) => {
      const num = Number(val);
      return !isNaN(num) && num > 0;
    },
    { message: "Produto é obrigatório" },
  ),
  stage: z.union([z.string(), z.number()]).refine(
    (val) => {
      const num = Number(val);
      return !isNaN(num) && num >= 0 && num <= 4;
    },
    { message: "Etapa é obrigatória" },
  ),
  amount: z
    .string()
    .min(1, "Valor é obrigatório")
    .refine(
      (val) => {
        const num = Number(val.replace(",", "."));
        return !isNaN(num) && num > 0;
      },
      { message: "Valor deve ser maior que zero" },
    ),
  expectedCloseDate: z.string().min(1, "Data prevista é obrigatória"),
});
