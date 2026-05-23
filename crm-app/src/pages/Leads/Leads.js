import { useState, useEffect, useCallback } from "react";
import { leadAPI } from "../../services/leadApi";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
// import { Toast } from "../../components/Toast/Toast";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import style from "./_leads.module.css";

import {
  MdAdd,
  MdEdit,
  MdDelete,
  MdChevronLeft,
  MdChevronRight,
} from "react-icons/md";

const ITEMS_PER_PAGE = 10;

export function Leads() {
  const [allLeads, setAllLeads] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);

  const totalPages = Math.max(1, Math.ceil(allLeads.length / ITEMS_PER_PAGE));
  const paginatedLeads = allLeads.slice(
    (currentPage - 1) * ITEMS_PER_PAGE,
    currentPage * ITEMS_PER_PAGE,
  );

  const getPageNumbers = () => {
    const maxVisible = 5;
    let start = Math.max(1, currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(totalPages, start + maxVisible - 1);
    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }
    const pages = [];
    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  };

  const fetchLeads = useCallback(async () => {
    setIsLoading(true);
    try {
      const data = await leadAPI.GetAll();
      setAllLeads(Array.isArray(data) ? data : []);
      setCurrentPage(1);
      toast.success("Leads carregados com sucesso!");
    } catch (err) {
      toast.error("Erro ao carregar os leads. Tente novamente.");
      setAllLeads([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchLeads();
  }, [fetchLeads]);

  return (
    <Sidebar>
      <Topbar>
        <div className={style["pagina-listagem"]}>
          <header className={style["listagem-header"]}>
            <div>
              <h1>Leads</h1>
              <p>Gerencie os leads cadastrados no CRM.</p>
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
                  <th className={style["coluna-acoes"]}>Ações</th>
                </tr>
              </thead>
              <tbody>
                {isLoading ? (
                  <tr>
                    <td
                      colSpan={4}
                      style={{
                        textAlign: "center",
                        padding: "32px",
                        color: "#64748b",
                      }}
                    >
                      Carregando...
                    </td>
                  </tr>
                ) : (
                  paginatedLeads.map((lead) => (
                    <tr key={lead.id}>
                      <td>{lead.name}</td>
                      <td>{lead.email}</td>
                      <td>{lead.phoneNumber}</td>
                      <td className={style["acoes"]}>
                        <button className={style["botao-editar"]}>
                          <MdEdit />
                        </button>
                        <button className={style["botao-excluir"]}>
                          <MdDelete />
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>{" "}
            </table>

            <footer className={style["paginacao"]}>
              <span>
                Página {currentPage} de {totalPages}
              </span>

              <div className={style["paginacao-acoes"]}>
                <button
                  onClick={() => setCurrentPage((p) => p - 1)}
                  disabled={currentPage === 1}
                >
                  <MdChevronLeft />
                </button>
                {getPageNumbers().map((page) => (
                  <button
                    key={page}
                    onClick={() => setCurrentPage(page)}
                    className={
                      page === currentPage ? style["pagina-ativa"] : undefined
                    }
                  >
                    {page}
                  </button>
                ))}
                <button
                  onClick={() => setCurrentPage((p) => p + 1)}
                  disabled={currentPage === totalPages}
                >
                  <MdChevronRight />
                </button>
              </div>
            </footer>
          </section>
          <ToastContainer position="top-right" autoClose={3000} />
        </div>
      </Topbar>
    </Sidebar>
  );
}
