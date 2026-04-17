import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { productSchema } from '../schemas/product.schema';
import { priceToNumber, priceToFormValue } from '../mappers/product.mapper';

/**
 * Hook do formulário de criação/edição de produto.
 * @param {object} options
 * @param {'create'|'edit'} options.mode
 * @param {object|null} [options.product] - Produto existente (edição).
 * @param {(data: object) => Promise<void>} options.onSubmit - Callback de submit.
 */
export function useProductForm({ mode, product, onSubmit }) {
  const isEdit = mode === 'edit';

  const defaultValues = isEdit && product
    ? {
        name: product.name ?? '',
        description: product.description ?? '',
        price: priceToFormValue(product.price),
      }
    : {
        name: '',
        description: '',
        price: '',
      };

  const form = useForm({
    resolver: zodResolver(productSchema),
    defaultValues,
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    const payload = {
      name: values.name.trim(),
      description: values.description?.trim() || '',
      price: priceToNumber(values.price),
    };

    if (isEdit && product) {
      payload.isActive = product.isActive;
      await onSubmit(product.id, payload);
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
