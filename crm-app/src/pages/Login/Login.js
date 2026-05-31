import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Form from "react-bootstrap/Form";
import { useState } from "react";
import { MdEmail, MdLock, MdArrowForward } from "react-icons/md";
import { useAuth } from "../../context/AuthContext";

import logo from "../../assets/logo.png";
import style from "./_login.module.css";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const INITIAL_USER_STATE = {
    email: "",
    password: "",
  };

  const [user, setUser] = useState(INITIAL_USER_STATE);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const { login } = useAuth();

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    setUser((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      await login(user);
      toast.success("Login feito com sucesso!");
      navigate("/leads", { replace: true });
    } catch (error) {
      toast.error(error?.message ?? "Erro ao efetuar login");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className={style["pagina-login"]}>
      <section className={style["secao-marca"]}>
        <div className={style.logoArea}>
          <img src={logo} alt="Leadfy" className={style.logo} />
        </div>

        <div className={style["conteudo-marca"]}>
          <h1>
            Transforme leads em <em>receita previsível.</em>
          </h1>
          <div className={style["linha-detalhe"]} />
        </div>

        <div className={style["container-cards"]}>
          <div className={style.card}>
            <h1>Planos de ação com IA</h1>
            <span>
              Gere próximos passos estratégicos com base no histórico de
              interações com seus leads.
            </span>
          </div>
          <div className={style.card}>
            <h1>Funil sempre organizado</h1>
            <span>
              Visualize cada etapa da jornada comercial e mantenha o time
              alinhado.
            </span>
          </div>
        </div>
      </section>

      <section className={style["secao-login"]}>
        <div className={style["caixa-login"]}>
          <h2>Bem-vindo de volta</h2>
          <p>Entre na sua conta para continuar.</p>

          <Form onSubmit={handleSubmit} className={style.form}>
            <Form.Group className={style.formGroup}>
              <Form.Label>E-mail</Form.Label>

              <div className={style["campo-input"]}>
                <MdEmail className={style["icone-input"]} />
                <Form.Control
                  type="email"
                  name="email"
                  placeholder="voce@empresa.com"
                  value={user.email}
                  onChange={handleInputChange}
                />
              </div>
            </Form.Group>

            <Form.Group className={style.formGroup}>
              <Form.Label>Senha</Form.Label>

              <div className={style["campo-input"]}>
                <MdLock className={style["icone-input"]} />
                <Form.Control
                  type="password"
                  name="password"
                  placeholder="Digite sua senha"
                  value={user.password}
                  onChange={handleInputChange}
                  maxLength={15}
                />
              </div>
            </Form.Group>
            <button
              type="submit"
              className={style.botaoEntrar}
              disabled={isLoading}
            >
              {isLoading ? "Carregando..." : "Entrar"}
              <MdArrowForward />
            </button>
          </Form>
        </div>
        <ToastContainer />
      </section>
    </main>
  );
}
