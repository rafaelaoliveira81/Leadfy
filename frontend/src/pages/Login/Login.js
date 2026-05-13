import { Navigate } from "react-router-dom";
import { MdArrowForward, MdLockOutline } from "react-icons/md";
import { Button } from "../../components/ui/Button/Button";
import { isAuthenticated } from "../../services/authStorage";
import { useLoginForm } from "../../features/auth/hooks/useLoginForm";
import style from "./_login.module.css";

export function Login() {
  const { form, submitError, handleSubmit, isSubmitting } = useLoginForm();

  const {
    register,
    formState: { errors },
  } = form;

  if (isAuthenticated()) {
    return <Navigate to="/" replace />;
  }

  return (
    <main className={style.page}>
      <section className={style.brandPanel}>
        <div className={style.brandGlow} aria-hidden="true" />
        <div className={style.brandContent}>
          <span className={style.eyebrow}>CRM Vendas</span>
          <h1 className={style.title}>
            Acesse seu painel com um fluxo simples e seguro.
          </h1>
          <p className={style.description}>
            Entre com sua conta para acompanhar leads, oportunidades, produtos e
            a operação comercial em um só lugar.
          </p>

          <div className={style.metrics}>
            <div className={style.metricCard}>
              <strong>Planos de ação com IA</strong>
              <span>
                Gere próximos passos estratégicos com base no histórico de
                interações com seus leads.
              </span>
            </div>
            <div className={style.metricCard}>
              <strong>Funil sempre organizado</strong>
              <span>
                Visualize cada etapa da jornada comercial e mantenha o time
                alinhado.
              </span>
            </div>
          </div>
        </div>
      </section>

      <section className={style.formPanel}>
        <div className={style.formCard}>
          <div className={style.formHeader}>
            <span className={style.iconBadge}>
              <MdLockOutline />
            </span>
            <h2>Login</h2>
            <p>Use seu e-mail corporativo e senha para entrar no sistema.</p>
          </div>

          <form
            className={style.form}
            onSubmit={(event) => {
              event.preventDefault();
              handleSubmit();
            }}
          >
            {submitError && (
              <div className={style.errorBanner}>{submitError}</div>
            )}

            <div className={style.field}>
              <label htmlFor="login-email">E-mail</label>
              <input
                id="login-email"
                type="email"
                placeholder="voce@empresa.com"
                className={errors.email ? style.hasError : ""}
                {...register("email")}
              />
              {errors.email && (
                <span className={style.fieldError}>{errors.email.message}</span>
              )}
            </div>

            <div className={style.field}>
              <label htmlFor="login-password">Senha</label>
              <input
                id="login-password"
                type="password"
                placeholder="Digite sua senha"
                className={errors.password ? style.hasError : ""}
                {...register("password")}
              />
              {errors.password && (
                <span className={style.fieldError}>
                  {errors.password.message}
                </span>
              )}
            </div>

            <Button type="submit" size="lg" fullWidth disabled={isSubmitting}>
              {isSubmitting ? "Entrando..." : "Entrar no CRM"}
              {!isSubmitting && <MdArrowForward />}
            </Button>
          </form>
        </div>
      </section>
    </main>
  );
}
