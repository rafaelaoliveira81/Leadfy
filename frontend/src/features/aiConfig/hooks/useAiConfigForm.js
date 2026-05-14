import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  aiConfigCreateSchema,
  aiConfigEditSchema,
} from "../schemas/aiConfig.schema";

export function useAiConfigForm({ mode, config, onSubmit }) {
  const isEdit = mode === "edit";

  const defaultValues =
    isEdit && config
      ? {
          title: config.title ?? "",
          promptTemplate: config.promptTemplate ?? "",
          model: config.model ?? "",
          apiKey: "",
        }
      : {
          title: "",
          promptTemplate: "",
          model: "",
          apiKey: "",
        };

  const form = useForm({
    resolver: zodResolver(isEdit ? aiConfigEditSchema : aiConfigCreateSchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      title: values.title.trim(),
      promptTemplate: values.promptTemplate.trim(),
      model: Number(values.model),
    };

    if (values.apiKey && values.apiKey.trim()) {
      payload.apiKey = values.apiKey.trim();
    }

    if (isEdit && config) {
      await onSubmit(config.id, payload);
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
