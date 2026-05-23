import { Link } from "react-router-dom";

function NotFound() {
  return (
    <div style={styles.container}>
      <h1 style={styles.code}>404</h1>
      <h2>Página não encontrada</h2>
      <p>A rota que você tentou acessar não existe.</p>

      <Link to="/Home" style={styles.button}>
        Voltar para Home
      </Link>
    </div>
  );
}

const styles = {
  container: {
    height: "100vh",
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    textAlign: "center",
    backgroundColor: "#f4f4f4",
  },
  code: {
    fontSize: "120px",
    margin: 0,
    color: "#ff4d4f",
  },
  button: {
    marginTop: "20px",
    padding: "10px 20px",
    backgroundColor: "#1890ff",
    color: "#fff",
    textDecoration: "none",
    borderRadius: "5px",
  },
};

export default NotFound;
