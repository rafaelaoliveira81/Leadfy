import { useState, useCallback } from "react";
import opportunityAPI from "../../../../services/opportunityApi";
import aiConfigApi from "../../../../services/aiConfigApi";

export function useActionPlan() {
  const [isGenerating, setIsGenerating] = useState(false);
  const [error, setError] = useState(null);

  const fetchActiveConfig = useCallback(async () => {
    const config = await aiConfigApi.GetActive();
    if (config?.type)
      throw new Error(config.message || "Configuração de IA não encontrada.");
    return config;
  }, []);

  const generatePlan = useCallback(async (opportunityId, configId) => {
    setIsGenerating(true);
    setError(null);
    try {
      const result = await opportunityAPI.GenerateActionPlan(
        opportunityId,
        configId,
      );
      if (result?.type)
        throw new Error(result.message || "Erro ao gerar plano de ação.");
      return result;
    } catch (err) {
      setError(err?.message || "Erro ao gerar plano de ação.");
      throw err;
    } finally {
      setIsGenerating(false);
    }
  }, []);

  return { isGenerating, error, fetchActiveConfig, generatePlan };
}
