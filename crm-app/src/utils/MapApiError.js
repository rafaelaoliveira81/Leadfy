export default function mapApiError(error) {
  if (!error.response) {
    throw {
      type: "network",
      message: "Erro de rede ou servidor indisponível",
    };
  }

  const { status, data } = error.response;

  if (status === 400) {
    throw {
      type: "validation",
      message: data?.message || "Dados inválidos",
      errors: data?.errors || null,
    };
  }

  if (status === 404) {
    throw {
      type: "not_found",
      message: data?.message || "Recurso não encontrado",
    };
  }

  if (status === 500) {
    throw {
      type: "server",
      message: data?.message || "Erro interno. Tente novamente mais tarde.",
    };
  }

  throw {
    type: "unknown",
    message: data?.message || "Erro inesperado",
  };
}
