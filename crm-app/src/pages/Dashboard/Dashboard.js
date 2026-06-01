import { useEffect, useState } from "react";
import { Alert, Col, Container, Row, Spinner } from "react-bootstrap";
import style from "./_dashboard.module.css";

import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import { dashboardAPI } from "../../services/dashboardApi";

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

const percentFormatter = new Intl.NumberFormat("pt-BR", {
  minimumFractionDigits: 0,
  maximumFractionDigits: 2,
});

const STAGE_CLASSNAMES = {
  "Novo Lead": "stage-blue",
  "Em contato": "stage-cyan",
  Qualificado: "stage-purple",
  "Proposta Enviada": "stage-orange",
  Negociação: "stage-yellow",
  Ganho: "stage-green",
  Perdido: "stage-red",
};

function formatCurrency(value) {
  return currencyFormatter.format(Number(value || 0));
}

function formatPercent(value) {
  return `${percentFormatter.format(Number(value || 0))}%`;
}

export function Dashboard() {
  const [dashboardData, setDashboardData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    async function loadDashboard() {
      try {
        const response = await dashboardAPI.GetData();
        setDashboardData(response);
      } catch (error) {
        setErrorMessage(error.message);
      } finally {
        setIsLoading(false);
      }
    }

    loadDashboard();
  }, []);

  const summaryCards = [
    {
      title: "Total de leads",
      value: dashboardData?.totalLeads ?? 0,
      tone: "card-blue",
    },
    {
      title: "Em andamento",
      value: dashboardData?.opportunitiesInProgress ?? 0,
      tone: "card-cyan",
    },
    {
      title: "Sem contato",
      value: dashboardData?.newOpportunitiesWithoutContact ?? 0,
      tone: "card-orange",
    },
    {
      title: "Previsão",
      value: formatCurrency(dashboardData?.revenueForecast),
      tone: "card-purple",
    },
    {
      title: "Conversão",
      value: formatPercent(dashboardData?.conversionRate),
      tone: "card-green",
    },
    {
      title: "Receita real",
      value: formatCurrency(dashboardData?.realRevenue),
      tone: "card-dark-green",
    },
  ];

  const stages = dashboardData?.stageAnalytics ?? [];
  const maxQuantity = Math.max(...stages.map((s) => s.quantity), 0);

  return (
    <Sidebar>
      <Topbar>
        <Container fluid className={style["dashboard-container"]}>
          <div className={style["dashboard-header"]}>
            <h1>Dashboard</h1>
            <p>Resumo básico dos indicadores comerciais.</p>
          </div>

          {isLoading && (
            <div className={style["loading-box"]}>
              <Spinner animation="border" size="sm" />
              <span>Carregando dashboard...</span>
            </div>
          )}

          {!isLoading && errorMessage && (
            <Alert variant="danger">{errorMessage}</Alert>
          )}

          {!isLoading && !errorMessage && (
            <>
              <Row className="g-3 mb-4">
                {summaryCards.map((card) => (
                  <Col key={card.title} xs={12} md={6} lg={4}>
                    <div
                      className={`${style["dashboard-card"]} ${style[card.tone]}`}
                    >
                      <span>{card.title}</span>
                      <h3>{card.value}</h3>
                    </div>
                  </Col>
                ))}
              </Row>

              <Row className="g-3">
                <Col lg={8}>
                  <div className={style["dashboard-panel"]}>
                    <h4>Quantidade por etapa</h4>

                    {stages.map((stage) => {
                      const stageClassName =
                        STAGE_CLASSNAMES[stage.stageName] || "stage-dark-green";

                      return (
                        <div
                          key={stage.stageName}
                          className={style["stage-item"]}
                        >
                          <div className={style["stage-info"]}>
                            <span>{stage.stageName}</span>
                            <strong>{stage.quantity}</strong>
                          </div>

                          <progress
                            className={`${style["progress-custom"]} ${style[stageClassName]}`}
                            value={stage.quantity}
                            max={maxQuantity || 1}
                          />
                        </div>
                      );
                    })}
                  </div>
                </Col>

                <Col lg={4}>
                  <div className={style["dashboard-panel"]}>
                    <h4>Valores por etapa</h4>

                    {stages.map((stage) => {
                      const stageClassName =
                        STAGE_CLASSNAMES[stage.stageName] || "stage-dark-green";

                      return (
                        <div
                          key={stage.stageName}
                          className={style["value-row"]}
                        >
                          <div className={style["value-left"]}>
                            <div
                              className={`${style["stage-dot"]} ${style[stageClassName]}`}
                            />

                            {stage.stageName}
                          </div>

                          <strong>{formatCurrency(stage.totalValue)}</strong>
                        </div>
                      );
                    })}
                  </div>
                </Col>
              </Row>
            </>
          )}
        </Container>
      </Topbar>
    </Sidebar>
  );
}
