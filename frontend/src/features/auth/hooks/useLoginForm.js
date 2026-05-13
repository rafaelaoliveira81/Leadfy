import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useNavigate } from 'react-router-dom';
import { useToast } from '../../../components/ui/Toast/Toast';
import authApi from '../../../services/authApi';
import { setAuthSession } from '../../../services/authStorage';
import { loginSchema } from '../schemas/login.schema';

export function useLoginForm() {
  const navigate = useNavigate();
  const addToast = useToast();
  const [submitError, setSubmitError] = useState('');

  const form = useForm({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const handleSubmit = form.handleSubmit(async (values) => {
    setSubmitError('');

    const response = await authApi.login({
      email: values.email.trim(),
      password: values.password,
    });

    if (!response?.token) {
      throw new Error('A autenticação não retornou um token válido.');
    }

    setAuthSession({
      token: response.token,
      name: response.name,
    });

    addToast(response.message || 'Login realizado com sucesso.', 'success');
    navigate('/', { replace: true });
  }, (errors) => {
    if (errors.email?.message || errors.password?.message) {
      setSubmitError('Revise os campos obrigatórios para continuar.');
    }
  });

  const submitWithHandling = async () => {
    try {
      await handleSubmit();
    } catch (error) {
      setSubmitError(error.message || 'Não foi possível fazer login.');
    }
  };

  return {
    form,
    submitError,
    handleSubmit: submitWithHandling,
    isSubmitting: form.formState.isSubmitting,
  };
}
