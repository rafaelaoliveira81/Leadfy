import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Form from "react-bootstrap/Form";
import { useState } from "react";
import { MdEmail, MdLock, MdArrowForward } from "react-icons/md";
import userApi from "../../services/userApi";

import logo from "../../assets/logo.png";
import style from "./_registro.module.css";
import { Link, useNavigate } from "react-router-dom";

export function Registro() {
  const INITIAL_USER_STATE = {
    name: "",
    email: "",
    password: "",
  };

  const [user, setUser] = useState(INITIAL_USER_STATE);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);

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
      await userApi.Register(user);
      toast.success("Conta criada com sucesso!");
      navigate("/login");
    } catch (error) {
      toast.error(error?.message ?? "Erro ao criar conta");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className={style["pagina-login"]}>
      <section className={style["secao-login"]}>
        <div className={style["caixa-login"]}>
          <h2>Crie sua conta</h2>
          <p>Preencha os campos abaixo para criar sua conta.</p>

          <Form onSubmit={handleSubmit} className={style.form}>
            <Form.Group className={style.formGroup}>
              <Form.Label>Nome</Form.Label>

              <div className={style["campo-input"]}>
                <MdEmail className={style["icone-input"]} />
                <Form.Control
                  type="text"
                  name="name"
                  placeholder="Seu nome"
                  value={user.name}
                  onChange={handleInputChange}
                />
              </div>
            </Form.Group>
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
              {isLoading ? "Carregando..." : "Criar conta"}
              <MdArrowForward />
            </button>
            <hr />
            <p className={style.textoCadastro}>
              Já tem uma conta? <Link to="/login">Faça login</Link>
            </p>
          </Form>
        </div>
      </section>
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
      <ToastContainer />
    </main>
  );
}
