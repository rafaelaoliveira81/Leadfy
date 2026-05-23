import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import style from "./_home.module.css";
import {
  MdAdd,
  MdEdit,
  MdDelete,
  MdChevronLeft,
  MdChevronRight,
} from "react-icons/md";

export function Home() {
  const clientes = [
    {
      id: 1,
      nome: "Empresa Alpha",
      email: "contato@alpha.com",
      telefone: "(35) 99999-0001",
      status: "Ativo",
    },
    {
      id: 2,
      nome: "Cliente Beta",
      email: "beta@email.com",
      telefone: "(35) 99999-0002",
      status: "Pendente",
    },
    {
      id: 3,
      nome: "Grupo Delta",
      email: "comercial@delta.com",
      telefone: "(35) 99999-0003",
      status: "Inativo",
    },
  ];

  return (
    <Sidebar>
      <Topbar>
        <div className={style["pagina-listagem"]}>
          <header className={style["listagem-header"]}>
            <div>
              <h1>Clientes</h1>
              <p>Gerencie os clientes cadastrados no CRM.</p>
            </div>

            <button className={style["botao-novo"]}>
              <MdAdd />
              Novo cliente
            </button>
          </header>

          <section className={style["tabela-card"]}>
            <table className={style["tabela"]}>
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>E-mail</th>
                  <th>Telefone</th>
                  <th>Status</th>
                  <th className={style["coluna-acoes"]}>Ações</th>
                </tr>
              </thead>

              <tbody>
                {clientes.map((cliente) => (
                  <tr key={cliente.id}>
                    <td>{cliente.nome}</td>
                    <td>{cliente.email}</td>
                    <td>{cliente.telefone}</td>
                    <td>
                      <span
                        className={`${style["badge"]} ${style[cliente.status.toLowerCase()]}`}
                      >
                        {cliente.status}
                      </span>
                    </td>
                    <td className={style["acoes"]}>
                      <button className={style["botao-editar"]}>
                        <MdEdit />
                      </button>
                      <button className={style["botao-excluir"]}>
                        <MdDelete />
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <footer className={style["paginacao"]}>
              <span>Página 1 de 10</span>

              <div className={style["paginacao-acoes"]}>
                <button>
                  <MdChevronLeft />
                </button>
                <button className={style["pagina-ativa"]}>1</button>
                <button>2</button>
                <button>3</button>
                <button>
                  <MdChevronRight />
                </button>
              </div>
            </footer>
          </section>
        </div>
      </Topbar>
    </Sidebar>
  );
}
