import { AI_CONFIG_PROMPT_PLACEHOLDERS } from "../constants/aiConfig.constants";
import style from "./_aiConfigForm.module.css";

export function AiConfigForm({ form, isEdit, models = [] }) {
  const {
    register,
    formState: { errors },
  } = form;

  return (
    <div className={style.form}>
      <div className={style.field}>
        <label htmlFor="ai-title">Título</label>
        <input
          id="ai-title"
          type="text"
          className={errors.title ? style.hasError : ""}
          placeholder="Ex: Configuração GPT-4.1 — Vendas"
          {...register("title")}
        />
        {errors.title && (
          <span className={style.fieldError}>{errors.title.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="ai-model">Modelo de IA</label>
        <select
          id="ai-model"
          className={errors.model ? style.hasError : ""}
          {...register("model", { valueAsNumber: true })}
        >
          <option value="">Selecione um modelo</option>
          {models.map((m) => (
            <option key={m.id} value={m.id}>
              {m.label}
            </option>
          ))}
        </select>
        {errors.model && (
          <span className={style.fieldError}>{errors.model.message}</span>
        )}
      </div>

      <div className={style.field}>
        <label htmlFor="ai-prompt-template">Template do Prompt</label>
        <textarea
          id="ai-prompt-template"
          className={errors.promptTemplate ? style.hasError : ""}
          placeholder="Ex: Você é um assistente de vendas. Crie um plano de ação para converter o lead {{LeadName}}..."
          {...register("promptTemplate")}
        />
        {errors.promptTemplate && (
          <span className={style.fieldError}>
            {errors.promptTemplate.message}
          </span>
        )}
        <p className={style.hint}>
          Placeholders disponíveis:{" "}
          {AI_CONFIG_PROMPT_PLACEHOLDERS.map((p) => (
            <code key={p} className={style.placeholder}>
              {p}
            </code>
          ))}
        </p>
      </div>

      <div className={style.field}>
        <label htmlFor="ai-api-key">
          Chave de API (GitHub Personal Access Token)
          {isEdit && <span className={style.optionalLabel}> — opcional</span>}
        </label>
        <input
          id="ai-api-key"
          type="password"
          autoComplete="new-password"
          className={errors.apiKey ? style.hasError : ""}
          placeholder={
            isEdit ? "Deixe em branco para manter a chave atual" : "ghp_..."
          }
          {...register("apiKey")}
        />
        {errors.apiKey && (
          <span className={style.fieldError}>{errors.apiKey.message}</span>
        )}
        <p className={style.hint}>
          A chave é armazenada de forma criptografada e nunca é exibida.
        </p>
      </div>
    </div>
  );
}
