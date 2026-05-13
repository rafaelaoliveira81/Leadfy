import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { ownerSchema } from "../schemas/owner.schema";

/**
 * Hook do formulário de criação/edição de owner.
 * @param {object} options
 * @param {'create'|'edit'} options.mode
 * @param {object|null} [options.owner] - Owner existente (edição).
 * @param {(data: object) => Promise<void>} options.onSubmit - Callback de submit.
 */
export function useOwnerForm({ mode, owner, onSubmit }) {
  const isEdit = mode === "edit";

  const defaultValues =
    isEdit && owner
      ? {
          name: owner.name ?? "",
          userID: owner.userID ?? "",
        }
      : {
          name: "",
          userID: "",
        };

  const form = useForm({
    resolver: zodResolver(ownerSchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      name: values.name.trim(),
      userID: Number(values.userID),
    };

    if (isEdit && owner) {
      payload.isActive = owner.isActive;
      await onSubmit(owner.id, payload);
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
