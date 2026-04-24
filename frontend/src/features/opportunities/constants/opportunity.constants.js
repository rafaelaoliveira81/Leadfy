export const OPPORTUNITY_PAGE_SIZES = [10, 25, 50, 100];

export const OPPORTUNITY_DEFAULT_PAGE_SIZE = 10;

export const OPPORTUNITY_STATUS_OPTIONS = [
  { value: "all", label: "Todos" },
  { value: "active", label: "Ativos" },
  { value: "inactive", label: "Inativos" },
];

export const OPPORTUNITY_STAGE_OPTIONS = [
  { value: 1, label: "Novo" },
  { value: 2, label: "Em contato" },
  { value: 3, label: "Qualificado" },
  { value: 4, label: "Proposta" },
  { value: 5, label: "Negociação" },
  { value: 6, label: "Ganho" },
  { value: 7, label: "Perdido" },
];

export const OPPORTUNITY_STAGE_MAP = {
  New: "Novo",
  Qualified: "Qualificado",
  Proposal: "Proposta",
  Negotiation: "Negociação",
  Closed: "Fechado",
};

export const OPPORTUNITY_SEARCH_DEBOUNCE_MS = 400;
