import { useEffect, useRef, useState } from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import { toast } from "react-toastify";
import style from "./_sidebar.module.css";
import { SidebarItem } from "../SidebarItem/SidebarItem";
import { Link, useNavigate } from "react-router-dom";
import { MdOutlineDashboard } from "react-icons/md";
import { MdSmartToy } from "react-icons/md";
import { MdViewKanban } from "react-icons/md";
import { MdShoppingCart } from "react-icons/md";
import { MdPersonSearch } from "react-icons/md";
import { MdPeople } from "react-icons/md";
import { MdKeyboardArrowUp } from "react-icons/md";
import { MdKeyboardArrowDown } from "react-icons/md";
import { MdLockOutline } from "react-icons/md";
import { MdLogout } from "react-icons/md";
import { MdPerson } from "react-icons/md";
import logo from "../../assets/logo.png";
import { useAuth } from "../../context/AuthContext";
import userApi from "../../services/userApi";
import { Button } from "../../components/Button/Button";

export function Sidebar({ children }) {
  const navigate = useNavigate();
  const { claims, logout } = useAuth();
  const menuRef = useRef(null);
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  const [isPasswordModalOpen, setIsPasswordModalOpen] = useState(false);
  const [isSubmittingPassword, setIsSubmittingPassword] = useState(false);
  const [passwordForm, setPasswordForm] = useState({
    currentPassword: "",
    newPassword: "",
  });

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (menuRef.current && !menuRef.current.contains(event.target)) {
        setIsUserMenuOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleLogout = () => {
    logout();
    toast.success("Logout efetuado com sucesso!");
    navigate("/login");
  };

  const handlePasswordInputChange = (event) => {
    const { name, value } = event.target;

    setPasswordForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const closePasswordModal = () => {
    setIsPasswordModalOpen(false);
    setPasswordForm({
      currentPassword: "",
      newPassword: "",
    });
  };

  const openPasswordModal = () => {
    setIsUserMenuOpen(false);
    setIsPasswordModalOpen(true);
  };

  const handleChangePassword = async (event) => {
    event.preventDefault();

    const currentPassword = passwordForm.currentPassword.trim();
    const newPassword = passwordForm.newPassword.trim();
    const authenticatedUserId = Number(claims?.usuarioId);

    if (!currentPassword || !newPassword) {
      toast.error("Preencha a senha atual e a nova senha.");
      return;
    }

    if (!authenticatedUserId) {
      toast.error("Não foi possível identificar o usuário autenticado.");
      return;
    }

    setIsSubmittingPassword(true);

    try {
      await userApi.UpdatePassword(authenticatedUserId, {
        currentPassword,
        newPassword,
      });

      toast.success("Senha alterada com sucesso.");
      closePasswordModal();
    } catch (error) {
      toast.error(error?.message || "Erro ao alterar a senha.");
    } finally {
      setIsSubmittingPassword(false);
    }
  };

  return (
    <div className={style.container}>
      <div className={style["container-sidebar"]}>
        <div className={style["header"]}>
          <Link to="/dashboard">
            <img src={logo} alt="Logo-Leadfy" className={style.logo} />
          </Link>
        </div>
        <hr className={style.divider} />
        <div className={style.nav}>
          <SidebarItem
            texto="Dashboard"
            link="/dashboard"
            logo={<MdOutlineDashboard />}
          />
          <SidebarItem texto="Kanban" link="/kanban" logo={<MdViewKanban />} />
          <hr className={style.divider} />
          <SidebarItem texto="Leads" link="/leads" logo={<MdPersonSearch />} />
          <SidebarItem
            texto="Produtos"
            link="/produtos"
            logo={<MdShoppingCart />}
          />
          <SidebarItem texto="Usuários" link="/usuarios" logo={<MdPeople />} />
          <hr className={style.divider} />
          <SidebarItem texto="Prompts" link="/prompts" logo={<MdSmartToy />} />
          <hr className={style.divider} />
        </div>

        <hr className={style.divider} />
        <div className={style["sidebar-usuario-wrapper"]} ref={menuRef}>
          <button
            type="button"
            className={style["sidebar-usuario"]}
            onClick={() => setIsUserMenuOpen((prev) => !prev)}
          >
            <div className={style["sidebar-usuario-info"]}>
              <MdPerson />
              <span>{claims?.nome || "Usuário"}</span>
            </div>
            {isUserMenuOpen ? <MdKeyboardArrowUp /> : <MdKeyboardArrowDown />}
          </button>

          {isUserMenuOpen && (
            <div className={style["sidebar-usuario-menu"]}>
              <button
                type="button"
                className={style["sidebar-usuario-menu-item"]}
                onClick={openPasswordModal}
              >
                <MdLockOutline />
                <span>Trocar senha</span>
              </button>
              <button
                type="button"
                className={style["sidebar-usuario-menu-item"]}
                onClick={handleLogout}
              >
                <MdLogout />
                <span>Logout</span>
              </button>
            </div>
          )}
        </div>
      </div>
      <div className={style["pagina-conteudo"]}>{children}</div>

      <Modal show={isPasswordModalOpen} onHide={closePasswordModal}>
        <Modal.Header closeButton>
          <Modal.Title>Trocar senha</Modal.Title>
        </Modal.Header>
        <Form onSubmit={handleChangePassword}>
          <Modal.Body>
            <Form.Group className="mb-3" controlId="currentPassword">
              <Form.Label>Senha atual</Form.Label>
              <Form.Control
                type="password"
                name="currentPassword"
                value={passwordForm.currentPassword}
                onChange={handlePasswordInputChange}
                placeholder="Digite sua senha atual"
              />
            </Form.Group>

            <Form.Group controlId="newPassword">
              <Form.Label>Nova senha</Form.Label>
              <Form.Control
                type="password"
                name="newPassword"
                value={passwordForm.newPassword}
                onChange={handlePasswordInputChange}
                placeholder="Digite a nova senha"
              />
            </Form.Group>
          </Modal.Body>
          <Modal.Footer>
            <Button
                  variant="secondary"
                  buttonLabel="Cancelar"
                  onButtonClick={closePasswordModal}
                  disabled={isSubmittingPassword}
                />
              <Button
                  variant="success"
                  type="submit"
                  buttonLabel={isSubmittingPassword ? "Salvando..." : "Salvar"}
                  disabled={isSubmittingPassword}
                />
          </Modal.Footer>
        </Form>
      </Modal>
    </div>
  );
}
